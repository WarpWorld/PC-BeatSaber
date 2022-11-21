using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CrowdControl.BeatSaber.Configuration;
using CrowdControl.BeatSaber.Effects;
using CrowdControl.Client.Binary;
using UnityEngine;

namespace CrowdControl.BeatSaber
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class CrowdControlBehavior : MonoBehaviour, IDisposable
    {
        public static CrowdControlBehavior Instance { get; private set; }

        private BinaryClient m_client;
        private Scheduler m_scheduler;

        ~CrowdControlBehavior() => Dispose(false);

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            try { m_scheduler.Dispose(); }
            catch {/**/}
            try { m_client.Dispose(); }
            catch {/**/}
        }

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            // For this particular MonoBehaviour, we only want one instance to exist at any time, so store a reference to it in a static property
            //   and destroy any that are created while one already exists.
            if (Instance != null)
            {
                Plugin.Log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                DestroyImmediate(this);
                return;
            }
            DontDestroyOnLoad(this); // Don't destroy this object on scene changes
            Instance = this;
            Plugin.Log?.Debug($"{name}: Awake()");

            //== keep init below here

            //BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;

            Plugin.Log?.Debug("Creating binary client.");
            m_client = new();
            m_client.ConnectionStateChanged += state =>
            {
                Plugin.Log?.Debug("Connection state changed to " + Enum.GetName(typeof(ConnectionStateValue), state));
                if (state == ConnectionStateValue.LoggedIn)
                {
                    StringBuilder sb = new();
                    foreach (byte b in m_client.LoginToken) sb.Append(b.ToString("X2"));
                    PluginConfig.Instance.LoginToken = sb.ToString();
                    Plugin.Log?.Debug("Connection established. Permanent token received.");

                    var effects = Assembly.GetExecutingAssembly().
                        GetTypesWithAttribute<EffectData>().
                        Select(d =>
                        {
                            EffectDescription result = new(d.Item2.Name, d.Item2.ID.ToString("D"));
                            if (d.Item2 is TimedEffectData td) result.Duration = result.DefaultDuration = td.Duration;
                            return result;
                        });
                    m_client.LoadMenu(new(effects));
                }
            };

            m_client.EffectRequested += (request, respond) =>
            {
                Plugin.Log?.Debug($"Effect request received [E:{request.EffectID}].");
                //probably want to do something here
                //respond(...);
            };

            m_scheduler = new Scheduler(m_client, Assembly.GetExecutingAssembly());
            m_scheduler.Start();
            Plugin.Log?.Debug("Scheduler initialized.");
            //dispose of s in some dispose method somewhere
        }

        //private void BSEvents_gameSceneLoaded() => BS_Utils.Gameplay.ScoreSubmission.DisableSubmission(Plugin.CROWD_CONTROL);

        /// <summary>
        /// Only ever called once on the first frame the script is Enabled. Start is called after any other script's Awake() and before Update().
        /// </summary>
        private void Start()
        {

        }

        /// <summary>
        /// Called every frame if the script is enabled.
        /// </summary>
        private void Update()
        {

        }

        /// <summary>
        /// Called every frame after every other enabled script's Update().
        /// </summary>
        private void LateUpdate()
        {

        }

        /// <summary>
        /// Called when the script becomes enabled and active
        /// </summary>
        private void OnEnable()
        {
            if (!SetLoginToken())
            {
                Plugin.Log?.Debug("Login token was not set, aborting connect.");
                return;
            }
            Connect();
        }

        /// <summary>
        /// Called when the script becomes disabled or when it is being destroyed.
        /// </summary>
        private void OnDisable()
        {
            Plugin.Log?.Debug("Cleaning up existing connections.");
            try { m_client.Close(true); }
            catch {/**/}
        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            if (Instance == this)
                Instance = null; // This MonoBehaviour is being destroyed, so set the static instance property to null.

        }
        #endregion

        private bool SetLoginToken()
        {
            string login = PluginConfig.Instance.LoginToken;
            if (!string.IsNullOrWhiteSpace(login))
            {
                Plugin.Log?.Debug($"Using login token: {login}");
                int len = login.Length;
                byte[] loginBytes = new byte[len / 2];
                for (int i = 0; i < len; i += 2)
                {
                    loginBytes[i / 2] = byte.Parse(login.Substring(i, 2), NumberStyles.AllowHexSpecifier);
                }
                m_client.LoginToken = loginBytes;
                return true;
            }

            Plugin.Log?.Debug("Login token not set.");
            return false;
        }

        public Task Connect()
        {
            Plugin.Log?.Debug("Connecting to Crowd Control.");
            var result = m_client.Connect(135, "staging-gamesocket.crowdcontrol.live", 56441, false);
            result.ContinueWith(async r =>
            {
                if (await r) Plugin.Log?.Debug("Socket opened successfully.");
                else Plugin.Log?.Debug("Socket failed to open.");
            });
            return result;
        }

        public async Task CompleteLogin(string login)
        {
            Plugin.Log?.Debug("Cleaning up existing connections.");
            try { await m_client.Close(true); }
            catch {/**/}

            if (string.IsNullOrWhiteSpace(login))
            {
                Plugin.Log?.Debug("Login key was blank. Aborting login.");
                return;
            }
            Plugin.Log?.Debug($"Temporary key was: {login}");

            m_client.LoginToken = null;
            m_client.TemporaryToken = login;
            await Connect();
        }
    }
}
