using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Interactions;
using EcommerceAppTestingFramework.Configuration;

namespace EcommerceAppTestingFramework.Utils
{
    public class DriverFixture : IWebDriver, IDriverActions
    {
        private IWebDriver Driver { get; }
        public WebDriverWait DriverWait { get; }

        private TestConfiguration _testConfig;

        public DriverFixture(TestConfiguration testConfig)
        {
            _testConfig = testConfig;

            //set environment variable: console: setx ENVIRONMENT Local/Azure /M
            var testRunType = Enum.Parse<TestRunType>(_testConfig.GetSetting("TestRunType"), ignoreCase: true);
            var browserType = Enum.Parse<BrowserType>(_testConfig.GetSetting("BrowserType"), ignoreCase: true);

            if (testRunType == TestRunType.Local)
            {
                Driver = GetWebDriver(browserType);
            }
            else if (testRunType == TestRunType.Grid)
            {
                var gridUrl = testConfig.GetSetting("GridUrl");
                Driver = GetRemoteWebDriver(browserType, gridUrl);
            }
            else
            {
                throw new ArgumentException("Invalid TestRunType specified.");
            }

        }

        public string Url { get => Driver.Url; set => Driver.Url = value; }

        public string Title => Driver.Title;

        public string PageSource => Driver.PageSource;

        public string CurrentWindowHandle => Driver.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => Driver.WindowHandles;

        public void Close() => Driver.Close();

        public void Quit() => Driver.Quit();

        public IWebElement FindElement(By by) => Driver.FindElement(by);

        public ReadOnlyCollection<IWebElement> FindElements(By by) => Driver.FindElements(by);

        public IOptions Manage() => Driver.Manage();

        public INavigation Navigate() => Driver.Navigate();

        public ITargetLocator SwitchTo() => Driver.SwitchTo();

        private IWebDriver GetWebDriver(BrowserType browserType)
        {
            return browserType switch
            {
                BrowserType.Chrome => new ChromeDriver(),
                BrowserType.Firefox => new FirefoxDriver(),
                BrowserType.Safari => new SafariDriver(),
                BrowserType.Edge => new EdgeDriver(),
                _ => new ChromeDriver()
            };
        }

        private IWebDriver GetRemoteWebDriver(BrowserType browserType, string GridUrl)
        {
            return browserType switch
            {
                BrowserType.Chrome => new RemoteWebDriver(new Uri(GridUrl), new ChromeOptions()),
                BrowserType.Firefox => new RemoteWebDriver(new Uri(GridUrl), new FirefoxOptions()),
                BrowserType.Safari => new RemoteWebDriver(new Uri(GridUrl), new SafariOptions()),
                BrowserType.Edge => new RemoteWebDriver(new Uri(GridUrl), new EdgeOptions()),
                _ => new RemoteWebDriver(new Uri(GridUrl), new ChromeOptions())
            };
        }

        public void NavigateToBaseURL()
        {
            string baseUrl = _testConfig.GetBaseUrl();
            Driver.Navigate().GoToUrl(baseUrl);
        }

        public void NavigateToAdminURL()
        {
            string adminUrl = _testConfig.GetAdminUrl();
            Driver.Navigate().GoToUrl(adminUrl);
        }

        public void NavigateToApiURL(string endpoint = "")
        {
            string apiUrl = _testConfig.GetApiUrl() + endpoint;
            Driver.Navigate().GoToUrl(apiUrl);
        }

        public IWebElement FindElementWait(By locator, int? timeoutSec = null)
        {
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(0, 0, timeoutSec ?? 10));
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public IWebElement WaitForElementToBeClickable(IWebElement element, int? timeoutInSeconds = null)
        {
            WebDriverWait wait = new WebDriverWait(this, new TimeSpan(0, 0, timeoutInSeconds ?? 10));
            return wait.Until(ExpectedConditions.ElementToBeClickable(element));

        }

        public bool WaitForElementToBeVisible(IWebElement element, int? timeoutInSeconds = null)
        {
            var wait = new WebDriverWait(this, new TimeSpan(0, 0, timeoutInSeconds ?? 10));
            return wait.Until(Driver => element.Displayed);
        }

        public bool WaitForText(IWebElement element, string expectedText, int? timeoutInSeconds = null)
        {

            var wait = new WebDriverWait(this, new TimeSpan(0, 0, timeoutInSeconds ?? 10));
            return wait.Until(Driver => element.Text == expectedText);

        }

        public bool WaitForTitle(string expectedTitle, int? timeoutInSeconds = null)
        {
            var wait = new WebDriverWait(this, new TimeSpan(0, 0, timeoutInSeconds ?? 10));
            return wait.Until(driver => driver.Title.Contains(expectedTitle));

        }

        public bool WaitForPageLoad(int? timeoutInSeconds = null)
        {
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(0, 0, timeoutInSeconds ?? 10));
            wait.Until(wd => ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState").ToString() == "complete");
            return true;
        }

        public void WaitForLoad(int? timeoutInSeconds = null)
        {
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(0, 0, timeoutInSeconds ?? 10));
            wait.Until(d =>
            {
                bool ajaxComplete;
                bool jsReady;

                IJavaScriptExecutor js = (IJavaScriptExecutor)d;
                jsReady = (bool)js.ExecuteScript("return (document.readyState == \"complete\" || document.readyState == \"interactive\")"); ;

                try
                {
                    ajaxComplete = (bool)js.ExecuteScript("var result = true; try { result = (typeof jQuery != 'undefined') ? jQuery.active == 0 : true } catch (e) {}; return result;");
                }
                catch (Exception)
                {
                    ajaxComplete = true;
                }

                return ajaxComplete && jsReady;
            });
        }

        public Actions ActionWait(int? timeoutInSeconds = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds ?? 5));
            wait.Until(driver => true);
            var actions = new Actions(Driver);
            return actions;
        }

        public void Click(IWebElement element)
        {
            var elementToClick = WaitForElementToBeClickable(element);
            elementToClick.Click();
        }

        public void SendKeys(IWebElement element, string text)
        {
            var elementClickable = WaitForElementToBeClickable(element);
            elementClickable.SendKeys(text);
        }

        public void SelectDropDownByText(IWebElement element, string text)
        {
            var elementClickable = WaitForElementToBeClickable(element);
            var select = new SelectElement(elementClickable);
            select.SelectByText(text);
        }

        public void Dispose()
        {
            Quit();
        }


    }

    public enum BrowserType
    {
        Chrome,
        Firefox,
        Edge,
        Safari
    }

    public enum TestRunType
    {
        Local,
        Grid
    }
}
