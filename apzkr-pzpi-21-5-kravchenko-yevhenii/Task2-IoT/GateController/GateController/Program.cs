using GateController.Forms;
using GateController.Infrastructure.Extensions;
using Infrastructure.DIContainer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GateController;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices()
            .ConfigureForms()
            .SetupConfiguration();

        DIContainer.Services = host
            .Build().Services;

        AddErrorHandlers();

        ApplicationConfiguration.Initialize();

        var loginForm = DIContainer.Services.GetRequiredService<Login>();

        Application.Run(loginForm);
    }

    static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
    {
        var exception = (Exception)args.ExceptionObject;
        MessageBox.Show(exception.Message, "", MessageBoxButtons.OK);
    }

    static void ThreadExeptionHandler(object sender, ThreadExceptionEventArgs args)
    {
        var exception = args.Exception;
        MessageBox.Show(exception.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    static void AddErrorHandlers()
    {
        Application.ThreadException
            += new ThreadExceptionEventHandler(ThreadExeptionHandler);

        AppDomain.CurrentDomain.UnhandledException
            += new UnhandledExceptionEventHandler(ExceptionHandler);
    }
}
