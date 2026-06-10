using System;
using System.IO;
using System.Reflection;
using Serilog;

namespace JustMyPackages.Hosting
{

    public static class ApplicationSetup
    {
        /// <summary>
        /// Configure a global exception handler on the current <see cref="AppDomain"/>.
        /// </summary>
        public static void SetRootExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// Set the current working directory to the directory that contains the executing assembly.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the executing assembly location cannot be determined.</exception>
        public static void SetRootDirectory()
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                ?? throw new ArgumentNullException(nameof(rootPath));
            Directory.SetCurrentDirectory(rootPath);
        }

        /// <summary>
        /// Initialize Serilog bootstrap logger used for early application logging.
        /// </summary>
        public static void SetupBootstrapLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path: "Logs/bootstrap.log", shared: true)
                .CreateBootstrapLogger();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal((Exception)e.ExceptionObject, "Unhandled exception caught. Runtime is terminating : {IsTerminating}", e.IsTerminating);
            Log.CloseAndFlush();
        }
    }
}
