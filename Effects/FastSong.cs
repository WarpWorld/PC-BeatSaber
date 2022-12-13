using System;
using System.Reflection;
using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 15,
      Name = "Fast Song",
      Duration = 10
    )]
    class FastSong : TimedEffect//bug not working
    {
        public override bool StartActions(SchedulerContext context)
        {
            if (!HarmonyBase.IsReady()) return false;
            if (HarmonyBase.speed != 0) return false;

            FieldInfo f_ssd = typeof(GameplayCoreInstaller).GetField("_sceneSetupData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Plugin.Log?.Debug($"p: {f_ssd}");

            GameplayCoreSceneSetupData sd = (GameplayCoreSceneSetupData)f_ssd.GetValue(HarmonyBase.gci);

            HarmonyBase.oldspeed = sd.gameplayModifiers.songSpeedMul;

            Plugin.Log?.Debug($"oldspeed: {HarmonyBase.oldspeed}");

            HarmonyBase.speed = HarmonyBase.oldspeed * 1.75f;

            //var p2 = typeof(UnityEngine.Time).GetProperty("timeSinceLevelLoad", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            //Plugin.Log?.Debug($"p2: {p2}");

            //float time = ((float)p2.GetValue(null));

            //Plugin.Log?.Debug($"time: {time}");

            try
            {
                //p2.SetValue(null, time / 1.75f);
            }
            catch(Exception e)
            {
                Plugin.Log?.Debug($"{e}");
            }

            Plugin.Log?.Debug($"speed: {HarmonyBase.speed}");

            f_ssd = HarmonyBase.gci.GetType().GetField("_audioManager", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Plugin.Log?.Debug($"p: {f_ssd}");

            AudioManagerSO am = (AudioManagerSO)f_ssd.GetValue(HarmonyBase.gci);

            //am.musicPitch = 1.0f / Base.speed;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.scale = 0;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }
}
