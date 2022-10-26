namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 10,
      Name = "Swap Note Colors (10 Seconds)",
      Duration = 10
    )]
    class NoteColorSwap : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.colorrandom || HarmonyBase.colorswap) return EffectResult.Retry;

            HarmonyBase.colorswap = true;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.colorswap = false;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
