namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 1,
      Name = "Giant Notes (30 Seconds)",
      Duration = 30
    )]
    class GiantNotes : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.scale != 0) return EffectResult.Retry;
            HarmonyBase.scale = 2.5f;

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

        public override void OnLoad()
        {
            HarmonyBase.patch();
        }
    }
}
