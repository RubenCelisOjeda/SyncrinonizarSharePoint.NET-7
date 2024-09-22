using ahpeSynPagPro.Application.Interfaces.PagoProveedores;
using System.Text.RegularExpressions;

namespace ahpeSynPagPro
{
    public class Worker : BackgroundService
    {
        #region [Properties]
        private readonly IConfiguration _configuration; 
        private readonly ILogger<Worker> _logger;
        private readonly IPagoProveedorApplication _pagoProveedorApplication;
        #endregion

        #region [Constructor]
        public Worker(ILogger<Worker> logger, IConfiguration configuration, IPagoProveedorApplication pagoProveedorApplication)
        {
            _logger = logger;
            _configuration = configuration;
            _pagoProveedorApplication = pagoProveedorApplication;
        }
        #endregion

        #region[Main]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delayTime = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string? sourceDirectory = _configuration?["ConfigurationPath:PathOneDrive"];
                    delayTime = Convert.ToInt32(_configuration?["Configuration:DelayTime"]);
                    string preservePattern = @"^\d{9}-\d{2}-[A-Z]\d{3}-\d{5}$";

                    await MoveFilesRecursively(sourceDirectory, preservePattern);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al descargar el archivo desde OneDrive.");
                }

                await Task.Delay(delayTime, stoppingToken); // Esperar 1 minuto antes de volver a verificar
            }
        } 
        #endregion

        #region [Functions]
        /// <summary>
        /// Funcion que permite
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destinationDirectory"></param>
        private async Task MoveFilesRecursively(string sourceDirectory, string preservePattern)
        {
            string[] ignoredExtensions = { ".ini" };
            string folderName = string.Empty;

            Regex preserveRegex = new Regex(preservePattern, RegexOptions.Compiled);

            foreach (var filePath in Directory.GetFiles(sourceDirectory))
            {
                string fileExtension = Path.GetExtension(filePath);
                if (Array.Exists(ignoredExtensions, ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogInformation($"Archivo {filePath} ignorado debido a su extensión.");
                    continue;
                }

                string fileName = Path.GetFileName(filePath);
                if (string.IsNullOrEmpty(folderName))
                    folderName = Path.GetFileNameWithoutExtension(filePath);

                try
                {
                    var responseData = await _pagoProveedorApplication.AddFileAzureBlobStorage(filePath, folderName, fileName);
                    if (responseData)
                    {
                        File.Delete(filePath);
                        _logger.LogInformation($"Archivo {filePath} subido a Azure Blob Storage {DateTime.Now}");
                    }
                    else
                    {
                        _logger.LogInformation($"No se pudo subir el archivo {filePath}, intente de nuevo");
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogError(ex, $"Acceso denegado al archivo {filePath}. Verifique los permisos.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ocurrió un error al subir el archivo {filePath}.");
                }
            }

            foreach (var directory in Directory.GetDirectories(sourceDirectory))
            {
                await MoveFilesRecursively(directory, preservePattern);

                try
                {
                    // Obtener el nombre del directorio
                    string directoryName = new DirectoryInfo(directory).Name;

                    // Verificar si el nombre del directorio coincide con el patrón que queremos preservar
                    if (!preserveRegex.IsMatch(directoryName))
                    {
                        if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                        {
                            Directory.Delete(directory);
                            _logger.LogInformation($"Directorio {directory} eliminado a las {DateTime.Now}");
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogError(ex, $"Acceso denegado al eliminar el directorio {directory}. Verifique los permisos.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ocurrió un error al eliminar el directorio {directory}.");
                }
            }
        }
        #endregion
    }
}
