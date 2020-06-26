using Kount.Ris;
using KountRisStandardTest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace KountRisStandardTest
{
    public class LoggingTests : TestBase
    {
        private string _sid;
        private string _orderNum;
        private Inquiry inquiry;

        public LoggingTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        public void TestLoggerFromConstructor()
        {
            var inquiry = TestHelper.DefaultInquiry(out _sid, out _orderNum, logger);
            inquiry.SetNoPayment();

            Response response = inquiry.GetResponse();
            Assert.True(logger.invocationCount > 0);
        }

        [Fact]
        public void TestLoggerFromProperty()
        {
            var inquiry = TestHelper.DefaultInquiry(out _sid, out _orderNum, null);
            inquiry.SetNoPayment();
            inquiry.logger = logger;

            Response response = inquiry.GetResponse();
            Assert.True(logger.invocationCount > 0);
        }

        [Fact]
        public void TestDefaultLogger()
        {
            LoggingComponent.defaultLogger = logger;
            var inquiry = TestHelper.DefaultInquiry(out _sid, out _orderNum, null);
            inquiry.SetNoPayment();

            Response response = inquiry.GetResponse();
            Assert.True(logger.invocationCount > 0);
        }
    }
}
