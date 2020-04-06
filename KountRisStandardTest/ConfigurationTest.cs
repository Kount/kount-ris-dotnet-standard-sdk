
namespace KountRisStandardTest
{
    using Kount.Ris;
    using Microsoft.Extensions.Configuration;
    using System.Configuration;
    using System.IO;
    using Xunit;



    public class ConfigurationTest
    {
        public Kount.Ris.Configuration SUT;

        
        public ConfigurationTest()
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
            ConfigurationManager.AppSettings["LOG.LOGGER"] = configuration.GetConnectionString("LOG.LOGGER");
            ConfigurationManager.AppSettings["LOG.SIMPLE.LEVEL"] = configuration.GetConnectionString("LOG.SIMPLE.LEVEL");
            ConfigurationManager.AppSettings["LOG.SIMPLE.ELAPSED"] = configuration.GetConnectionString("LOG.SIMPLE.ELAPSED");

            SUT = Kount.Ris.Configuration.FromAppSettings();
        }

         [Fact]
        public void FromAppSettings_assigns_Connect_Timeout()
        {
            Assert.Equal("10000", SUT.ConnectTimeout);
        }

         [Fact]
        public void FromAppSettings_assigns_MerchantId()
        {
            Assert.Equal("000000", SUT.MerchantId);
        }

         [Fact]
        public void FromAppSettings_assigns_API_Key()
        {
            Assert.Equal("", SUT.ApiKey);
        }

         [Fact]
        public void FromAppSettings_assigns_Version()
        {
            Assert.Equal("0700", SUT.Version);
        }

         [Fact]
        public void FromAppSettings_assigns_Url()
        {
            Assert.Equal("https://risk.beta.kount.net", SUT.URL);
        }

         [Fact]
        public void FromAppSettings_assigns_CertificateFile()
        {
            Assert.Equal("certificate.pfx", SUT.CertificateFile);
        }

         [Fact]
        public void FromAppSettings_assigns_PrivateKeyPassword()
        {
            Assert.Equal("11111111111111111", SUT.PrivateKeyPassword);
        }

         [Fact]
        public void FromAppSettings_assigns_ConfigKey()
        {
            Assert.NotNull(SUT.ConfigKey);
        }
    }
}