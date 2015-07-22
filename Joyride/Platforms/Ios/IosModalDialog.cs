
using Joyride.Extensions;
using OpenQA.Selenium;

namespace Joyride.Platforms.Ios
{

    abstract public class IosModalDialog : ModalDialog
    {
        protected static readonly ScreenFactory ScreenFactory = new IosScreenFactory();

        public override string Title
        {
            get
            {
                var element = Driver.FindElementWithImplicitWait(By.XPath("//UIAAlert/UIAScrollView/UIAStaticText[1]"));
                return (element == null) ? null : element.Text;
            }
        }
        
        public override string Body
        {
            get
            {
                var element = Driver.FindElementWithImplicitWait(By.XPath("//UIAAlert/UIAScrollView/UIAStaticText[2]"));
                return (element == null) ? null : element.Text;
            }
        }
        

    }
}
