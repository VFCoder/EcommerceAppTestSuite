using EcommerceAppTestingFramework.Drivers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Pages
{
    public class RegisterPage
    {
        private readonly IDriverActions _driver;

        public RegisterPage(IDriverActions driver)
        {
            _driver = driver;
        }

        private IWebElement GenderRadioBtn(string gender) => _driver.FindElementWait(By.Id($"gender-{gender}"));
        private IWebElement FirstNameInput => _driver.FindElementWait(By.Id("FirstName"));
        private IWebElement LastNameInput => _driver.FindElementWait(By.Id("LastName"));
        private IWebElement BirthDateDayDropdown => _driver.FindElementWait(By.CssSelector("[name='DateOfBirthDay']"));
        private IWebElement BirthDateMonthDropdown => _driver.FindElementWait(By.CssSelector("[name='DateOfBirthMonth']"));
        private IWebElement BirthDateYearDropdown => _driver.FindElementWait(By.CssSelector("[name='DateOfBirthYear']"));
        private IWebElement EmailInput => _driver.FindElementWait(By.Id("Email"));
        private IWebElement CompanyInput => _driver.FindElementWait(By.Id("Company"));
        private IWebElement NewsletterCheckbox => _driver.FindElementWait(By.Id("Newsletter"));
        private IWebElement PasswordInput => _driver.FindElementWait(By.Id("Password"));
        private IWebElement ConfirmPasswordInput => _driver.FindElementWait(By.Id("ConfirmPassword"));
        private IWebElement RegisterBtn => _driver.FindElementWait(By.Id("register-button"));
        private IWebElement RegistrationCompletedMessage => _driver.FindElementWait(By.CssSelector(".page-body .result"));
        private IWebElement ContinueBtn => _driver.FindElementWait(By.CssSelector(".register-continue-button"));
        private IWebElement PasswordMismatchError => _driver.FindElementWait(By.Id("ConfirmPassword-error"));
        private IWebElement InvalidEmailError => _driver.FindElementWait(By.Id("Email-error"));


        public string pageTitle = "Register";


        public void SelectGender(string gender)
        {
            GenderRadioBtn(gender).Click();
        }

        public void EnterFirstName(string firstName)
        {
            FirstNameInput.SendKeys(firstName);
        }

        public void EnterLastName(string lastName)
        {
           LastNameInput.SendKeys(lastName);
        }

        public void SelectBirthDateDay(string day)
        {
            _driver.SelectDropDownByText(BirthDateDayDropdown, day);
        }
        
        public void SelectBirthDateMonth(string month)
        {
            _driver.SelectDropDownByText(BirthDateMonthDropdown, month);
        }
        
        public void SelectBirthDateYear(string year)
        {
            _driver.SelectDropDownByText(BirthDateYearDropdown, year);
        }

        public void EnterEmail(string email)
        {
            EmailInput.SendKeys(email);
        }        

        public void EnterCompany(string company)
        {
            CompanyInput.SendKeys(company);
        }

        public void EnterPassword(string password)
        {
            PasswordInput.SendKeys(password);
        }

        public void EnterConfirmPassword(string confirmPassword)
        {
            ConfirmPasswordInput.SendKeys(confirmPassword);
        }

        public void ClickRegisterButton()
        {
            RegisterBtn.Click();
        }

        public bool IsRegistrationCompleted()
        {
            return _driver.WaitForText(RegistrationCompletedMessage, "Your registration completed");
        }

        public void ClickContinueButton()
        {
            ContinueBtn.Click();
        }

        public class Gender
        {
            public const string Male = "male";
            public const string Female = "female";
        }

        public bool IsPasswordMismatchErrorMsgDisplayed()
        {
            return PasswordMismatchError.Displayed;
        }

        public string GetPasswordMismatchErrorMsg() 
        {
            return PasswordMismatchError.Text;
        }

        public bool IsInvalidEmailErrorMsgDisplayed()
        {
            return InvalidEmailError.Displayed;
        }

        public string GetInvalidEmailErrorMsg() 
        {
            return InvalidEmailError.Text;
        }
    }
}
