using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestHometask
{
    public class Tests
    {
        public ChromeDriver Driver;
        private readonly By emailInputLocator = By.Name("email");
        private readonly By buttonLocator = By.Id("sendMe");
        private readonly By radioBoyLocator = By.Id("boy");
        private readonly By radioGirlLocator = By.Id("girl");
        private readonly By resultTextLLocator = By.ClassName("result-text");
        private readonly By resultEmailLocator = By.ClassName("your-email");
        private readonly By anotherEmailLinkLocator = By.Id("anotherEmail");
        private const string validEmail = "1@mail.ru";

        private void SendRequest(string email, By radioButtonLocator)
        {
            Driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice");
            Driver.FindElement(radioButtonLocator).Click();
            Driver.FindElement(emailInputLocator).SendKeys(email);
            Driver.FindElement(buttonLocator).Click();
        }

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            Driver = new ChromeDriver(options);
        }

        [Test]
        public void parrotNameSite_EmptyEmail_ErrorText()
        {
            SendRequest(string.Empty, radioBoyLocator);
            var errorText = Driver.FindElementByClassName("form-error");
            Assert.IsTrue(errorText.Displayed, "Не отображается текст ошибки \"Введите email\"");
            Assert.AreEqual("Введите email", errorText.Text, "Неверный текст ошибки");
        }

        [Test]
        public void parrotNameSite_InvalidEmail_ErrorText()
        {
            SendRequest("email", radioBoyLocator);
            var errorText = Driver.FindElementByClassName("form-error");
            Assert.IsTrue(errorText.Displayed, "Не отображается текст ошибки \"Некорректный email\"");
            Assert.AreEqual("Некорректный email", errorText.Text, "Неверный тескт ошибки");
        }

        [Test]
        public void parrotNameSite_FillFormWithEmail_Success()
        {
            var expectedEmail = validEmail;
            SendRequest(expectedEmail, radioBoyLocator);
            Assert.AreEqual(expectedEmail, Driver.FindElement(resultEmailLocator).Text, "Заявка сделана не на тот email");
        }

        [Test]
        public void parrotNameSite_RequestForBoy_BoyText()
        {
            var boyText = "Хорошо, мы пришлём имя для вашего мальчика на e-mail:";
            SendRequest(validEmail, radioBoyLocator);
            Assert.AreEqual(boyText, Driver.FindElement(resultTextLLocator).Text, "Неверный текст подтверждения заявки на имя мальчика");
        }

        [Test]
        public void parrotNameSite_RequestForGirl_GirlText()
        {
            var girlText = "Хорошо, мы пришлём имя для вашей девочки на e-mail:";
            SendRequest(validEmail, radioGirlLocator);
            Assert.AreEqual(girlText, Driver.FindElement(resultTextLLocator).Text, "Неверный текст подтверждения заявки на имя девочки");
        }

        [Test]
        public void parrotNameSite_СlickAnotherEmail_EmailInputIsEmpty()
        {
            SendRequest(validEmail, radioBoyLocator);
            Driver.FindElement(anotherEmailLinkLocator).Click();
            Assert.AreEqual(string.Empty, Driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле не очистилось поле email");
            Assert.IsFalse(Driver.FindElements(anotherEmailLinkLocator).Count == 0, "Не исчезла ссылка для ввода другого e-mail");
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
        }
    }
}
