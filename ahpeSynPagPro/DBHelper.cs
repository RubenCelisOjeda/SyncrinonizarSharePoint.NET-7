using System;

public class DBHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public DbContextOptions<AhpeReceptorBDContext> GetConnectionAhpeAuthBD()
    {
        var optionBuilder = new DbContextOptionsBuilder<AhpeAuthBDContext>();
        optionBuilder.UseSqlServer(ConnectionStringsSetting.AhpeAuthBD);
        return optionBuilder.Options;
    }
}
