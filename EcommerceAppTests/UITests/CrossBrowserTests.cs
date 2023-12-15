using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using NUnit.Framework;
using System;

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    [Category("UI_CrossBrowserTests")]
    [Parallelizable(ParallelScope.All)]
    public class CrossBrowserTests
    {
        private TestConfiguration _testConfig;
        private string _homePageTitle = "Ecommerce Testing App";

        [SetUp]
        public void Setup()
        {
            _testConfig = new TestConfiguration();
        }

        //set Browser types to run in appsettings.json
        [Test]
        [TestCaseSource(typeof(TestConfiguration), nameof(TestConfiguration.BrowserToRun))]
        public void TestCrossBrowserFunctionality(string browserName)
        {
            using var driver = new BrowserDriver(_testConfig, browserName);
            Console.WriteLine("Test browser: " + browserName);

            driver.Driver.Navigate().GoToUrl("https://ecommercetestingapp.azurewebsites.net/");
            Assert.That(driver.Driver.Title.ToLower(), Does.Contain((_homePageTitle).ToLower()));
        }
        
        //Declare browsers individually
        [Test]
        [TestCase("Chrome")]
        [TestCase("Edge")]
        [TestCase("Firefox")]
        public void TestCrossBrowserFunctionalityV2(string browserName)
        {
            using var driver = new BrowserDriver(_testConfig, browserName);
            Console.WriteLine("Test browser: " + browserName);

            driver.Driver.Navigate().GoToUrl("https://ecommercetestingapp.azurewebsites.net/");
            Assert.That(driver.Driver.Title.ToLower(), Does.Contain((_homePageTitle).ToLower()));
        }

    }
}
