using EcommerceAppTestingFramework.Drivers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EcommerceAppTestingFramework.Pages
{
    public class SearchPage : ProductPage
    {
        private readonly IDriverActions _driver;

        public SearchPage(IDriverActions driver) : base(driver)
        {
            _driver = driver;
        }
        private IWebElement SearchBoxAdvanced => _driver.FindElementWait(By.Id("q"));
        private IWebElement AdvancedSearchCheckbox => _driver.FindElementWait(By.Id("advs"));
        private IWebElement CategoryDropdownAdvanced => _driver.FindElementWait(By.Id("cid"));
        private IWebElement ManufacturerDropdownAdvanced => _driver.FindElementWait(By.Id("mid"));
        private IWebElement SearchSubcategoriesCheckbox => _driver.FindElementWait(By.Id("isc"));
        private IWebElement SearchProductDescriptionsCheckbox => _driver.FindElementWait(By.Id("sid"));
        private IWebElement SearchbuttonAdvanced => _driver.FindElementWait(By.CssSelector(".search-button"));

        public string pageTitle = "Search";

        public string GetSearchKeywordAdvancedText()
        {
            return SearchBoxAdvanced.GetAttribute("value").ToString();
        }

        public void ClearAdvancedSearchBox()
        {
            SearchBoxAdvanced.Clear();
        }

        public void EnterAdvancedSearchText(string searchText)
        {
            SearchBoxAdvanced.SendKeys(searchText);
        }

        public void ClickAdvancedSearchCheckbox()
        {
            AdvancedSearchCheckbox.Click();
        }

        public void ClickSearchProductDescriptionCheckbox()
        {
            SearchProductDescriptionsCheckbox.Click();
        }

        public void ClickSearchSubcategoriesCheckbox()
        {
            SearchSubcategoriesCheckbox.Click();
        }

        public void SelectCategoryAdvancedSearchDropdown(string category)
        {
            _driver.SelectDropDownByTextContains(CategoryDropdownAdvanced, category);
        }

        public void SelectManufacturerAdvancedSearchDropdown(string manufacturer)
        {
            _driver.SelectDropDownByTextContains(ManufacturerDropdownAdvanced, manufacturer);
        }

        public void ClickSearchButtonAdvanced()
        {
            SearchbuttonAdvanced.Click();
        }
    }

}

