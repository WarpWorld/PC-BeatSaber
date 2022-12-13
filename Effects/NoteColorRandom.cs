using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 11,
      Name = "Random Note Colors",
      Duration = 10
    )]
    class NoteColorRandom : TimedEffect
    {
        public override bool StartActions(SchedulerContext context)
        {
            if (!HarmonyBase.IsReady()) return false;

            if (HarmonyBase.colorrandom || HarmonyBase.colorswap) return false;

            HarmonyBase.colorrandom = true;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.colorrandom = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }
}
