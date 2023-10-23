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

        private void UserRegistration_Base(string gender, string firstName, string lastName, string dobDay, string dobMonth, string dobYear, string email, string company, string password, string confirmPassword)
        {
            // Common test steps for user registration
            _extentReporting.LogInfo($"Starting test - Register user: {firstName} {lastName} {email}");

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");
            _extentReporting.LogInfo("Navigated to base url");

            _basePage.ClickRegisterLink();
            Assert.That(_basePage.PageLoaded(_registerPage.pageTitle), Is.True, "Register page did not load correctly.");
            _extentReporting.LogInfo("Navigated to register page");

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

            Assert.That(_registerPage.IsRegistrationCompleted(), Is.True, "Registration completed message was not displayed correctly.");
            _extentReporting.LogInfo($"Register form successfully filled and submitted for user {_bogusData.FirstName} {_bogusData.LastName} {_bogusData.Email}");
        }

        [Test]
        [Category("Functional_Test")]
        [Category("Negative_Test")]
        public void UserRegistration_PasswordConfirmationMismatch()
        {
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

            bool isErrorMessageVisible = _registerPage.IsInvalidEmailErrorMsgDisplayed();
            string errorMessage = _registerPage.GetInvalidEmailErrorMsg();

            Assert.Multiple(() =>
            {
                Assert.That(isErrorMessageVisible, Is.True, "Invalid email error message not displayed.");
                Assert.That(errorMessage, Is.EqualTo("Wrong email"), "Incorrect error message text.");
            });
            _extentReporting.LogInfo("Invalid email error message displayed successfully");
        }


        private void UserLogin_Base(string email, string password)
        {
            _extentReporting.LogInfo($"Starting test - Login User: {email}");

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");
            _extentReporting.LogInfo("Navigated to base url");

            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");
            _extentReporting.LogInfo("Navigated to login page");

            _loginPage.EnterLoginEmail(email);
            _loginPage.EnterLoginPassword(password);
            _loginPage.ClickLoginBtn();

        }

        [Test]
        [Category("Functional_Test")]
        [Category("Positive_Test")]
        public void UserLogin_ValidCredentials()
        {
            UserLogin_Base(ValidUserData.Email, ValidUserData.Password);

            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Logout link is not displayed.");
            _extentReporting.LogInfo("Submitted login credentials and logged in successfully.");
        }
        
        [Test]
        [Category("Functional_Test")]
        [Category("Negative_Test")]
        public void UserLogin_InvalidEmail()
        {
            UserLogin_Base("InvalidEmail", ValidUserData.Password);

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

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");
            _extentReporting.LogInfo("Navigated to base url");

            _basePage.ClickMyAccountLink();
            Assert.That(_basePage.PageLoaded(_myAccountPage.pageTitle), Is.True, "My account page did not load correctly.");
            _extentReporting.LogInfo("Navigated to my account page");

            _myAccountPage.ClickChangePasswordLink();
            Assert.That(_myAccountPage.ChangePasswordPageTitleDisplayed, Is.True, "Change password page did not load correctly.");
            _extentReporting.LogInfo("Navigated to change password page");

            _myAccountPage.EnterOldPassword(oldPassword);
            _myAccountPage.EnterNewPassword(newPassword);
            _myAccountPage.EnterConfirmNewPassword(confirmNewPassword);
            _myAccountPage.ClickChangePasswordBtn();

        }
    }
}
