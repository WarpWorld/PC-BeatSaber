using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrowdControl.BeatSaber.Effects
{
    [EffectData(
        ID = 5,
        Name = "Fast Forward Song"
    )]
    class FastForward : Effect
    {
        public override EffectResult OnStart(CCEffectInstance effectInstance)
        {
            if (!HarmonyBase.isReady()) return EffectResult.Retry;

            try
            {
                var p = HarmonyBase.mgr.GetType().GetField("_gameSongController", BindingFlags.Instance | BindingFlags.NonPublic);
                GameSongController gsc = (GameSongController)p.GetValue(HarmonyBase.mgr);

                p = HarmonyBase.atc.GetType().GetField("_startSongTime", BindingFlags.Instance | BindingFlags.NonPublic);
                float start = (float)p.GetValue(HarmonyBase.atc);

                float time = HarmonyBase.atc.songTime / HarmonyBase.atc.timeScale - start + 10.0f;

                if(time >= HarmonyBase.atc.songEndTime)return EffectResult.Retry;

                Plugin.Log?.Debug($"New Time: {time}");


                p = typeof(BeatmapObjectManager).GetField("_allBeatmapObjects", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                

                List<IBeatmapObjectController> objs = (List<IBeatmapObjectController>)p.GetValue(HarmonyBase.bom);

                var m  = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Type.DefaultBinder, new[] { typeof(NoteController) }, null);
                var m2 = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Type.DefaultBinder, new[] { typeof(ObstacleController) }, null);
                var m3 = typeof(BeatmapObjectManager).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Type.DefaultBinder, new[] { typeof(SliderController) }, null);


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
                 

                gsc.SeekTo(time);
             
            }
            catch (Exception e)
            {
                Plugin.Log?.Debug($"e: {e}");

            }
            return EffectResult.Success;
        }
    }
}
