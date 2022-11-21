using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [TimedEffectData(
      ID = 6,
      Name = "All Notes Any Direction (10 Seconds)",
      Duration = 10
    )]
    class AllNotesAny : TimedEffect
    {
        public override bool Start()
        {
            Log.Debug("Starting effect AllNotesAny");
            if (!HarmonyBase.isReady())
            {
                Log.Debug("HarmonyBase is not ready. Aborting...");
                return false;
            }

            if (HarmonyBase.allany || HarmonyBase.alldown || HarmonyBase.allrandom)
            {
                Log.Debug("Conflict detected. Aborting...");
                return false;
            }

            HarmonyBase.allany = true;
            Log.Debug("Started effect AllNotesAny");
            return true;
        }

        public override bool Stop(bool force)
        {
            HarmonyBase.allany = false;
            Log.Debug("Stopped effect AllNotesAny");
            return true;
        }

        public override bool IsReady()
        {
            Log.Debug("Checking effect AllNotesAny");
            if (!HarmonyBase.isReady())
            {
                Log.Debug("HarmonyBase is not ready. Aborting...");
                return false;
            }
            return true;
        }
    }
}
