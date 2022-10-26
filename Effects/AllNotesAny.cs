namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 6,
      Name = "All Notes Any Direction (10 Seconds)",
      Duration = 10
    )]
    class AllNotesAny : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.allany || HarmonyBase.alldown || HarmonyBase.allrandom) return EffectResult.Retry;

            HarmonyBase.allany = true;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.allany = false;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
