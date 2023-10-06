using EcommerceAppTestingFramework.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Pages
{
    public class LoginPage
    {
        private readonly IDriverActions _driver;

        public LoginPage(IDriverActions driver)
        {
            _driver = driver;
        }
        private IWebElement EmailInput => _driver.FindElementWait(By.Id("Email"));
        private IWebElement PasswordInput => _driver.FindElementWait(By.Id("Password"));
        private IWebElement LoginBtn => _driver.FindElementWait(By.CssSelector(".login-button"));


        public string pageTitle = "Login";


        public void EnterLoginEmail(string email)
        {
            EmailInput.SendKeys(email);
        }
        
        public void EnterLoginPassword(string password)
        {
            PasswordInput.SendKeys(password);
        }

        public void ClickLoginBtn()
        {
            LoginBtn.Click();
        }

    }
}
