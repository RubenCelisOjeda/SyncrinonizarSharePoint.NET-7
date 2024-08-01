namespace ahpeSynPagPro.Application.Interfaces.PagoProveedores
{
    public interface IPagoProveedorApplication
    {
        public Task<bool> AddFileAzureBlobStorage(string filePath,string folderName, string fileName);
    }
}
