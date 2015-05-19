using HandyConfig.Configuration;

namespace Joyride.Specflow.Configuration
{
    public class CapabilitiesElement : HandyConfigElement
    {
        public static NameValueTypeElementCollection Settings = JoyrideSectionHandler.Settings.Capabilities.NameValueTypes;

    }
}