using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using System.Threading;

namespace TestProject1
{
    public class PurchaseTests
    {
        private static IWebDriver _driver;
        private static string _accountEmail= "alex@alexsvstrom.com";
        private static string _accountPassword = "Test_123456789!";
        private static string _lastOrderNumber;

        [SetUp]
        public static void Initialize()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://demos.bellatrix.solutions/");
            _driver.Manage().Window.Maximize();
        }

        [TearDown]
        public static void TearDown()
        {
            _driver.Quit();
        }

        [Test, Order(1)]
        public static void SuccessfullyOrdered_When_NewCustomer()
        {
            var Falcon9AddToCartButton = _driver.FindElement(By.XPath("//a[@href='?add-to-cart=28']"));
            Falcon9AddToCartButton.Click();
            Thread.Sleep(1000);

            var ViewCartButton = _driver.FindElement(By.XPath("//a[@title='View cart']"));
            ViewCartButton.Click();
            Thread.Sleep(1000);

            var CouponInputField = _driver.FindElement(By.XPath("//input[@id='coupon_code']"));
            CouponInputField.SendKeys("happybirthday");
            CouponInputField.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            var CouponSuccessMessage = _driver.FindElement(By.XPath("//div[@class='woocommerce-message']"));
            Assert.AreEqual("Coupon code applied successfully.", CouponSuccessMessage.Text);
            Thread.Sleep(2000);

            var QuantityField = _driver.FindElement(By.XPath("//input[@type='number']"));
            QuantityField.Click();
            QuantityField.SendKeys(Keys.ArrowUp);


            var UpdateButton = _driver.FindElement(By.XPath("//button[@value='Update cart']"));
            UpdateButton.Click();
            Thread.Sleep(3000);
            var TotalValue = _driver.FindElement(By.XPath("//td[@data-title='Total']//bdi"));

            Assert.AreEqual("114.00€", TotalValue.Text);


            var proceedToCheckout = _driver.FindElement(By.CssSelector("[class*='checkout-button button alt wc-forward']"));
            proceedToCheckout.Click();

            var billingFirstName = _driver.FindElement(By.Id("billing_first_name"));
            billingFirstName.SendKeys("Aleks");
            var billingLastName = _driver.FindElement(By.Id("billing_last_name"));
            billingLastName.SendKeys("Zhelyazkov");
            var billingCompany = _driver.FindElement(By.Id("billing_company"));
            billingCompany.SendKeys("Aleks & Co");
            var billingCountryWrapper = _driver.FindElement(By.Id("select2-billing_country-container"));
            billingCountryWrapper.Click();
            var billingCountryFilter = _driver.FindElement(By.ClassName("select2-search__field"));
            billingCountryFilter.SendKeys("Bulgaria");
            billingCountryFilter.SendKeys(Keys.Enter);
            var billingAddress = _driver.FindElement(By.Id("billing_address_1"));
            billingAddress.SendKeys("Kinder Garden 2");
            var billingCity = _driver.FindElement(By.Id("billing_city"));
            billingCity.SendKeys("Sofia");
            var billingZip = _driver.FindElement(By.Id("billing_postcode"));
            billingZip.Clear();
            billingZip.SendKeys("10000");
            var billingPhone = _driver.FindElement(By.Id("billing_phone"));
            billingPhone.SendKeys("+359888777666");

            var billingEmail = _driver.FindElement(By.Id("billing_email"));
            var randomizerEmail = RandomizerFactory.GetRandomizer(new FieldOptionsEmailAddress());
            string email = randomizerEmail.Generate();
            billingEmail.SendKeys(email);
            //_purchaseEmail = email;

            //var CreateAccountCheckbox = _driver.FindElement(By.Id("createaccount"));
            //CreateAccountCheckbox.Click();
            Thread.Sleep(3000);

            var placeOrderButton = _driver.FindElement(By.Id("place_order"));
            placeOrderButton.Click();

            Thread.Sleep(15000);

            var receivedMessage = _driver.FindElement(By.ClassName("entry-title"));
            Assert.AreEqual("Order received", receivedMessage.Text);
        }

        [Test, Order(2)]
        public static void SuccessfullyOrdered_When_ExistingCustomer()
        {
            var Falcon9AddToCartButton = _driver.FindElement(By.XPath("//a[@href='?add-to-cart=28']"));
            Falcon9AddToCartButton.Click();
            Thread.Sleep(1000);

            var ViewCartButton = _driver.FindElement(By.XPath("//a[@title='View cart']"));
            ViewCartButton.Click();
            Thread.Sleep(1000);

            var CouponInputField = _driver.FindElement(By.XPath("//input[@id='coupon_code']"));
            CouponInputField.SendKeys("happybirthday");
            CouponInputField.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            var CouponSuccessMessage = _driver.FindElement(By.XPath("//div[@class='woocommerce-message']"));
            Assert.AreEqual("Coupon code applied successfully.", CouponSuccessMessage.Text);
            Thread.Sleep(2000);

            var QuantityField = _driver.FindElement(By.XPath("//input[@type='number']"));
            QuantityField.Click();
            QuantityField.SendKeys(Keys.ArrowUp);


            var UpdateButton = _driver.FindElement(By.XPath("//button[@value='Update cart']"));
            UpdateButton.Click();
            Thread.Sleep(4000);
            var TotalValue = _driver.FindElement(By.XPath("//td[@data-title='Total']//bdi"));

            Assert.AreEqual("114.00€", TotalValue.Text);


            var proceedToCheckout = _driver.FindElement(By.CssSelector("[class*='checkout-button button alt wc-forward']"));
            proceedToCheckout.Click();

            var LoginHyperlink = _driver.FindElement(By.ClassName("showlogin"));
            LoginHyperlink.Click();
            Thread.Sleep(3000);

            var UsernameField = _driver.FindElement(By.XPath("//p[@class='form-row form-row-first']//input[@name='username']"));
            var PasswordField = _driver.FindElement(By.XPath("//p[@class='form-row form-row-last']//input[@name='password']"));
            var LoginButton = _driver.FindElement(By.ClassName("woocommerce-button"));

            UsernameField.SendKeys(_accountEmail);
            PasswordField.SendKeys(_accountPassword);
            LoginButton.Click();
            Thread.Sleep(5000);

            var placeOrderButton = _driver.FindElement(By.Id("place_order"));
            placeOrderButton.Click();

            Thread.Sleep(10000);

            var receivedMessage = _driver.FindElement(By.ClassName("entry-title"));
            Assert.AreEqual("Order received", receivedMessage.Text);

            var lastOrder = _driver.FindElement(By.XPath("//li[@class='woocommerce-order-overview__order order']//strong"));
            _lastOrderNumber = lastOrder.Text;
        }

        [Test, Order(3)]
        public static void CorrectOrderDisplayed_When_NavigateToMyAccount()
        {

            var MyAccountButton = _driver.FindElement(By.LinkText("My account"));
            MyAccountButton.Click();
            Thread.Sleep(3000);

            var UsernameField = _driver.FindElement(By.XPath("//input[@id='username']"));
            var PasswordField = _driver.FindElement(By.XPath("//input[@id='password']"));
            var LoginButton = _driver.FindElement(By.ClassName("woocommerce-button"));

            UsernameField.SendKeys(_accountEmail);
            PasswordField.SendKeys(_accountPassword);
            LoginButton.Click();
            Thread.Sleep(5000);

            var OrdersButton = _driver.FindElement(By.XPath("//a[@href='https://demos.bellatrix.solutions/my-account/orders/']"));
            OrdersButton.Click();
            Thread.Sleep(3000);

            var LastOrderInTable = _driver.FindElement(By.XPath("//tbody//tr//td//a"));

            Assert.AreEqual($"#{_lastOrderNumber}", LastOrderInTable.Text);
        }

    }
}