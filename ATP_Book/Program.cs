using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace ATP_Book
{
    class Program
    {
        private static IWebDriver _driver;

        [SetUp]
        public static void Initialize()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://demos.bellatrix.solutions/");
        }

        [TearDown]
        public static void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        public static void SuccessfullyOrdered_When_AddToCartButtonClicked()
        {
            var Falcon9AddToCartButton = _driver.FindElement(By.Id("28"));
            var ViewCartButton = _driver.FindElement(By.ClassName("added_to_cart wc-forward"));

            Falcon9AddToCartButton.Click();
            ViewCartButton.Click();
        }
        
    }
}
