using HandyConfig.Configuration;

namespace Joyride.Specflow.Configuration
{
    public class ServersElement : HandyConfigElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Servers.NameValueTypes;

 
    }
}