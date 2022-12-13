using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    abstract class NoteSize : TimedEffect
    {
        public bool Start(float scale)
        {
            if (!HarmonyBase.IsReady()) return false;

            if (HarmonyBase.scale != 0) return false;
            HarmonyBase.scale = scale;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.scale = 0;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }

    [TimedEffectData(
        ID = 1,
        Name = "Large Notes",
        Duration = 10
    )]
    class LargeNotes : NoteSize
    {
        public override bool StartActions(SchedulerContext context) => Start(1.6f);
    }

    [TimedEffectData(
        ID = 2,
        Name = "Small Notes",
        Duration = 10
    )]
    class SmallNotes : NoteSize
    {
        public override bool StartActions(SchedulerContext context) => Start(0.6f);
    }
}
