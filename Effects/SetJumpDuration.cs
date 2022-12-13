using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    abstract class SetJumpDuration : TimedEffect
    {
        public bool StartActions(float scale)
        {
            if (HarmonyBase.jd != 0) return false;
            HarmonyBase.jd = scale;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.jd = 0;
            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }

    /*[TimedEffectData(
        ID = 20,
        Name = "JD Near (10)",
        Duration = 10
    )]
    class JumpDurationLow : SetJumpDuration
    {
        public override bool StartActions(SchedulerContext context) => StartActions(10f);
    }

    [TimedEffectData(
        ID = 21,
        Name = "JD Far (20)",
        Duration = 10
    )]
    class JumpDurationHigh : SetJumpDuration
    {
        public override bool StartActions(SchedulerContext context) => StartActions(20f);
    }*/
}
