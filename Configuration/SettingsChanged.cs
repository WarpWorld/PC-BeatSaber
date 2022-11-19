using BeatSaberMarkupLanguage.Attributes;

namespace CrowdControl.BeatSaber.Configuration
{
    public class SettingsChanged : PersistentSingleton<SettingsChanged>
    {
        [UIValue("Login")]
        public string Login;

        [UIAction("ConnectClicked")]
        public void ConnectClicked()
        {
            Plugin.Log?.Debug("on-connect-click starting");
            Plugin.Behavior?.CompleteLogin(Login);
            Plugin.Log?.Debug("on-connect-click finished");
        }
    }
}
