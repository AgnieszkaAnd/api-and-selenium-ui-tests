using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ReportingLibrary;
using SeleniumHelperLibrary.WebDriverHelpers;

namespace GoogleMap.UITests
{
    [TestFixture]
    public abstract class TestBase
    {
        private const string _browserType = "chrome";
        protected ExtentReportsHelper extent;
        protected IWebDriver _driver;

        [OneTimeSetUp]
        public void SetUpReporter()
        {
            extent = new ExtentReportsHelper();
        }

        [SetUp]
        public void Setup()
        {
            switch (_browserType)
            {
                case "chrome":
                    _driver = new ChromeDriver();
                    break;
                default:
                    _driver = new ChromeDriver();
                    break;
            }
        }

        [TearDown]
        public void AfterTest()
        {
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                var stacktrace = TestContext.CurrentContext.Result.StackTrace;
                var errorMessage = "<pre>" + TestContext.CurrentContext.Result.Message + "</pre>";
                switch (status)
                {
                    case TestStatus.Failed:
                        extent.SetTestStatusFail($"<br>{errorMessage}<br>Stack Trace: <br>{stacktrace}<br>");
                        extent.AddTestFailureScreenshot(_driver.ScreenCaptureAsBase64String());
                        break;
                    case TestStatus.Skipped:
                        extent.SetTestStatusSkipped();
                        break;
                    default:
                        extent.SetTestStatusPass();
                        break;
                }
            } catch (Exception e)
            {
                throw (e);
            } 
        }
        [OneTimeTearDown]
        public void CloseAll()
        {
            try
            {
                extent.Close();
            } catch (Exception e)
            {
                throw (e);
            }
        }
    }
}