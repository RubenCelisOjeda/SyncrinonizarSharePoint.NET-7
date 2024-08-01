namespace ahpeSynPagPro.Infraestructure.ExternalService.Azure.Interfaces.PagoProveedores
{
    public interface IPagoProveedorExternalService
    {
        public Task<bool> AddFileAzureBlobStorage(string filePath,string folderName, string fileName);
    }
}
