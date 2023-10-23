﻿using EcommerceAppTestingFramework.Drivers;
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
        private IWebElement ForgotPasswordLink => _driver.FindElementWait(By.CssSelector("a[href='/passwordrecovery']"));
        private IWebElement RecoverPasswordBtn => _driver.FindElementWait(By.CssSelector(".password-recovery-button"));
        private IWebElement LoginErrorMessage => _driver.FindElementWait(By.CssSelector(".message-error"));


        public string pageTitle = "Login";
        public string recoveryPageTitle = "Login";


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
        
        public void ClickPasswordRecoveryBtn()
        {
            RecoverPasswordBtn.Click();
        }

        public void ClickForgotPasswordLink()
        {
            ForgotPasswordLink.Click();
        }

        public bool IsLoginErrorMsgDisplayed()
        {
            return LoginErrorMessage.Displayed;
        }

        public string GetLoginErrorMsg()
        {
            return LoginErrorMessage.Text;
        }

    }
}
