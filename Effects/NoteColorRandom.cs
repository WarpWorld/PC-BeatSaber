namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 11,
      Name = "Random Note Colors (10 Seconds)",
      Duration = 10
    )]
    class NoteColorRandom : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.colorrandom || HarmonyBase.colorswap) return EffectResult.Retry;

            HarmonyBase.colorrandom = true;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.colorrandom = false;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
