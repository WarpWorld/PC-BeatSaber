using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 12,
      Name = "Wavy Notes Horizontal",
      Duration = 10
    )]
    class NoteWaveX : TimedEffect
    {
        public override bool StartActions(SchedulerContext context)
        {
            if (!HarmonyBase.IsReady()) return false;

            if (HarmonyBase.wavex) return false;

            HarmonyBase.wavex = true;

            return true;
        }

        public override bool StopActions(bool force)
        {
            HarmonyBase.wavex = false;

            return true;
        }

        public override bool IsReady()
        {
            if (!HarmonyBase.IsReady()) return false;
            return true;
        }
    }
}
