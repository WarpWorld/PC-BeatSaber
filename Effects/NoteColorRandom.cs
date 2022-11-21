namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 11,
      Name = "Random Note Colors (10 Seconds)",
      Duration = 10
    )]
    class NoteColorRandom : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.colorrandom || HarmonyBase.colorswap) return false;

            HarmonyBase.colorrandom = true;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.colorrandom = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
