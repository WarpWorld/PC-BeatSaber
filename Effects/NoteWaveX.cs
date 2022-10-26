namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 12,
      Name = "Wavy Notes Horizontal (10 Seconds)",
      Duration = 10
    )]
    class NoteWaveX : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.wavex) return EffectResult.Retry;

            HarmonyBase.wavex = true;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.wavex = false;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
