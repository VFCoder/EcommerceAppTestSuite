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
        protected DataGenerator _dataGenerator;
        protected BogusData _bogusData;
        protected BasePage _basePage;
        protected HomePage _homePage;
        protected LoginPage _loginPage;
        protected RegisterPage _registerPage;
        protected ProductPage _productPage;
        protected SearchPage _searchPage;
        protected UserDataAndOrderVerifier _verifier;
        protected CartPage _cartPage;
        protected CheckoutPage _checkoutPage;
        protected OrderDetailsPage _orderDetailsPage;

        [SetUp]
        public void Setup()
        {
            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig);
            _extentReporting = new ExtentReporting(_driver);
            _dataGenerator = new DataGenerator();
            _bogusData = _dataGenerator.GenerateData();
            _basePage = new BasePage(_driver);
            _homePage = new HomePage(_driver);
            _registerPage = new RegisterPage(_driver);
            _loginPage = new LoginPage(_driver);
            _productPage = new ProductPage(_driver);
            _searchPage = new SearchPage(_driver);
            _verifier = new UserDataAndOrderVerifier(_driver);
            _cartPage = new CartPage(_driver);
            _checkoutPage = new CheckoutPage(_driver);
            _orderDetailsPage = new OrderDetailsPage(_driver);

            _extentReporting.CreateTest(TestContext.CurrentContext.Test.MethodName);
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
