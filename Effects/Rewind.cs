using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrowdControl.BeatSaber.Effects
{
    [EffectData(
        ID = 4,
        Name = "Rewind Song"
    )]
    class Rewind : Effect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            try
            {
                var p = HarmonyBase.mgr.GetType().GetField("_gameSongController", BindingFlags.Instance | BindingFlags.NonPublic);
                GameSongController gsc = (GameSongController)p.GetValue(HarmonyBase.mgr);

                p = HarmonyBase.atc.GetType().GetField("_startSongTime", BindingFlags.Instance | BindingFlags.NonPublic);
                float start = (float)p.GetValue(HarmonyBase.atc);

                float time = HarmonyBase.atc.songTime / HarmonyBase.atc.timeScale - start - 10.0f;

                if (time < 0) return false;

                Plugin.Log?.Debug($"New Time: {time}");


                p = typeof(BeatmapObjectManager).GetField("_allBeatmapObjects", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);


                List<IBeatmapObjectController> objs = (List<IBeatmapObjectController>)p.GetValue(HarmonyBase.bom);

                var m = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new[] { typeof(NoteController) }, null);
                var m2 = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new[] { typeof(ObstacleController) }, null);
                var m3 = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new[] { typeof(SliderController) }, null);


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


                //gsc.SeekTo(time);
  
                p = HarmonyBase.atc.GetType().GetField("_songTime", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                var p2 = HarmonyBase.atc.GetType().GetField("_startSongTime", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                Plugin.Log?.Debug($"p: {p}");
                Plugin.Log?.Debug($"p2: {p2}");

                Plugin.Log?.Debug($"songtime: {p.GetValue(HarmonyBase.atc)}");
                //p.SetValue(Base.atc, ((float)p2.GetValue(Base.atc)) + time * Base.atc.timeScale);
                Plugin.Log?.Debug($"songtime: {p.GetValue(HarmonyBase.atc)}");
                
            }
            catch (Exception e)
            {
                Plugin.Log?.Debug($"e: {e}");

            }
            return true;
            
        }
    }
}
