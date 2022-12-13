using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 10,
      Name = "Swap Note Colors",
      Duration = 10
    )]
    class NoteColorSwap : TimedEffect//bug not working - notes vanish
    {
        public override bool StartActions(SchedulerContext context)
        {
            if (!HarmonyBase.IsReady()) return false;

            if (HarmonyBase.colorrandom || HarmonyBase.colorswap) return false;

            HarmonyBase.colorswap = true;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.colorswap = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }
}
