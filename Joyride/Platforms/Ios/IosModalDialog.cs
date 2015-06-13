
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
                var element = Driver.FindElement(By.XPath("//UIAAlert//UIAStaticText[1]"), DefaultWaitSeconds);
                return (element == null) ? null : element.Text;
            }
        }
        
        public override string Body
        {
            get
            {
                var element = Driver.FindElement(By.XPath("//UIAAlert//UIAStaticText[2]"), DefaultWaitSeconds);
                return (element == null) ? null : element.Text;
            }
        }
        

    }
}
