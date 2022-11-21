namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 12,
      Name = "Wavy Notes Horizontal (10 Seconds)",
      Duration = 10
    )]
    class NoteWaveX : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.wavex) return false;

            HarmonyBase.wavex = true;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.wavex = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
