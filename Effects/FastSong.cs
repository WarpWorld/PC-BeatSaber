﻿using System;
using System.Reflection;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 666,
      Name = "Fast Song (10 Seconds)",
      Duration = 10
    )]
    class FastSong : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            
            
            if (HarmonyBase.speed != 0) return false;

            var p = HarmonyBase.gci.GetType().GetField("_sceneSetupData", BindingFlags.Instance | BindingFlags.NonPublic);

            Plugin.Log?.Debug($"p: {p}");

            GameplayCoreSceneSetupData sd = (GameplayCoreSceneSetupData)p.GetValue(HarmonyBase.gci);

            HarmonyBase.oldspeed = sd.gameplayModifiers.songSpeedMul;

            Plugin.Log?.Debug($"oldspeed: {HarmonyBase.oldspeed}");

            HarmonyBase.speed = HarmonyBase.oldspeed * 1.75f;

            //var p2 = typeof(UnityEngine.Time).GetProperty("timeSinceLevelLoad", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

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

            p = HarmonyBase.gci.GetType().GetField("_audioManager", BindingFlags.Instance | BindingFlags.NonPublic);

            Plugin.Log?.Debug($"p: {p}");

            AudioManagerSO am = (AudioManagerSO)p.GetValue(HarmonyBase.gci);

            //am.musicPitch = 1.0f / Base.speed;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.scale = 0;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
