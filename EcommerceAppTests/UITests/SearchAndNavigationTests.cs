using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Models.UiModels;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.TestData;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTests.UITests
{
    public class SearchAndNavigationTests : TestBase
    {
        private HomePage _homePage;
        private SearchPage _searchPage;
        private ProductPage _productPage;
        private LoginPage _loginPage;

        [SetUp]
        public void SetUp()
        {
            _homePage = new HomePage(_driver);
            _searchPage = new SearchPage(_driver);
            _productPage = new ProductPage(_driver);
            _loginPage = new LoginPage(_driver);
        }


        [Test]
        [Category("Smoke_Test")]
        [Category("Positive_Test")]
        [TestCase(PageNavigation.LoginPage)]
        [TestCase(PageNavigation.RegisterPage)]
        [TestCase(PageNavigation.WishlistPage)]
        [TestCase(PageNavigation.ShoppingCartPage)]
        public void NavigateToPageWhileLoggedOut(string page)
        {
            _basePage.NavigateToPage(page);
            Assert.That(_basePage.PageLoaded(page), Is.True, $"{page} page did not load correctly.");
        }

        [Test]
        [Category("Smoke_Test")]
        [Category("Positive_Test")]
        [TestCase(PageNavigation.WishlistPage)]
        [TestCase(PageNavigation.ShoppingCartPage)]
        [TestCase(PageNavigation.MyAccountPage)]
        public void NavigateToPageWhileLoggedIn(string page)
        {
            _loginPage.LoginHelper(ValidUserData.Email, ValidUserData.Password);
            _basePage.NavigateToPage(page);
            Assert.That(_basePage.PageLoaded(page), Is.True, $"{page} page did not load correctly.");
        }

        [Test]
        [Category("Smoke_Test")]
        [Category("Positive_Test")]
        [TestCase("apple")]
        [TestCase("camera")]
        public void VerifyBasicSearchFunctionality(string searchText)
        {
            _basePage.EnterSearchText(searchText);
            _basePage.ClickSearchButton();
            Assert.That(_basePage.PageLoaded(_searchPage.pageTitle), Is.True, "Search page did not load correctly.");
            _productPage.VerifySearchResults(searchText);
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        [TestCase("cam", ProductPageTitle.Electronics)]
        [TestCase("phone", ProductPageTitle.Electronics)]
        public void VerifyAdvancedSearchFunctionality(string searchText, string productCategory)
        {
            _basePage.EnterSearchText(searchText);
            _basePage.GetAllSearchDropdownItems(searchText);
            _basePage.ClickSearchButton();
            Assert.That(_basePage.PageLoaded(_searchPage.pageTitle), Is.True, "Search page did not load correctly.");

            _productPage.VerifySearchResults(searchText);
            Assert.That(_searchPage.GetSearchKeywordAdvancedText(), Is.EqualTo(searchText), "Search text is not correct.");

            _searchPage.ClickAdvancedSearchCheckbox();
            _searchPage.SelectCategoryAdvancedSearchDropdown(productCategory);
            _searchPage.ClickSearchSubcategoriesCheckbox();
            //_searchPage.SelectManufacturerAdvancedSearchDropdown(ManufacturerList.Apple);
            _searchPage.ClickSearchButtonAdvanced();

            _productPage.GetAllProductsList();
            _productPage.VerifyAdvancedSearchResults(searchText, productCategory);

        }
    }
}
