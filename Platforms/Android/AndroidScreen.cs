
using OpenQA.Selenium;

namespace Joyride.Platforms.Android
{
    abstract public class AndroidScreen : Screen
    {
        protected static ScreenFactory ScreenFactory = new AndroidScreenFactory();

        public override Screen EnterText(string elementName, string text)
        {
            var element = FindElement(elementName);

            if (element == null)
                throw new NoSuchElementException("Cannot find element:  " + elementName);

            //Appium clears it regardless of this being called or not.
            //element.Clear();
            element.Click();
            element.SendKeys(text);
            Driver.HideKeyboard();
            //Driver.Navigate().Back();
            return this;
        }
    }
}
