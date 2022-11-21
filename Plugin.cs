using System;
using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using BS_Utils.Gameplay;
using CrowdControl.BeatSaber.Configuration;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using Object = UnityEngine.Object;

namespace CrowdControl.BeatSaber
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        // TODO: If using Harmony, uncomment and change YourGitHub to the name of your GitHub account, or use the form "com.company.project.product"
        //       You must also add a reference to the Harmony assembly in the Libs folder.
        public const string HarmonyId = "com.warpworld.beatsabercc";

        public const string CROWD_CONTROL = "Crowd Control";

        internal static readonly Harmony harmony = new(HarmonyId);

        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        internal static CrowdControlBehavior Behavior => CrowdControlBehavior.Instance;

        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        [Init]
        public Plugin(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log?.Debug("Logger initialized.");
            Client.Binary.Log.OnMessage += (s, _) => Log?.Debug(s);

            BSMLSettings.instance.AddSettingsMenu("Crowd Control", "CrowdControl.BeatSaber.Configuration.settings.bsml", new SettingsChanged());
        }

        #region BSIPA Config
        
        [Init]
        public void InitWithConfig(Config conf)
        {
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            Log?.Debug("Config loaded");
        }
        
        #endregion


        #region Disableable

        /// <summary>
        /// Called when the plugin is enabled (including when the game starts if the plugin is enabled).
        /// </summary>
        [OnEnable]
        public void OnEnable()
        {
            ScoreSubmission.ProlongedDisableSubmission(CROWD_CONTROL);
            new GameObject(CROWD_CONTROL).AddComponent<CrowdControlBehavior>();
            ApplyHarmonyPatches();
        }

        /// <summary>
        /// Called when the plugin is disabled and on Beat Saber quit. It is important to clean up any Harmony patches, GameObjects, and Monobehaviours here.
        /// The game should be left in a state as if the plugin was never started.
        /// Methods marked [OnDisable] must return void or Task.
        /// </summary>
        [OnDisable]
        public void OnDisable()
        {
            if (Behavior != null)
                Object.Destroy(Behavior);
            RemoveHarmonyPatches();
            ScoreSubmission.RemoveProlongedDisable(CROWD_CONTROL);
        }

        /*
        /// <summary>
        /// Called when the plugin is disabled and on Beat Saber quit.
        /// Return Task for when the plugin needs to do some long-running, asynchronous work to disable.
        /// [OnDisable] methods that return Task are called after all [OnDisable] methods that return void.
        /// </summary>
        [OnDisable]
        public async Task OnDisableAsync()
        {
            await LongRunningUnloadTask().ConfigureAwait(false);
        }
        */
        #endregion

        // Uncomment the methods in this section if using Harmony
        #region Harmony
        
        /// <summary>
        /// Attempts to apply all the Harmony patches in this assembly.
        /// </summary>
        internal static void ApplyHarmonyPatches()
        {
            try
            {
                Log?.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Log?.Error("Error applying Harmony patches: " + ex.Message);
                Log?.Debug(ex);
            }
        }

        /// <summary>
        /// Attempts to remove all the Harmony patches that used our HarmonyId.
        /// </summary>
        internal static void RemoveHarmonyPatches()
        {
            try
            {
                // Removes all patches with this HarmonyId
                Harmony.UnpatchID(HarmonyId);
            }
            catch (Exception ex)
            {
                Log?.Error("Error removing Harmony patches: " + ex.Message);
                Log?.Debug(ex);
            }
        }
        
        #endregion
    }
}
