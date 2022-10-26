namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 7,
      Name = "All Notes Down Cut (10 Seconds)",
      Duration = 10
    )]
    class AllNotesDown : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.allany || HarmonyBase.alldown || HarmonyBase.allrandom) return EffectResult.Retry;

            HarmonyBase.alldown = true;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.alldown = false;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
