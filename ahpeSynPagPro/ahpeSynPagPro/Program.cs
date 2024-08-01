using ahpeSynPagPro;
using ahpeSynPagPro.Application.Interfaces.PagoProveedores;
using ahpeSynPagPro.Application.Main.PagoProveedores;
using ahpeSynPagPro.Infraestructure.ExternalService.Azure.Interfaces.PagoProveedores;
using ahpeSynPagPro.Infraestructure.ExternalService.Azure.Main.PagoProveedores;
using ahpeSynPagPro.Transversal.Common._01.Common._02.Settings._02.Jwt;
using NLog.Extensions.Logging;

IHost host = Host.CreateDefaultBuilder(args)
.ConfigureServices((context, services) =>
{
    AppSettings.Initialize(context.Configuration);

    #region [AddRegistration]
    services.AddTransient<IPagoProveedorApplication, PagoProveedorApplication>();
    services.AddTransient<IPagoProveedorExternalService, PagoProveedorExternalService>();
    #endregion

    #region [AddLog]
    services.AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.SetMinimumLevel(LogLevel.Information);
        builder.AddNLog("NLog.config");
    });
    #endregion

    services.AddHostedService<Worker>();
})
.UseWindowsService()
.Build();

host.Run();
