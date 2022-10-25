using BeatSaberMarkupLanguage.Attributes;

namespace CrowdControl.BeatSaber.Configuration
{
    public class SettingsChanged : PersistentSingleton<SettingsChanged>
    {
        [UIValue("login")]
        public string Login;
    }
}
