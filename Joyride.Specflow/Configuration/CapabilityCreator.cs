using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Joyride.Specflow.Configuration
{
    public class CapabilityCreator
    {
        public const string Server = "local";
        public static Platform Platform { get; set; }
        public DesiredCapabilities Capabilities { get; set; }
        private readonly JoyrideConfig _config = JoyrideConfig.Settings;
        private readonly NameValueConfigurationCollection _globalCapabilities = JoyrideConfig.Settings.Capabilities.Settings;


        public CapabilityCreator(Platform platform)
        {         
            Platform = platform;
            Capabilities = new DesiredCapabilities();
        }

        public DesiredCapabilities Create()
        {
            LoadCapabilities(_globalCapabilities);
            
            if (Platform == Platform.Android)
            {
                LoadCapabilities(_config.Android.Settings);    
            }
            else // if (Platform == Platform.Ios)
            {
                
            }

            return Capabilities;
        }

        private void LoadCapabilities(NameValueConfigurationCollection settings)
        {
            foreach (var key in settings.AllKeys)
            {                
                Capabilities.SetCapability(settings[key].Name, settings[key].Value);
            }

        }
        private string GetServerAddress(JoyrideConfig config, string key)
        {
            var endpoints = config.Endpoints.Settings;
            var server = endpoints[key];

            if (server == null)
                throw new NotFoundException("Unable to find server with key: " + key);

            return server.Value;
        }
    }
}
