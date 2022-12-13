using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    //rewind and fast forward
    abstract class SkipDirection : Effect
    {
        protected bool Start(SchedulerContext context, float skip)
        {
            if (!HarmonyBase.IsReady()) return false;

            try
            {
                FieldInfo f_gsc = typeof(StandardLevelGameplayManager).GetField("_gameSongController", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f_gsc == null)
                {
                    NotSupported(context, $"{nameof(StandardLevelGameplayManager)}._gameSongController could not be found.");
                    return false;
                }
                GameSongController gsc = (GameSongController)f_gsc.GetValue(HarmonyBase.mgr);

                FieldInfo f_atsc = typeof(GameSongController).GetField("_audioTimeSyncController", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f_atsc == null)
                {
                    NotSupported(context, $"{nameof(GameSongController)}._audioTimeSyncController could not be found.");
                    return false;
                }
                AudioTimeSyncController atsc = (AudioTimeSyncController)f_atsc.GetValue(HarmonyBase.mgr);

                FieldInfo f_sst = typeof(AudioTimeSyncController).GetField("_startSongTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f_sst == null)
                {
                    NotSupported(context, $"{nameof(AudioTimeSyncController)}._startSongTime could not be found.");
                    return false;
                }
                float start = (float)f_sst.GetValue(atsc);

                float oldTime = HarmonyBase.atc.songTime / HarmonyBase.atc.timeScale - start;
                float newTime = oldTime + skip;

                Plugin.Log?.Debug($"Old Time: {oldTime}");
                Plugin.Log?.Debug($"New Time: {newTime}");

                if (newTime < 0) return false;
                if (newTime >= HarmonyBase.atc.songEndTime) return false;

                FieldInfo f_abo = typeof(BeatmapObjectManager).GetField("_allBeatmapObjects", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                if (f_abo == null)
                {
                    NotSupported(context, $"{nameof(BeatmapObjectManager)}._allBeatmapObjects could not be found.");
                    return false;
                }
                List<IBeatmapObjectController> objs = (List<IBeatmapObjectController>)f_abo.GetValue(HarmonyBase.bom);

                var m = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new[] { typeof(NoteController) }, null);
                if (m == null)
                {
                    NotSupported(context, $"{nameof(BeatmapObjectManager)}.Despawn({nameof(NoteController)}) could not be found.");
                    return false;
                }

                var m2 = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new[] { typeof(ObstacleController) }, null);
                if (m2 == null)
                {
                    NotSupported(context, $"{nameof(BeatmapObjectManager)}.Despawn({nameof(ObstacleController)}) could not be found.");
                    return false;
                }

                var m3 = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new[] { typeof(SliderController) }, null);
                if (m3 == null)
                {
                    NotSupported(context, $"{nameof(BeatmapObjectManager)}.Despawn({nameof(SliderController)}) could not be found.");
                    return false;
                }

                foreach (IBeatmapObjectController obj in objs.ToList())
                {
                    if (obj.GetType().IsSubclassOf(typeof(NoteController)))
                    {
                        m.Invoke(HarmonyBase.bom, new object[] { obj });
                    }
                    if (obj.GetType().IsSubclassOf(typeof(ObstacleController)))
                    {
                        m2.Invoke(HarmonyBase.bom, new object[] { obj });
                    }
                    if (obj.GetType().IsSubclassOf(typeof(SliderController)))
                    {
                        m3.Invoke(HarmonyBase.bom, new object[] { obj });
                    }
                }

                gsc.SeekTo(newTime);
                return true;
            }
            catch (Exception e)
            {
                Plugin.Log?.Debug(e);
                return false;
            }
        }
    }

    [EffectData(
        ID = 4,
        Name = "Rewind Song"
    )]
    class Rewind : SkipDirection
    {
        public override bool StartActions(SchedulerContext context) => Start(context, - 10f);
    }


    [EffectData(
        ID = 5,
        Name = "Fast Forward Song"
    )]
    class FastForward : SkipDirection
    {
        public override bool StartActions(SchedulerContext context) => Start(context, 10f);
    }
}
