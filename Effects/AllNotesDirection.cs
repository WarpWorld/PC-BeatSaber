using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    abstract class AllNotesDirection : TimedEffect
    {
        public abstract HarmonyBase.ArrowDirection ArrowDirection { get; }

        public override bool StartActions(SchedulerContext context)
        {
            HarmonyBase.arrowModifier = ArrowDirection;
            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.arrowModifier = null;
            return true;
        }

        public override bool IsReady() => HarmonyBase.IsReady();
    }

    [TimedEffectData(
        ID = 6,
        Name = "All Notes Any Direction",
        Duration = 10
    )]
    class AllNotesAny : AllNotesDirection
    {
        public override HarmonyBase.ArrowDirection ArrowDirection => HarmonyBase.ArrowDirection.Dot;
    }

    [TimedEffectData(
        ID = 7,
        Name = "All Notes Down Cut",
        Duration = 10
    )]
    class AllNotesDown : AllNotesDirection
    {
        public override HarmonyBase.ArrowDirection ArrowDirection => HarmonyBase.ArrowDirection.Down;
    }

    [TimedEffectData(
        ID = 8,
        Name = "All Notes Random Direction",
        Duration = 10
    )]
    class AllNotesRandom : AllNotesDirection
    {
        public override HarmonyBase.ArrowDirection ArrowDirection => HarmonyBase.ArrowDirection.Random;
    }
}
