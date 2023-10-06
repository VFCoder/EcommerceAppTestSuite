using EcommerceAppTestingFramework.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EcommerceAppTestingFramework.Pages
{
    public class BasePage
    {
        private readonly IDriverActions _driver;

        public BasePage(IDriverActions driver)
        {
            _driver = driver;
        }

        private IWebElement HeaderLinks => _driver.FindElementWait(By.CssSelector(".header-links"));
        private IWebElement HeaderMenu => _driver.FindElementWait(By.CssSelector(".header-menu"));
        private IWebElement LoginLink => _driver.FindElementWait(By.CssSelector(".ico-login"));
        private IWebElement LogoutLink => _driver.FindElementWait(By.CssSelector(".ico-logout"));
        private IWebElement RegisterLink => _driver.FindElementWait(By.CssSelector(".ico-register"));
        private IWebElement MyAccountLink => _driver.FindElementWait(By.CssSelector(".ico-account"));
        private IWebElement WishlistLink => _driver.FindElementWait(By.CssSelector(".ico-wishlist"));
        private IWebElement CartLink => _driver.FindElementWait(By.CssSelector(".ico-cart"));
        private IWebElement AdminLink => _driver.FindElementWait(By.CssSelector(".administration"));
        private IWebElement SearchBox => _driver.FindElementWait(By.Id("small-searchterms"));
        private IWebElement SearchBtn => _driver.FindElementWait(By.CssSelector(".search-box-button"));
        private IWebElement LogoImage => _driver.FindElementWait(By.CssSelector("img[alt='Ecommerce Testing App']"));
        private IWebElement ComputersHoverMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/computers']"));
        private IWebElement DesktopsSubMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/desktops']"));
        private IWebElement NotebooksSubMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/notebooks']"));
        private IWebElement SoftwareSubMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/software']"));
        private IWebElement ElectronicsHoverMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/electronics']"));
        private IWebElement ApparelHoverMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/apparel']"));
        private IWebElement DigitalDownloadsMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/digital-downloads']"));
        private IWebElement BooksMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/books']"));
        private IWebElement JewelryMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/jewelry']"));
        private IWebElement GiftCardsMenuLink => _driver.FindElementWait(By.CssSelector(".top-menu.notmobile a[href='/gift-cards']"));
        private IWebElement CategoryMenuLink(string category) => _driver.FindElementWait(By.XPath($"//a[@href='/{category}']"), 2);
        private IWebElement SubCategoryMenuLink(string category) => _driver.FindElementWait(By.XPath($"//a[@href='/{category}']/ancestor::li//a"), 2);
        private IWebElement PageTitle => _driver.FindElementWait(By.CssSelector(".page-title"));


        public bool PageLoaded(string pageTitle)
        {
            _driver.WaitForPageLoad();
            return _driver.WaitForTitle(pageTitle) &&
                   _driver.WaitForElementToBeVisible(HeaderLinks) &&
                   _driver.WaitForElementToBeVisible(HeaderMenu) &&
                   _driver.WaitForElementToBeVisible(LogoImage) &&
                   _driver.WaitForElementToBeVisible(SearchBox);
        }

        public bool AdminLoggedIn()
        {
            return _driver.WaitForElementToBeVisible(AdminLink);
        }

        public bool IsLogoutLinkDisplayed()
        {
            return _driver.WaitForElementToBeVisible(LogoutLink);
        }

        public bool IsLoginLinkDisplayed()
        {
            return _driver.WaitForElementToBeVisible(LoginLink);
        }

        public void Search(string searchText)
        {
            SearchBox.SendKeys(searchText);
            SearchBtn.Click();
        }

        public void ClickLoginLink()
        {
            _driver.Click(LoginLink);
        }

        public void ClickLogoutLink()
        {
            _driver.Click(LogoutLink);
        }

        public void ClickRegisterLink()
        {
            _driver.Click(RegisterLink);
        }

        public void ClickWishlistLink()
        {
            _driver.Click(WishlistLink);
        }

        public void ClickCartLink()
        {
            _driver.Click(CartLink);
        }

        public void ClickComputersCategory()
        {
            ComputersHoverMenuLink.Click();
        }

        public void ClickElectronicsCategory()
        {
            ElectronicsHoverMenuLink.Click();
        }

        public void ClickApparelCategory()
        {
            ApparelHoverMenuLink.Click();
        }

        public void ClickDigitalDownloadsCategory()
        {
            DigitalDownloadsMenuLink.Click();
        }

        public void ClickBooksCategory()
        {
            BooksMenuLink.Click();
        }

        public void ClickJewelryCategory()
        {
            JewelryMenuLink.Click();
        }

        public void ClickGiftCardsCategory()
        {
            GiftCardsMenuLink.Click();
        }

        public void HoverComputersAndClickDesktopsCategory()
        {
            _driver.ActionWait().MoveToElement(ComputersHoverMenuLink).Perform();
            DesktopsSubMenuLink.Click();
        }

        public void PageTitleLoaded()
        {
            _driver.WaitForElementToBeVisible(PageTitle);
        }

        public void SelectCategoryLink(string category)
        {
            try 
            { 
                CategoryMenuLink(category).Click();
            }
            catch (WebDriverTimeoutException)
            {
                _driver.ActionWait().MoveToElement(SubCategoryMenuLink(category)).Perform();
                CategoryMenuLink(category).Click();
            };
        }

        public class Category
        {
            public const string Computers = "computers";
            public const string Desktops = "desktops";
            public const string Notebooks = "notebooks";
            public const string Software = "software";
            public const string Electronics = "electronics";
            public const string CameraPhoto = "camera-photo";
            public const string CellPhones = "cell-phones";
            public const string Others = "others";
            public const string Apparel = "apparel";
            public const string Shoes = "shoes";
            public const string Clothing = "clothing";
            public const string Accessories = "accessories";
            public const string DigitalDownloads = "digital-downloads";
            public const string Books = "books";
            public const string Jewelry = "jewelry";
            public const string GiftCards = "gift-cards";
        }
    }

}

