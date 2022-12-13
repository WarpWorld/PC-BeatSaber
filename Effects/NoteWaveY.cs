using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 13,
      Name = "Wavy Notes Vertical",
      Duration = 10
    )]
    class NoteWaveY : TimedEffect
    {
        public override bool StartActions(SchedulerContext context)
        {
            if (!HarmonyBase.IsReady()) return false;

            if (HarmonyBase.wavey) return false;

            HarmonyBase.wavey = true;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.wavey = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }
}
