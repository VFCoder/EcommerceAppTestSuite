using EcommerceAppTestingFramework.Configuration;
using EcommerceAppTestingFramework.Pages;
using EcommerceAppTestingFramework.Drivers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EcommerceAppTestingFramework.Pages.BasePage;
using static EcommerceAppTestingFramework.Pages.CartPage;
using static EcommerceAppTestingFramework.Pages.CheckoutPage;
using static EcommerceAppTestingFramework.Pages.RegisterPage;
using static EcommerceAppTestingFramework.Pages.UserDataAndOrderVerifier;
using EcommerceAppTestingFramework.TestData;

namespace EcommerceAppTests.UITests
{
    [TestFixture]
    [Category("UI_SmokeTests")]
    [Parallelizable]
    public class CustomerAppSmokeTest : TestBase
    {

        [Test]
        public void TestBasicAppFunctionalities()
        {
            _extentReporting.LogInfo("Starting smoke test - TestBasicAppFunctionalities");


            //Navigate to home page and confirm navigation links/menus are loaded:

            _driver.NavigateToBaseURL();
            Assert.That(_basePage.PageLoaded(_homePage.pageTitle), Is.True, "Home page did not load correctly.");
            _extentReporting.LogInfo("Navigated to base url");


            //Register new customer:

            _basePage.ClickRegisterLink();
            Assert.That(_basePage.PageLoaded(_registerPage.pageTitle), Is.True, "Register page did not load correctly.");
            _extentReporting.LogInfo("Navigated to register page");


            _registerPage.SelectGender(Gender.Male);
            _registerPage.EnterFirstName(_bogusData.FirstName);
            _registerPage.EnterLastName(_bogusData.LastName);
            _registerPage.SelectBirthDateDay(_bogusData.DOBDay);
            _registerPage.SelectBirthDateMonth(_bogusData.DOBMonth);
            _registerPage.SelectBirthDateYear(_bogusData.DOBYear);
            _registerPage.EnterEmail(_bogusData.Email);
            _registerPage.EnterCompany(_bogusData.Company);
            _registerPage.EnterPassword(ValidUserData.Password);
            _registerPage.EnterConfirmPassword(ValidUserData.Password);
            _registerPage.ClickRegisterButton();
            Assert.That(_registerPage.IsRegistrationCompleted(), Is.True, "Registration completed message was not displayed correctly.");
            _extentReporting.LogInfo($"Submitted registration form for customer {_bogusData.FullName}");

            //Log out customer:

            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Logout link not displayed.");
            _basePage.ClickLogoutLink();
            Assert.That(_basePage.IsLoginLinkDisplayed, Is.True, "Logout was not successful.");
            _extentReporting.LogInfo("Logged out");


            //Log in customer:
            _basePage.ClickLoginLink();
            Assert.That(_basePage.PageLoaded(_loginPage.pageTitle), Is.True, "Login page did not load correctly.");


            _loginPage.EnterLoginEmail(_bogusData.Email);
            _loginPage.EnterLoginPassword(ValidUserData.Password);
            _loginPage.ClickLoginBtn();
            Assert.That(_basePage.IsLogoutLinkDisplayed, Is.True, "Logout link not displayed.");
            _extentReporting.LogInfo("Logged back in");

            //Perform product search and confirm product is returned:

            string searchText = "camera";

            _basePage.EnterSearchText(searchText);
            _basePage.ClickSearchButton();
            Assert.That(_basePage.PageLoaded(_searchPage.pageTitle), Is.True, "Search page did not load correctly.");
            Assert.That(_productPage.GetProductCount(), Is.GreaterThan(0), "No products returned from search");
            _extentReporting.LogInfo($"Performed product search for '{searchText}'");


            //Confirm product listing page:

            _basePage.SelectCategoryLink(Category.CellPhones);
            Assert.That(_basePage.PageLoaded(ProductPageTitle.CellPhones), Is.True, "Desktops page did not load correctly");
            Assert.That(_productPage.GetProductCount(), Is.GreaterThan(0), "No products displayed on product page");
            _extentReporting.LogInfo($"Selected product category '{Category.CellPhones}' and navigated to product listing page");


            //Confirm product details of first product listed:

            string productName = _productPage.GetProductNameByListingOrder(1);

            _productPage.ClickProductDetailsByListingOrder(1);
            Assert.That(_basePage.PageLoaded(productName), Is.True, "Product details page did not load correctly.");
            _extentReporting.LogInfo($"Navigated to product details page for '{productName}'");


            //Add product to cart:

            _productPage.AddProductToCartFromDetailsPage();
            Assert.That(_productPage.NotificationSuccessDisplayed(), Is.True, "Product added notification did not display");
            _productPage.CloseNotificationBar();
            _extentReporting.LogInfo("Added product to cart");


            //Confirm product is in cart and total price:

            _basePage.ClickCartLink();
            Assert.That(_basePage.PageLoaded("Shopping Cart"), Is.True, "Cart did not load");

            int productQuantity = _verifier.GetProductCount();

            Assert.That(productQuantity, Is.EqualTo(1), "Quantity of products in cart does not match");
            Assert.That(_cartPage.GetProductNameByCartOrder(1), Is.EqualTo(productName), "Product name in cart does not match");

            string cartTotalPrice = _verifier.GetTotalPrice();

            _extentReporting.LogInfo($"Confirmed product {productName} is in cart with price '{cartTotalPrice}'");

            //Check out:

            _cartPage.ClickTermsOfService();
            _cartPage.ClickCheckoutButton();
            Assert.That(_basePage.PageLoaded(_checkoutPage.pageTitle), Is.True, "Checkout page did not load");
            _extentReporting.LogInfo($"Navigated to check out");


            //Complete address details:

            _checkoutPage.SelectBillingAddressCountryDropdown(ValidUserData.Country);
            _checkoutPage.SelectBillingAddressStateDropdown(_bogusData.State);
            _checkoutPage.EnterBillingAddressCity(_bogusData.City);
            _checkoutPage.EnterBillingAddressStreet(_bogusData.Street);
            _checkoutPage.EnterBillingAddressZip(_bogusData.Zip);
            _checkoutPage.EnterBillingAddressPhone(_bogusData.PhoneNumber);
            _checkoutPage.ClickContinueFromBillingAddress();
            _extentReporting.LogInfo("Completed address details");

            //Choose shipping method:

            _checkoutPage.SelectShippingMethod(ShippingMethod.NextDayAir);
            _checkoutPage.ClickContinueFromShippingMethod();
            _extentReporting.LogInfo($"Selected shipping method");


            //Choose payment method:

            _checkoutPage.SelectPaymentMethod(PaymentMethod.CreditCard);
            _checkoutPage.ClickContinueFromPaymentMethod();
            _extentReporting.LogInfo($"Selected payment method");


            //Enter credit card details:

            _checkoutPage.SelectCreditCartType(CreditCardType.MasterCard);
            _checkoutPage.InputCardHolderName(_bogusData.FullName);
            _checkoutPage.InputCardNumber(_bogusData.CardNumber);
            _checkoutPage.SelectExpireMonth(_bogusData.ExpMonth);
            _checkoutPage.SelectExpireYear(_bogusData.ExpYear);
            _checkoutPage.InputCardCode(_bogusData.CVC);
            _checkoutPage.ClickContinueFromPaymentInfo();
            _extentReporting.LogInfo($"Entered credit card details");


            //Verify correct product and total price:

            Assert.That(_verifier.GetProductCount(), Is.EqualTo(productQuantity), "Quantity of products in checkout does not match");
            Assert.That(_cartPage.GetProductNameByCartOrder(1), Is.EqualTo(productName), "Product name in cart does not match");

            string checkoutTotalPrice = _verifier.GetTotalPrice();

            Assert.That(cartTotalPrice, Is.EqualTo(checkoutTotalPrice), "Total price does not match");
            _extentReporting.LogInfo($"Confirmed product {productName} and price {cartTotalPrice}");


            //Place order:

            _checkoutPage.ClickConfirmOrderBtn();
            Assert.That(_checkoutPage.IsConfirmOrderMessageDisplayed(), Is.True, "Confirm order message not displayed");
            _extentReporting.LogInfo($"Placed order");

            //Confirm order number:

            string orderNumberCheckout = _checkoutPage.GetOrderNumber();

            _checkoutPage.ClickOrderDetailsLink();
            Assert.That(_basePage.PageLoaded(_orderDetailsPage.pageTitle), Is.True, "Order Details page did not load");

            string orderNumberOrderDetails = _orderDetailsPage.GetOrderNumber();

            Assert.That(orderNumberCheckout, Is.EqualTo(orderNumberOrderDetails), "Order numbers do not match");
            _extentReporting.LogInfo($"Confirmed order number {orderNumberCheckout}");

        }
    }
}
