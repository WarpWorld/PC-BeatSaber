namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 10,
      Name = "Swap Note Colors (10 Seconds)",
      Duration = 10
    )]
    class NoteColorSwap : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.colorrandom || HarmonyBase.colorswap) return false;

            HarmonyBase.colorswap = true;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.colorswap = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
