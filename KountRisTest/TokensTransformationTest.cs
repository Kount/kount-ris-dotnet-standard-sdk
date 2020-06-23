
namespace KountRisStandardTest
{
    using Xunit;
    using Kount.Ris;
    using Microsoft.Extensions.Configuration;
    using System.Configuration;
    using System.IO;

    public class TokensTransformationTest
    {
        /// <summary>
        /// Payment Token
        /// </summary>
        private const string PTOK = "0007380568572514";

        /// <summary>
        ///
        /// </summary>
        /// 

        public TokensTransformationTest()
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
        }

        [Fact]
        public void TestMaskingCorrectUsage()
        {
            Request request = new Inquiry(false);

            request.SetCardPaymentMasked(PTOK);

            Assert.True("000738XXXXXX2514".Equals(request.GetParam("PTOK")), "Test failed! Masked token is wrong.");
            Assert.True("MASK".Equals(request.GetParam("PENC")), "Test failed! PENC param is wrong.");
            Assert.True("2514".Equals(request.GetParam("LAST4")), "Test failed! LAST4 param is wrong.");
        }

        [Fact]
        public void TestIncorrectMasking()
        {
            Inquiry request = new Inquiry(false);

            request.SetPayment(Kount.Enums.PaymentTypes.Card, "000738XXXXXX2514");

            var ptok = request.GetParam("PTOK");
            Assert.False("000738XXXXXX2514".Equals(ptok), "Test failed! Masked token is wrong.");
            Assert.False("MASK".Equals(request.GetParam("PENC")), "Test failed! PENC param is wrong.");
            Assert.True("2514".Equals(request.GetParam("LAST4")), "Test failed! LAST4 param is wrong.");
        }
    }
}
