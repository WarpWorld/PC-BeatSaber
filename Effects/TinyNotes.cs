namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 2,
      Name = "Tiny Notes (30 Seconds)",
      Duration = 30
    )]
    class TinyNotes : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.scale != 0) return false;
            HarmonyBase.scale = 0.25f;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.scale = 0;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
