using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    abstract class SetNJS : TimedEffect
    {
        public bool StartActions(float scale)
        {
            if (!HarmonyBase.IsReady()) return false;

            if (HarmonyBase.njs != 0) return false;
            HarmonyBase.njs = scale;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.njs = 0;
            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }

    /*[TimedEffectData(
        ID = 16,
        Name = "NJS Low (7)",
        Duration = 10
    )]
    class NJSLow : SetNJS
    {
        public override bool StartActions(SchedulerContext context) => StartActions(7f);
    }

    [TimedEffectData(
        ID = 17,
        Name = "NJS Medium (14)",
        Duration = 10
    )]
    class NJSMedium : SetNJS
    {
        public override bool StartActions(SchedulerContext context) => StartActions(14f);
    }

    [TimedEffectData(
        ID = 18,
        Name = "NJS High (19)",
        Duration = 10
    )]
    class NJSHigh : SetNJS
    {
        public override bool StartActions(SchedulerContext context) => StartActions(19f);
    }

    [TimedEffectData(
        ID = 19,
        Name = "NJS Very High (24)",
        Duration = 10
    )]
    class NJSVeryHigh : SetNJS
    {
        public override bool StartActions(SchedulerContext context) => StartActions(24f);
    }*/
}
