using ahpeSynPagPro.Application.Interfaces.PagoProveedores;
using ahpeSynPagPro.Infraestructure.ExternalService.Azure.Interfaces.PagoProveedores;
using Microsoft.Extensions.Logging;

namespace ahpeSynPagPro.Application.Main.PagoProveedores
{
    public class PagoProveedorApplication : IPagoProveedorApplication
    {
        #region [Properties]
        private readonly ILogger<PagoProveedorApplication> _logger;
        private readonly IPagoProveedorExternalService _pagoProveedorExternalService;
        #endregion

        #region [Constructor]
        public PagoProveedorApplication(ILogger<PagoProveedorApplication> logger, IPagoProveedorExternalService pagoProveedorExternalService)
        {
            _logger = logger;
            _pagoProveedorExternalService = pagoProveedorExternalService;
        }
        #endregion

        #region [Methods]
        public async Task<bool> AddFileAzureBlobStorage(string filePath,string folderName, string fileName)
        {
            bool response = false;

            try
            {
                _logger.LogInformation("-------------AddFileAzureBlobStorage Started------------");

                response = await _pagoProveedorExternalService.AddFileAzureBlobStorage(filePath,folderName, fileName);

                _logger.LogInformation("-------------AddFileAzureBlobStorage Finished------------");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion
    }
}
