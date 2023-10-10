using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace EcommerceAppTestingFramework.Drivers
{
    public interface IDriverActions
    {
        IWebElement FindElement(By locator);
        IWebElement FindElementWait(By locator, int? timeoutSec = null);
        IWebElement WaitForElementToBeClickable(IWebElement element, int? timeoutInSeconds = null);
        ReadOnlyCollection<IWebElement> FindElements(By by);

        INavigation Navigate();
        void NavigateToBaseURL();
        void NavigateToAdminURL();
        void NavigateToApiURL(string endpoint = "");
        void Click(IWebElement element);
        void SendKeys(IWebElement element, string text);
        void SelectDropDownByText(IWebElement element, string text);
        void SelectDropDownByTextContains(IWebElement element, string partialText);
        void WaitForLoad(int? timeoutInSeconds = null);
        //void HoverSelectWait(IWebElement hoverElement, IWebElement clickElement, int? timeoutInSeconds = null);
        void Dispose();

        bool WaitForPageLoad(int? timeoutInSeconds = null);
        bool WaitForElementToBeVisible(IWebElement element, int? timeoutInSeconds = null);
        bool WaitForText(IWebElement element, string expectedText, int? timeoutInSeconds = null);
        bool WaitForTitle(string expectedText, int? timeoutInSeconds = null);

        Actions ActionWait(int? timeoutInSeconds = null);
    }
}
