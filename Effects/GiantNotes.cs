namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 1,
      Name = "Giant Notes (30 Seconds)",
      Duration = 30
    )]
    class GiantNotes : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.scale != 0) return false;
            HarmonyBase.scale = 2.5f;

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
