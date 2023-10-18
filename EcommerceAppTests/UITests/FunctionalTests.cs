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

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    [Category("UI_FunctionalTests")]
    [Parallelizable]
    public class CustomerAppFunctionalTest
    {
        private TestConfiguration _testConfig;
        private IDriverActions _driver;
        private BasePage _basePage;
        private HomePage _homePage;
        private LoginPage _loginPage;
        private RegisterPage _registerPage;
        private ProductPage _productPage;
        private SearchPage _searchPage;
        private UserDataAndOrderVerifier _verifier;
        private CartPage _cartPage;
        private CheckoutPage _checkoutPage;
        private OrderDetailsPage _orderDetailsPage;


        [SetUp]
        public void Setup()
        {

            _testConfig = new TestConfiguration();
            _driver = new DriverFixture(_testConfig);
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


        }

        [TearDown]
        public void Teardown()
        {
            _driver.Dispose();
        }

        [Test]
        public void RegisterNewCustomerTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.ClickRegisterLink();
            Assert.That(_basePage.PageLoaded(_registerPage.pageTitle), Is.True, "Register page did not load correctly.");

            _registerPage.SelectGender(Gender.Female);
            _registerPage.EnterFirstName("Mary");
            _registerPage.EnterLastName("Hicks");
            _registerPage.SelectBirthDateDay("13");
            _registerPage.SelectBirthDateMonth("June");
            _registerPage.SelectBirthDateYear("1995");
            _registerPage.EnterEmail("MaryHicks123@email.com");
            _registerPage.EnterCompany("");
            _registerPage.EnterPassword("UserPassword123");
            _registerPage.EnterConfirmPassword("UserPassword123");
            _registerPage.ClickRegisterButton();
            Assert.That(_registerPage.IsRegistrationCompleted(), Is.True, "Registration completed message was not displayed correctly.");

        }

        [Test]
        public void LoginCustomerTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");

            _loginPage.EnterLoginEmail("MaryHicks123@email.com");
            _loginPage.EnterLoginPassword("UserPassword123");
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Login was not successful.");

        }

        [Test]
        public void LogoutCustomerTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");

            _loginPage.EnterLoginEmail("MaryHicks123@email.com");
            _loginPage.EnterLoginPassword("UserPassword123");
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Login was not successful.");

            _basePage.ClickLogoutLink();
            Assert.That(_basePage.IsLoginLinkDisplayed, Is.True, "Logout was not successful.");

        }

        [Test]
        public void VerifyProductListingsTest()
        {
            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            VerifyListing(Category.Computers, ProductPageTitle.Computers, true);
            VerifyListing(Category.Electronics, ProductPageTitle.Electronics, true);
            VerifyListing(Category.Apparel, ProductPageTitle.Apparel, true);
            VerifyListing(Category.Desktops, ProductPageTitle.Desktops);
            VerifyListing(Category.Notebooks, ProductPageTitle.Notebooks);
            VerifyListing(Category.Software, ProductPageTitle.Software);
            VerifyListing(Category.CameraPhoto, ProductPageTitle.CameraPhoto);
            VerifyListing(Category.CellPhones, ProductPageTitle.CellPhones);
            VerifyListing(Category.Others, ProductPageTitle.Others);
            VerifyListing(Category.Shoes, ProductPageTitle.Shoes);
            VerifyListing(Category.Clothing, ProductPageTitle.Clothing);
            VerifyListing(Category.Accessories, ProductPageTitle.Accessories);
            VerifyListing(Category.DigitalDownloads, ProductPageTitle.DigitalDownloads);
            VerifyListing(Category.Books, ProductPageTitle.Books);
            VerifyListing(Category.Jewelry, ProductPageTitle.Jewelry);
            VerifyListing(Category.GiftCards, ProductPageTitle.GiftCards);
        }

        private void VerifyListing(string category, string expectedPageTitle, bool isSubcategory = false)
        {
            _basePage.SelectCategoryLink(category);
            Assert.That(_basePage.PageLoaded(expectedPageTitle), Is.True, $"{category} page did not load correctly.");

            if (isSubcategory)
            {
                _productPage.GetAllSubcategories();
            }
            else
            {
                _productPage.GetAllProductsList();
            }
        }

        [Test]
        public void VerifyBasicSearchFunctionalityTest()
        {
            string searchText = "apple";

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.EnterSearchText(searchText);
            _basePage.ClickSearchButton();
            Assert.That(_basePage.PageLoaded(_searchPage.pageTitle), Is.True, "Search page did not load correctly.");

            _productPage.VerifySearchResults(searchText);

        }
        
        [Test]
        public void VerifyAdvancedSearchFunctionalityTest()
        {
            string searchText = "cam";

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");

            _basePage.EnterSearchText(searchText);
            _basePage.GetAllSearchDropdownItems(searchText);
            _basePage.ClickSearchButton();
            Assert.That(_basePage.PageLoaded(_searchPage.pageTitle), Is.True, "Search page did not load correctly.");

            _productPage.VerifySearchResults(searchText);
            Assert.That(_searchPage.GetSearchKeywordAdvancedText(), Is.EqualTo(searchText), "Search text is not correct.");

            _searchPage.ClickAdvancedSearchCheckbox();
            _searchPage.SelectCategoryAdvancedSearchDropdown(ProductPageTitle.Electronics);
            _searchPage.ClickSearchSubcategoriesCheckbox();
            //_searchPage.SelectManufacturerAdvancedSearchDropdown(ManufacturerList.Apple);
            _searchPage.ClickSearchButtonAdvanced();

            _productPage.GetAllProductsList();
            _productPage.VerifyAdvancedSearchResults(searchText, ProductPageTitle.Electronics);

        }

    }
}
