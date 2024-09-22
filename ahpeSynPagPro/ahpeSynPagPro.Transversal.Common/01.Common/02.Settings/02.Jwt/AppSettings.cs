using Microsoft.Extensions.Configuration;

namespace ahpeSynPagPro.Transversal.Common._01.Common._02.Settings._02.Jwt
{
    public static class AppSettings
    {
        #region [Properties]

        public static IConfiguration? _configuration;
        public static string? StorageAccountName { get; set; }
        public static string? StorageAccountKey { get; set; }
        public static string? ContainerNameDev { get; set; }
        public static string? ContainerNameHistorialDev { get; set; }
        #endregion

        #region [Constructor]
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
            Load();
        }
        #endregion

        #region [Functions]
        private static void Load()
        {
            StorageAccountName = _configuration?["AzureBlobStorage:StorageAccountName"];
            StorageAccountKey = _configuration?["AzureBlobStorage:StorageAccountKey"];
            ContainerNameDev = _configuration?["AzureBlobStorage:ContainerNameDev"];
            ContainerNameHistorialDev = _configuration?["AzureBlobStorage:ContainerNameHistorialDev"];
        }
        #endregion
    }
}
