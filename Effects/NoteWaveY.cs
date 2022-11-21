namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 13,
      Name = "Wavy Notes Vertical (10 Seconds)",
      Duration = 10
    )]
    class NoteWaveY : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.wavey) return false;

            HarmonyBase.wavey = true;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.wavey = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
