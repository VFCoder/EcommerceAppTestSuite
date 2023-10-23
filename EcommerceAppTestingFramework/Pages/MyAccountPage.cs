using EcommerceAppTestingFramework.Drivers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Pages
{
    public class MyAccountPage
    {
        private readonly IDriverActions _driver;

        public MyAccountPage(IDriverActions driver)
        {
            _driver = driver;
        }
        private IWebElement ChangePasswordLink => _driver.FindElementWait(By.CssSelector("a[href='/customer/changepassword']"));
        private IWebElement OldPasswordInput => _driver.FindElementWait(By.Id("OldPassword"));
        private IWebElement NewPasswordInput => _driver.FindElementWait(By.Id("NewPassword"));
        private IWebElement ConfirmNewPasswordInput => _driver.FindElementWait(By.Id("ConfirmNewPassword"));
        private IWebElement ChangePasswordBtn => _driver.FindElementWait(By.CssSelector(".change-password-button"));
        private IWebElement PasswordChangeSuccessMessage => _driver.FindElementWait(By.CssSelector(".bar-notification.success"));
        private IWebElement ChangePasswordPageTitle => _driver.FindElementWait(By.CssSelector(".page-title h1"));

        public string pageTitle = "Account";

        public void EnterOldPassword(string oldPassword)
        {
            OldPasswordInput.SendKeys(oldPassword);
        }

        public void EnterNewPassword(string newPassword)
        {
            NewPasswordInput.SendKeys(newPassword);
        }

        public void EnterConfirmNewPassword(string confirmNewPassword)
        {
            ConfirmNewPasswordInput.SendKeys(confirmNewPassword);
        }

        public void ClickChangePasswordLink()
        {
            ChangePasswordLink.Click();
        }


        public void ClickChangePasswordBtn()
        {
            ChangePasswordBtn.Click();
        }

        public bool PasswordChangeSuccessMsgDisplayed()
        {
            return PasswordChangeSuccessMessage.Displayed;
        }
        
        public bool ChangePasswordPageTitleDisplayed()
        {
            return ChangePasswordPageTitle.Displayed;
        }
    }
}
