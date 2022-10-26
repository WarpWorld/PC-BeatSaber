namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 667,
      Name = "Mirror Notes (20 Seconds)",
      Duration = 20
    )]
    class NoteMirror : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.mirror) return EffectResult.Retry;

            HarmonyBase.mirror = true;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.mirror = false;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
