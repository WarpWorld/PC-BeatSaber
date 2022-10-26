namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 2,
      Name = "Tiny Notes (30 Seconds)",
      Duration = 30
    )]
    class TinyNotes : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.scale != 0) return EffectResult.Retry;
            HarmonyBase.scale = 0.25f;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.scale = 0;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
