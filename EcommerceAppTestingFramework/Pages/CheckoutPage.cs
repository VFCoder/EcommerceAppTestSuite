using Bogus.DataSets;
using EcommerceAppTestingFramework.Drivers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EcommerceAppTestingFramework.Pages
{

    public class CheckoutPage
    {
        private readonly IDriverActions _driver;

        public CheckoutPage(IDriverActions driver)
        {
            _driver = driver;
        }

        private IWebElement ShipToSameAddressCheckbox => _driver.FindElementWait(By.Id("ShipToSameAddress"));
        private IWebElement BillingAddressDropdown => _driver.FindElementWait(By.Id("billing-address-select"));
        private IWebElement ContinueBtnBillingAddress => _driver.FindElementWait(By.CssSelector("#checkout-step-billing .new-address-next-step-button"));
        private IWebElement ShippingAddressDropdown => _driver.FindElementWait(By.Id("shipping-address-select"));
        private IWebElement ContinueBtnShippingAddress => _driver.FindElementWait(By.CssSelector("#checkout-step-shipping .new-address-next-step-button"));
        private IWebElement ContinueBtnShippingMethod => _driver.FindElementWait(By.CssSelector(".shipping-method-next-step-button"));
        private IWebElement ContinueBtnPaymentMethod => _driver.FindElementWait(By.CssSelector(".payment-method-next-step-button"));
        private IWebElement ContinueBtnPaymentInfo => _driver.FindElementWait(By.CssSelector(".payment-info-next-step-button"));
        private IWebElement ConfirmOrderBtn => _driver.FindElementWait(By.CssSelector(".confirm-order-next-step-button"));
        private IWebElement ShippingMethodRadioBtn(string value) => _driver.FindElementWait(By.Id($"shippingoption_{value}"));
        private IWebElement ShippingGroundRadio => _driver.FindElementWait(By.Id("shippingoption_0"));
        private IWebElement ShippingNextDayAirRadio => _driver.FindElementWait(By.Id("shippingoption_1"));
        private IWebElement Shipping2ndDayAirRadio => _driver.FindElementWait(By.Id("shippingoption_2"));
        private IWebElement PaymentMethodRadioBtn(string value) => _driver.FindElementWait(By.Id($"paymentmethod_{value}"));
        private IWebElement PaymentMethodCheckMoneyOrderRadio => _driver.FindElementWait(By.Id("paymentmethod_0"));
        private IWebElement PaymentMethodCreditCardRadio => _driver.FindElementWait(By.Id("paymentmethod_1"));
        private IWebElement PaymentMethodPaypalRadio => _driver.FindElementWait(By.Id("paymentmethod_2"));
        private IWebElement CreditCardTypeDropdown => _driver.FindElementWait(By.Id("CreditCardType"));
        private IWebElement CreditCardNameInput => _driver.FindElementWait(By.Id("CardholderName"));
        private IWebElement CreditCardNumberInput => _driver.FindElementWait(By.Id("CardNumber"));
        private IWebElement CreditCardExpireMonth => _driver.FindElementWait(By.Id("ExpireMonth"));
        private IWebElement CreditCardExpireYear => _driver.FindElementWait(By.Id("ExpireYear"));
        private IWebElement CreditCardCodeInput => _driver.FindElementWait(By.Id("CardCode"));
        private IWebElement OrderNumber => _driver.FindElementWait(By.CssSelector(".order-number strong"));
        private IWebElement ConfirmOrderMessage => _driver.FindElementWait(By.CssSelector(".order-completed .title strong"));
        private IWebElement OrderDetailsLink => _driver.FindElementWait(By.CssSelector(".details-link a"));
        private IWebElement BillingAddress => _driver.FindElementWait(By.CssSelector(".billing-info"));
        private IWebElement ShippingAddress => _driver.FindElementWait(By.CssSelector(".shipping-info"));
        private IWebElement AddressName(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .name"));
        private IWebElement AddressEmail(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .email"));
        private IWebElement AddressPhone(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .phone"));
        private IWebElement AddressFax(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .fax"));
        private IWebElement AddressCompany(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .company"));
        private IWebElement AddressStreet(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .address1"));
        private IWebElement AddressCityStateZip(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .city-state-zip"));
        private IWebElement AddressCountry(string addressType) => _driver.FindElementWait(By.CssSelector($".{addressType}-info .info-list .country"));
        private IWebElement AddressPaymentMethod => _driver.FindElementWait(By.CssSelector(".info-list .payment-method"));
        private IWebElement AddressShippingMethod => _driver.FindElementWait(By.CssSelector(".info-list .shipping-method"));
        private IWebElement BillingAddressCountryDropdown => _driver.FindElementWait(By.Id("BillingNewAddress_CountryId"));
        private IWebElement BillingAddressStateDropdown => _driver.FindElementWait(By.Id("BillingNewAddress_StateProvinceId"));
        private IWebElement BillingAddressCityInput => _driver.FindElementWait(By.Id("BillingNewAddress_City"));
        private IWebElement BillingAddressStreetInput => _driver.FindElementWait(By.Id("BillingNewAddress_Address1"));
        private IWebElement BillingAddressZipInput => _driver.FindElementWait(By.Id("BillingNewAddress_ZipPostalCode"));
        private IWebElement BillingAddressPhoneInput => _driver.FindElementWait(By.Id("BillingNewAddress_PhoneNumber"));

        public string pageTitle = "Checkout";

        public void SelectBillingAddressCountryDropdown(string country)
        {
            _driver.SelectDropDownByTextContains(BillingAddressCountryDropdown, country);
            _driver.WaitForLoad();

        }

        public void SelectBillingAddressStateDropdown(string state)
        {
            _driver.SelectDropDownByTextContains(BillingAddressStateDropdown, state);
        }

        public void EnterBillingAddressCity(string city)
        {
            BillingAddressCityInput.SendKeys(city);
        }

        public void EnterBillingAddressStreet(string street)
        {
            BillingAddressStreetInput.SendKeys(street);
        }
        
        public void EnterBillingAddressZip(string zip)
        {
            BillingAddressZipInput.SendKeys(zip);
        }
        
        public void EnterBillingAddressPhone(string phone)
        {
            BillingAddressPhoneInput.SendKeys(phone);
        }

        public void ClickContinueFromBillingAddress()
        {
            ContinueBtnBillingAddress.Click();
        }

        public void ClickContinueFromShippingAddress()
        {
            ContinueBtnShippingAddress.Click();
        }

        public void ClickNewBillingAddressDropdown()
        {
            _driver.SelectDropDownByText(BillingAddressDropdown, "New Address");            
        }

        public void ClickNewShippingAddressDropdown()
        {
            _driver.SelectDropDownByText(ShippingAddressDropdown, "New Address");            
        }

        public void SelectShippingtMethod(string shippingMethod)
        {
            ShippingNextDayAirRadio.Click();
        }

        public void SelectShippingMethod(string value)
        {
            ShippingMethodRadioBtn(value).Click();
        }

        public string GetSelectedShippingMethod(string value)
        {
            string selectedShippingMethod = _driver.FindElement(By.CssSelector($"label[for='shippingoption_{value}']")).Text[..^8];

            return selectedShippingMethod;
        }

        public void ClickContinueFromShippingMethod()
        {
            ContinueBtnShippingMethod.Click();
        }

        public void SelectPaymentMethod(string value)
        {
            PaymentMethodRadioBtn(value).Click();
        }

        public string GetSelectedPaymentMethod(string value)
        {
            string selectedPaymentMethod = _driver.FindElement(By.CssSelector($".payment-details label[for='paymentmethod_{value}']")).Text;

            return selectedPaymentMethod;
        }

        public void ClickContinueFromPaymentMethod()
        {
            ContinueBtnPaymentMethod.Click();
        }

        public void CreditCardFormHelper(string cardType, string name, string cardNumber, string month, string year, string cvcNumber)
        {
            _driver.SelectDropDownByText(CreditCardTypeDropdown, cardType);
            CreditCardNameInput.SendKeys(name);
            CreditCardNumberInput.SendKeys(cardNumber);
            _driver.SelectDropDownByText(CreditCardExpireMonth, month);
            _driver.SelectDropDownByText(CreditCardExpireYear, year);
            CreditCardCodeInput.SendKeys(cvcNumber);
            ContinueBtnPaymentInfo.Click();

        }

        public void SelectCreditCartType(string cardType)
        {
            _driver.SelectDropDownByText(CreditCardTypeDropdown, cardType);
        }

        public void InputCardHolderName(string name)
        {
            CreditCardNameInput.SendKeys(name);
        }

        public void InputCardNumber(string number)
        {
            CreditCardNumberInput.SendKeys(number);
        }

        public void SelectExpireMonth(string month)
        {
            _driver.SelectDropDownByText(CreditCardExpireMonth, month);
        }

        public void SelectExpireYear(string year)
        {
            _driver.SelectDropDownByText(CreditCardExpireYear, year);
        }

        public void InputCardCode(string number)
        {
            CreditCardCodeInput.SendKeys(number);
        }

        public void ClickContinueFromPaymentInfo()
        {
            ContinueBtnPaymentInfo.Click();
        }

        public void ClickConfirmOrderBtn()
        {
            ConfirmOrderBtn.Click();
        }

        public void ClickOrderDetailsLink()
        {
            OrderDetailsLink.Click();
        }

        public string GetAddressText(string addressType)
        {
            string AddressText = $"{AddressName(addressType).Text}\n{AddressEmail(addressType).Text}\n{AddressPhone(addressType).Text}\n{AddressFax(addressType).Text}\n{AddressCompany(addressType).Text}\n{AddressStreet(addressType).Text}\n{AddressCityStateZip(addressType).Text}\n{AddressCountry(addressType).Text}";

            return AddressText;
        }

/*        public string GetBillingAddressText()
        {
            string billingAddressText = $"{BillingAddressName.Text}\n{BillingAddressEmail.Text}\n{BillingAddressPhone.Text}\n{BillingAddressFax.Text}\n{BillingAddressCompany.Text}\n{BillingAddressAddress1.Text}\n{BillingAddressCityStateZip.Text}\n{BillingAddressCountry.Text}";

            return billingAddressText;
        }

        public string GetShippingAddressText()
        {

            string shippingAddressText = $"{ShippingAddressName.Text}\n{ShippingAddressEmail.Text}\n{ShippingAddressPhone.Text}\n{ShippingAddressFax.Text}\n{ShippingAddressCompany.Text}\n{ShippingAddressAddress1.Text}\n{ShippingAddressCityStateZip.Text}\n{ShippingAddressCountry.Text}";

            return shippingAddressText;
        }*/

        public string GetPaymentMethodText()
        {
            string paymentMethodText = AddressPaymentMethod.Text;
            return paymentMethodText;
        }

        public string GetShippingMethodText()
        {
            string shippingMethodText = AddressShippingMethod.Text;
            return shippingMethodText;
        }

        public bool IsConfirmOrderMessageDisplayed()
        {
            _driver.WaitForLoad();
            return _driver.WaitForText(ConfirmOrderMessage, "Your order has been successfully processed!");
        }

        public string GetOrderNumber()
        {
            string orderNumberText = OrderNumber.Text[14..];
            return orderNumberText;
        }
    }
}
