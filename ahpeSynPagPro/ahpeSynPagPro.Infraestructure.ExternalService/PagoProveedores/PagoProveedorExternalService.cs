using ahpeSynPagPro.Infraestructure.ExternalService.Azure.Interfaces.PagoProveedores;
using ahpeSynPagPro.Transversal.Common._01.Common._02.Settings._02.Jwt;
using Azure.Storage;
using Azure.Storage.Blobs;
using System.Text.RegularExpressions;

namespace ahpeSynPagPro.Infraestructure.ExternalService.Azure.Main.PagoProveedores
{
    public class PagoProveedorExternalService : IPagoProveedorExternalService
    {
        #region [Properties]
        private readonly string? _blobServiceEndpoint;
        private readonly string? _storageAccountName;
        private readonly string? _storageAccountKey;
        private readonly string? _containerNameDev;
        private readonly string? _containerNameHistorialDev;
        #endregion

        #region [Constructor]
        public PagoProveedorExternalService()
        {
            _blobServiceEndpoint = $"https://{AppSettings.StorageAccountName}.blob.core.windows.net";
            _storageAccountName = AppSettings.StorageAccountName;
            _storageAccountKey = AppSettings.StorageAccountKey;
            _containerNameDev = AppSettings.ContainerNameDev;
            _containerNameHistorialDev = AppSettings.ContainerNameHistorialDev;
        }
        #endregion

        #region [Methods]
        public async Task<bool> AddFileAzureBlobStorage(string filePath,string folderName, string fileName)
        {
            bool response = false;

            // Obtén el año y el mes actuales
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.ToString("MM_MMMM", System.Globalization.CultureInfo.InvariantCulture);

            // Construye la ruta del blob
            string newFileName = FormatFileNameAzure(fileName);
            string blobPath = $"FactCompra/{year}/{month}/{folderName}/{newFileName}";

            //1.Container Dev
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(_blobServiceEndpoint), new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey));
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerNameDev);
            BlobClient blobClient = containerClient.GetBlobClient(blobPath);

            using FileStream uploadFileStream = File.OpenRead(filePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            response = true;

            await AddFileAzureBlobStorageHistorialDev(filePath,folderName,fileName);

            return response;
        }

        public async Task<bool> AddFileAzureBlobStorageHistorialDev(string filePath, string folderName, string fileName)
        {
            bool response = false;

            // Obtén el año y el mes actuales
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.ToString("MM_MMMM", System.Globalization.CultureInfo.InvariantCulture);

            // Construye la ruta del blob
            string newFileName = FormatFileNameAzure(fileName);
            string blobPath = $"FactCompra/{year}/{month}/{folderName}/{newFileName}";

            //1.Container Dev
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(_blobServiceEndpoint), new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey));
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerNameHistorialDev);
            BlobClient blobClient = containerClient.GetBlobClient(blobPath);

            using FileStream uploadFileStream = File.OpenRead(filePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            response = true;

            return response;
        }
        #endregion

        #region [Functions]

        /// <summary>
        /// Funcion que permite retornar el nombre del archivo si tiene la 
        /// letra F o si no por defecto el nombre del archivo.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string FormatFileNameAzure(string fileName)
        {
            // Obtener la extensión del archivo
            string extension = Path.GetExtension(fileName);

            // Regex para extraer el InvoiceNumber si contiene la letra 'F'
            string patternWithF = @"(\d{11}-\d{2}-(F\d{3}-\d{5}))" + Regex.Escape(extension) + "$";
            // Regex para extraer el nombre completo si no contiene la letra 'F'
            string patternWithoutF = @"(\d{11}-\d{2}-\d{3}-\d{5})" + Regex.Escape(extension) + "$";

            Match matchWithF = Regex.Match(fileName, patternWithF);
            Match matchWithoutF = Regex.Match(fileName, patternWithoutF);

            if (matchWithF.Success)
            {
                string invoiceNumber = matchWithF.Groups[2].Value;

                // Devolver el nuevo nombre del archivo
                return $"{invoiceNumber}{extension}";
            }
            else if (matchWithoutF.Success)
            {
                // Devolver el nombre completo
                return fileName;
            }
            return fileName;
        }
        #endregion
    }
}
