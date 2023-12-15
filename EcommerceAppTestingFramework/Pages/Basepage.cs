using EcommerceAppTestingFramework.Drivers;
using EcommerceAppTestingFramework.Models.UiModels;
using NUnit.Framework;
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
        private IWebElement BaseLink(string page) => _driver.FindElementWait(By.CssSelector($".ico-{page}"));
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
        private IReadOnlyCollection<IWebElement> SearchBarDropdownItems => _driver.FindElements(By.CssSelector(".ui-menu-item"));
        private IWebElement SearchBarDropdown => _driver.FindElementWait(By.Id("ui-id-1"), 2);
        private IWebElement CloseNotificationBtn => _driver.FindElementWait(By.CssSelector(".close"));



        public bool PageLoaded(string pageTitle)
        {
            _driver.WaitForPageLoad();
            return _driver.WaitForTitle(pageTitle) &&
                   _driver.WaitForElementToBeVisible(HeaderLinks) &&
                   _driver.WaitForElementToBeVisible(HeaderMenu) &&
                   _driver.WaitForElementToBeVisible(LogoImage) &&
                   _driver.WaitForElementToBeVisible(SearchBox);
        }
        
        public bool PageLoadedPassing(string pageTitle)
        {
            try
            {
                _driver.WaitForPageLoad();
                _driver.WaitForTitle(pageTitle,3);
                _driver.WaitForElementToBeVisible(HeaderLinks);
                _driver.WaitForElementToBeVisible(HeaderMenu);
                _driver.WaitForElementToBeVisible(LogoImage);
                _driver.WaitForElementToBeVisible(SearchBox);

                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Page not loaded" + ex.Message);
                return false;
            }
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

        public void EnterSearchText(string searchText)
        {
            SearchBox.SendKeys(searchText);
        }

        public void ClickSearchButton()
        {
            SearchBtn.Click();
        }

        public void NavigateToPage(string page)
        {
            _driver.Click(BaseLink(page));
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

        public void ClickMyAccountLink()
        {
            _driver.Click(MyAccountLink);
        }

        public void ClickShoppingCartLink()
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

        public void CloseNotificationPopup()
        {
            CloseNotificationBtn.Click();
            _driver.WaitForLoad();
        }



        public List<Product>? GetAllSearchDropdownItems(string search)
        {
            List<Product> products = new List<Product>();

            try { _driver.WaitForElementToBeVisible(SearchBarDropdown); }
            catch { }

            if (SearchBarDropdownItems.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"Number of search dropdown items displayed for {search}: {SearchBarDropdownItems.Count}");
                Console.WriteLine();

                foreach (var searchItemContainer in SearchBarDropdownItems)
                {
                    IWebElement ProductDropdownItemName = searchItemContainer.FindElement(By.CssSelector("span"));

                    string productName = ProductDropdownItemName.Text;

                    Assert.That(ProductDropdownItemName.Displayed, Is.True, "Product dropdown item(s) not displayed");
                    Assert.That(productName.ToLower().Contains(search.ToLower()), Is.True, $"Search dropdown item {productName} do not match search text {search}");


                    Product product = new Product
                    {
                        Name = productName,
                    };

                    products.Add(product);

                    Console.WriteLine($"Product Name: {product.Name}");

                }

                return products;
            }
            else
            {
                Console.WriteLine($"No search dropdown items found for '{search}'.");
                return null;
            }
        }
    }
}

