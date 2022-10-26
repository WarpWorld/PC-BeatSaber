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
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            try {
                var p = HarmonyBase.atc.GetType().GetField("_audioSource", BindingFlags.Instance | BindingFlags.NonPublic);
                AudioSource aso = (AudioSource)p.GetValue(HarmonyBase.atc);

                HarmonyBase.oldvolume = aso.volume;

                aso.volume = 0;
            }
            catch (Exception e)
            {
                return EffectResult.Retry;
            }
            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
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

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
