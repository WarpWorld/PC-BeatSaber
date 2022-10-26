namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 13,
      Name = "Wavy Notes Vertical (10 Seconds)",
      Duration = 10
    )]
    class NoteWaveY : TimedEffect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            if (HarmonyBase.wavey) return EffectResult.Retry;

            HarmonyBase.wavey = true;

            return EffectResult.Success;
        }

        public override bool OnStop(CCEffectInstance effectInstance, bool force)
        {
            HarmonyBase.wavey = false;

            return true;
        }

        public override bool ShouldRun()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
