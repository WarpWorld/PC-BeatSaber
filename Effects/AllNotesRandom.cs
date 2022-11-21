namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 8,
      Name = "All Notes Random Direction (10 Seconds)",
      Duration = 10
    )]
    class AllNotesRandom : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.allany || HarmonyBase.alldown || HarmonyBase.allrandom) return false;

            HarmonyBase.allrandom = true;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.allrandom = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
