using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using NUnit.Framework;
using System;

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    [Category("UI_CrossBrowserTests")]
    [Parallelizable(ParallelScope.Children)]
    public class CrossBrowserTesting
    {
        private TestConfiguration _testConfig;
        private BrowserDriver _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            _testConfig = new TestConfiguration();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            {
                _driver.Dispose();
            }
        }

        [Test]
        [TestCaseSource(typeof(TestConfiguration), nameof(TestConfiguration.BrowserToRun))]
        public void TestBasicAppFunctionalitiesCrossBrowser(string browserName)
        {
            using var driver = new BrowserDriver(_testConfig, browserName);
            Console.WriteLine("Test browser: " + browserName);

            driver.Driver.Navigate().GoToUrl("https://www.google.com");
        }
        

    }
}
