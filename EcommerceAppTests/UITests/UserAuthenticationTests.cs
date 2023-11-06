using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Drivers;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Reports;
using EcommerceAppTestingFramework.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.RegisterPage;

namespace EcommerceAppTests.UITests

{

    [TestFixture]
    [Category("UI_Tests")]
    [Parallelizable]

    public class UserAuthenticationTests : TestBase
    {
        protected DataGenerator _dataGenerator;
        protected BogusData _bogusData;
        protected HomePage _homePage;
        protected MyAccountPage _myAccountPage;
        protected LoginPage _loginPage;
        protected RegisterPage _registerPage;

        [SetUp]
        public void Setup()
        {
            _dataGenerator = new DataGenerator();
            _bogusData = _dataGenerator.GenerateData();
            _homePage = new HomePage(_driver);
            _myAccountPage = new MyAccountPage(_driver);
            _registerPage = new RegisterPage(_driver);
            _loginPage = new LoginPage(_driver);
        }

        private void UserRegistration_Base(string gender, string firstName, string lastName, string dobDay, string dobMonth, string dobYear, string email, string company, string password, string confirmPassword)
        {
            // Common test steps for user registration
            _extentReporting.LogInfo($"Starting test - Register user: {firstName} {lastName} {email}");

            //navigate to register page
            _basePage.ClickRegisterLink();
            Assert.That(_basePage.PageLoaded(_registerPage.pageTitle), Is.True, "Register page did not load correctly.");
            _extentReporting.LogInfo("Navigated to register page");

            //complete registration form
            _registerPage.SelectGender(gender);
            _registerPage.EnterFirstName(firstName);
            _registerPage.EnterLastName(lastName);
            _registerPage.SelectBirthDateDay(dobDay);
            _registerPage.SelectBirthDateMonth(dobMonth);
            _registerPage.SelectBirthDateYear(dobYear);
            _registerPage.EnterEmail(email);
            _registerPage.EnterCompany(company);
            _registerPage.EnterPassword(password);
            _registerPage.EnterConfirmPassword(confirmPassword);
            _registerPage.ClickRegisterButton();

        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        public void UserRegistration_ValidCredentials()
        {
            //complete registration form
            UserRegistration_Base(
                $"{Gender.Male}", 
                $"{_bogusData.FirstName}", 
                $"{_bogusData.LastName}",
                $"{_bogusData.DOBDay}",
                $"{_bogusData.DOBMonth}", 
                $"{_bogusData.DOBYear}", 
                $"{_bogusData.Email}", 
                $"{_bogusData.Company}", 
                ValidUserData.Password,
                ValidUserData.Password
                );

            //confirm registration is successful
            Assert.That(_registerPage.IsRegistrationCompleted(), Is.True, "Registration completed message was not displayed correctly.");
            _extentReporting.LogInfo($"Register form successfully filled and submitted for user {_bogusData.FirstName} {_bogusData.LastName} {_bogusData.Email}");
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Negative_Test")]
        public void UserRegistration_PasswordConfirmationMismatch()
        {
            //complete register form with mismatched password confirmation
            UserRegistration_Base(
                $"{Gender.Male}",
                $"{_bogusData.FirstName}",
                $"{_bogusData.LastName}",
                $"{_bogusData.DOBDay}",
                $"{_bogusData.DOBMonth}",
                $"{_bogusData.DOBYear}",
                $"{_bogusData.Email}",
                $"{_bogusData.Company}",
                ValidUserData.Password,
                "PasswordMismatch"
                );

            //confirm error message
            bool isErrorMessageVisible = _registerPage.IsPasswordMismatchErrorMsgDisplayed();
            string errorMessage = _registerPage.GetPasswordMismatchErrorMsg();

            Assert.Multiple(() =>
            {
                Assert.That(isErrorMessageVisible, Is.True, "Password mismatch error message not displayed.");
                Assert.That(errorMessage, Is.EqualTo("The password and confirmation password do not match."), "Incorrect error message text.");
            });
            _extentReporting.LogInfo("Password confirmation mismatch error message displayed successfully");
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Negative_Test")]
        public void UserRegistration_InvalidEmail()
        {
            //complete registration form with invalid email
            UserRegistration_Base(
                $"{Gender.Male}",
                $"{_bogusData.FirstName}",
                $"{_bogusData.LastName}",
                $"{_bogusData.DOBDay}",
                $"{_bogusData.DOBMonth}",
                $"{_bogusData.DOBYear}",
                "InvalidEmail",
                $"{_bogusData.Company}",
                ValidUserData.Password,
                ValidUserData.Password
                );

            //confirm error message
            bool isErrorMessageVisible = _registerPage.IsInvalidEmailErrorMsgDisplayed();
            string errorMessage = _registerPage.GetInvalidEmailErrorMsg();

            Assert.Multiple(() =>
            {
                Assert.That(isErrorMessageVisible, Is.True, "Invalid email error message not displayed.");
                Assert.That(errorMessage, Is.EqualTo("Wrong email"), "Incorrect error message text.");
            });
            _extentReporting.LogInfo("Invalid email error message displayed successfully");
        }


        protected void UserLogin_Base(string email, string password)
        {
            _extentReporting.LogInfo($"Starting test - Login User: {email}");

            //navigate to login page
            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");
            _extentReporting.LogInfo("Navigated to login page");

            //submit login form
            _loginPage.LoginHelper(email,password);

        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        public void UserLogin_ValidCredentials()
        {
            //login as valid user
            UserLogin_Base(ValidUserData.Email, ValidUserData.Password);

            //confirm login is successful
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Logout link is not displayed.");
            _extentReporting.LogInfo("Submitted login credentials and logged in successfully.");
        }
        
        [Test]
        [Category("Functional_Test")]
        [Category("Negative_Test")]
        public void UserLogin_InvalidEmail()
        {
            //attempt login with invalid email
            UserLogin_Base("InvalidEmail", ValidUserData.Password);

            //confirm error message is displayed
            bool isErrorMessageVisible = _registerPage.IsInvalidEmailErrorMsgDisplayed();
            string errorMessage = _registerPage.GetInvalidEmailErrorMsg();

            Assert.Multiple(() =>
            {
                Assert.That(isErrorMessageVisible, Is.True, "Invalid email error message not displayed.");
                Assert.That(errorMessage, Is.EqualTo("Wrong email"), "Incorrect error message text.");
            });
            _extentReporting.LogInfo("Invalid email error message displayed successfully");

        }
        
        [Test]
        [Category("Functional_Test")]
        [Category("Negative_Test")]
        public void UserLogin_WrongPassword()
        {
            //attempt login with wrong password:
            UserLogin_Base(ValidUserData.Email, "WrongPassword");

            //confirm error message is displayed
            bool isErrorMessageVisible = _loginPage.IsLoginErrorMsgDisplayed();
            string errorMessage = _loginPage.GetLoginErrorMsg();

            Assert.Multiple(() =>
            {
                Assert.That(isErrorMessageVisible, Is.True, "Invalid email error message not displayed.");
                Assert.That(errorMessage, Is.EqualTo("Login was unsuccessful. Please correct the errors and try again."), "Incorrect error message text.");
            });
            _extentReporting.LogInfo("Login error message displayed successfully");

        }


        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        public void UserLogout()
        {
            //login:
            UserLogin_ValidCredentials();

            //logout:
            _basePage.ClickLogoutLink();
            Assert.That(_basePage.IsLoginLinkDisplayed(), Is.True, "Login link is not displayed");
            _extentReporting.LogInfo("Logged out successfully");
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        public void ChangePassword_ValidCredentials()
        {
            //login user:
            UserLogin_ValidCredentials();

            //change password:
            ChangePassword_Base(ValidUserData.Password, "ChangedPassword8", "ChangedPassword8");

            //confirm "password successfully changed" message is displayed
            Assert.That(_myAccountPage.PasswordChangeSuccessMsgDisplayed(), Is.True, "Password change success message was not displayed.");
            _extentReporting.LogInfo("Changed password successfully");
            _basePage.CloseNotificationPopup();

            //logout:
            _basePage.ClickLogoutLink();
            Assert.That(_basePage.IsLoginLinkDisplayed(), Is.True, "Login link is not displayed");
            _extentReporting.LogInfo("Logged out successfully");

            //login with new password:
            UserLogin_Base(ValidUserData.Email, "ChangedPassword8");
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Login was not successful.");
            _extentReporting.LogInfo("Submitted login credentials with new password and logged in.");
        }

        private void ChangePassword_Base(string oldPassword, string newPassword, string confirmNewPassword)
        {
            _extentReporting.LogInfo("Starting test - Change password");

            //navigate to my account page
            _basePage.ClickMyAccountLink();
            Assert.That(_basePage.PageLoaded(_myAccountPage.pageTitle), Is.True, "My account page did not load correctly.");
            _extentReporting.LogInfo("Navigated to my account page");

            //go to change password area
            _myAccountPage.ClickChangePasswordLink();
            Assert.That(_myAccountPage.ChangePasswordPageTitleDisplayed, Is.True, "Change password page did not load correctly.");
            _extentReporting.LogInfo("Navigated to change password page");

            //change password
            _myAccountPage.EnterOldPassword(oldPassword);
            _myAccountPage.EnterNewPassword(newPassword);
            _myAccountPage.EnterConfirmNewPassword(confirmNewPassword);
            _myAccountPage.ClickChangePasswordBtn();

        }
    }
}
