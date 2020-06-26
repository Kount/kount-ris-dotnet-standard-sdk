using Kount.Ris;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using Xunit.Abstractions;

namespace KountRisStandardConfigTest
{
    public class XUnitLogger : ILogger, IDisposable
    {
        protected ITestOutputHelper outputHelper { get; set; }

        protected string category { get; set; }

        protected int _invocationCount = 0;
        public int invocationCount {  get => _invocationCount; }

        public XUnitLogger(ITestOutputHelper outputHelper, string category = null)
        {
            this.outputHelper = outputHelper;
            this.category = category != null ? category : String.Empty;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Interlocked.Increment(ref _invocationCount);

            //XUnit defect TestOutputHelper sometimes gets called after test completes - so wrapping with try/catch
            try { outputHelper.WriteLine($"{logLevel}:{category} - " + formatter(state, exception)); }
            catch (Exception) { }
        }

        public bool IsEnabled(LogLevel logLevel) { return true; }

        public IDisposable BeginScope<TState>(TState state) { return this; }

        public void Dispose(){}
    }


    public class TestBase
    {
        private static Object _lock = new Object();
        private static bool configured = false;

        protected XUnitLogger logger;

        public TestBase(ITestOutputHelper outputHelper = null)
        {
            if (outputHelper!=null)
            {
                logger = new XUnitLogger(outputHelper);
            }

            //check if global configuration has been applied
            if (!configured)
            {
                lock (_lock)
                {
                    if (!configured)
                    {
                        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                        IConfigurationRoot configuration = builder.Build();

                        ConfigurationManager.AppSettings["Ris.MerchantId"] = configuration.GetConnectionString("Ris.MerchantId");
                        ConfigurationManager.AppSettings["Ris.API.Key"] = configuration.GetConnectionString("Ris.API.Key");
                        ConfigurationManager.AppSettings["Ris.Config.Key"] = configuration.GetConnectionString("Ris.Config.Key");
                        ConfigurationManager.AppSettings["Ris.Url"] = configuration.GetConnectionString("Ris.Url");
                        ConfigurationManager.AppSettings["Ris.Version"] = configuration.GetConnectionString("Ris.Version");
                        ConfigurationManager.AppSettings["Ris.CertificateFile"] = configuration.GetConnectionString("Ris.CertificateFile");
                        ConfigurationManager.AppSettings["Ris.PrivateKeyPassword"] = configuration.GetConnectionString("Ris.PrivateKeyPassword");
                        ConfigurationManager.AppSettings["Ris.Connect.Timeout"] = configuration.GetConnectionString("Ris.Connect.Timeout");
                        configured = true;
                    }
                }
            }
        }
    }
}
