using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Models.UiModels;
using EcommerceAppTestingFramework.Pages;
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

        [SetUp]
        public void SetUp()
        {
            _homePage = new HomePage(_driver);
            _searchPage = new SearchPage(_driver);
            _productPage = new ProductPage(_driver);
        }

        [Test]
        [Category("Smoke_Test")]
        [Category("Positive_Test")]
        public void VerifyBasicSearchFunctionality()
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
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        public void VerifyAdvancedSearchFunctionality()
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
