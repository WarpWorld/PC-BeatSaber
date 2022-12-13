using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 14,
      Name = "Mirror Notes",
      Duration = 10
    )]
    class NoteMirror : TimedEffect//bug not working
    {
        public override bool StartActions(SchedulerContext context)
        {
            if (!HarmonyBase.IsReady()) return false;

            if (HarmonyBase.mirror) return false;

            HarmonyBase.mirror = true;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.mirror = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }
}
