﻿using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using NUnit.Framework;
using System;

namespace EcommerceAppTests.UITests.OldTests
{
    [TestFixture]
    [Category("UI_CrossBrowserTests")]
    [Parallelizable(ParallelScope.All)]
    public class CrossBrowserTests
    {
        private TestConfiguration _testConfig;

        [SetUp]
        public void Setup()
        {
            _testConfig = new TestConfiguration();
        }


        [Test]
        [TestCaseSource(typeof(TestConfiguration), nameof(TestConfiguration.BrowserToRun))]
        public void TestBasicAppFunctionalitiesCrossBrowser(string browserName)
        {
            using var driver = new BrowserDriver(_testConfig, browserName);
            Console.WriteLine("Test browser: " + browserName);

            driver.Driver.Navigate().GoToUrl("https://ecommercetestingapp.azurewebsites.net/");
        }

    }
}