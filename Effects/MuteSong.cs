using System;
using System.Reflection;
using UnityEngine;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 9,
      Name = "Mute Song (10 Seconds)",
      Duration = 10
    )]
    class MuteSong : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            try {
                var p = HarmonyBase.atc.GetType().GetField("_audioSource", BindingFlags.Instance | BindingFlags.NonPublic);
                AudioSource aso = (AudioSource)p.GetValue(HarmonyBase.atc);

                HarmonyBase.oldvolume = aso.volume;

                aso.volume = 0;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public override bool Stop(bool force)
        {
            try
            {
                var p = HarmonyBase.atc.GetType().GetField("_audioSource", BindingFlags.Instance | BindingFlags.NonPublic);
                AudioSource aso = (AudioSource)p.GetValue(HarmonyBase.atc);

                aso.volume = HarmonyBase.oldvolume;

                HarmonyBase.oldvolume = 0;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
