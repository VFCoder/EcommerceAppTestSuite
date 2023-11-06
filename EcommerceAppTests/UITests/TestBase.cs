using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Drivers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.BasePage;
using static EcommerceAppTestingFramework.Pages.CartPage;
using static EcommerceAppTestingFramework.Pages.CheckoutPage;
using static EcommerceAppTestingFramework.Pages.RegisterPage;
using static EcommerceAppTestingFramework.Pages.UserDataAndOrderVerifier;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Reflection;
using EcommerceAppTestingFramework.Reports;
using EcommerceAppTestingFramework.TestData;

namespace EcommerceAppTests.UITests
{
    public class TestBase
    {
        protected TestConfiguration _testConfig;
        protected IDriverActions _driver;
        protected ExtentReporting _extentReporting;
        protected BasePage _basePage;
        private HomePage _homePage;

        [SetUp]
        public void Setup()
        {
            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig);
            _extentReporting = new ExtentReporting(_driver);
            _extentReporting.CreateTest(TestContext.CurrentContext.Test.MethodName);
            _basePage = new BasePage(_driver);
            _homePage = new HomePage(_driver);

            //navigate to base url
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");
            _extentReporting.LogInfo("Navigated to base url");

        }

        [TearDown]
        public void Teardown()
        {
            _extentReporting.EndTest();
            _extentReporting.EndReporting();
            _driver.Dispose();

        }
    }
}
