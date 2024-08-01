using Microsoft.Extensions.Configuration;

namespace ahpeSynPagPro.Transversal.Common._01.Common._02.Settings._01.ConnectionStrings
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConnectionStringsSetting
    {
        #region [Properties]
        /// <summary>
        /// 
        /// </summary>
        private static IConfiguration? _configuration;

        /// <summary>
        /// 
        /// </summary>
        public static string AhpeReceptorBD;
        #endregion

        #region [Constructor]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public static void Initialize(IConfiguration? configuration)
        {

            _configuration = configuration;
            Load();
        }
        #endregion

        #region [Functions]
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static void Load()
        {
            AhpeReceptorBD = _configuration["DataBaseConnection:SQLServer:AhpeReceptorBD"];
        }
        #endregion
    }
}
