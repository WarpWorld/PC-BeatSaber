namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 7,
      Name = "All Notes Down Cut (10 Seconds)",
      Duration = 10
    )]
    class AllNotesDown : TimedEffect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            if (HarmonyBase.allany || HarmonyBase.alldown || HarmonyBase.allrandom) return false;

            HarmonyBase.alldown = true;

            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.alldown = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.isReady()) return false;
            return true;
        }
    }
}
