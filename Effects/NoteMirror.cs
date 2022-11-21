namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 667,
      Name = "Mirror Notes (20 Seconds)",
      Duration = 20
    )]
    class NoteMirror : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.mirror) return false;

            HarmonyBase.mirror = true;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.mirror = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
