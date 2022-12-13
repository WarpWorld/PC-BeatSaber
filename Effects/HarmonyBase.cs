using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Random = System.Random;

namespace CrowdControl.BeatSaber.Effects
{
    /*
[HarmonyPatch(typeof(SimpleLevelStarter), "StartLevel")]
class Patch
{
    public static void Postfix(SimpleLevelStarter __instance)
    {
        Plugin.Log?.Debug($"StartLevel A");
        var p = __instance.GetType().GetField("_level", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        BeatmapLevelSO cw = (BeatmapLevelSO)p.GetValue(__instance);

        Plugin.Log?.Debug($"Song Start - {cw.songName} by {cw.songAuthorName} Length: {cw.songDuration}");
    }
}
*/
/*
[HarmonyPatch(typeof(BeatmapDataSO), "Load")]
class Patch2
{
    public static void Postfix(BeatmapLevelSO __instance)
    {
        Plugin.Log?.Debug($"Load");
        Plugin.Log?.Debug($"{__instance}");
        //Plugin.Log?.Debug($"Song Start - {__instance.songName} by {__instance.songAuthorName} Length: {__instance.songDuration}");
    }
}
*/


    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "Init")]
    class Patch4
    {
        public static void Postfix(StandardLevelScenesTransitionSetupDataSO __instance)
        {
            Plugin.Log?.Debug("Init");
            Plugin.Log?.Debug($"{__instance}");

            var p = __instance.GetType().GetProperty("gameplayCoreSceneSetupData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            GameplayCoreSceneSetupData gsd = (GameplayCoreSceneSetupData)p.GetValue(__instance);


            Plugin.Log?.Debug($"Song Start B - {gsd.previewBeatmapLevel.songName} by {gsd.previewBeatmapLevel.songAuthorName} Length: {gsd.previewBeatmapLevel.songDuration}");
        }
    }



    [HarmonyPatch(typeof(AudioTimeSyncController), "StartSong")]
    class Patch7
    {
        public static void Postfix(AudioTimeSyncController __instance)
        {
            Plugin.Log?.Debug("StartSong2");
            HarmonyBase.atc = __instance;
        }
    }


/*
[HarmonyPatch(typeof(AudioTimeSyncController), "get_timeScale")]
class Patch7B
{
    public static void Postfix(float __result)
    {
        Plugin.Log?.Debug($"get_timeScale");
        if (TestEffectPack.Base.speed != 0) __result = TestEffectPack.Base.speed;

        Plugin.Log?.Debug($"{__result}");
    }
}

[HarmonyPatch(typeof(GameplayModifiers), "get_songSpeedMul")]
class Patch7C
{
    public static void Postfix(float __result)
    {
        Plugin.Log?.Debug($"get_songSpeedMul");
        if (TestEffectPack.Base.speed != 0) __result = TestEffectPack.Base.speed;

        Plugin.Log?.Debug($"get_songSpeedMul: {__result}");

        ///Plugin.Log?.Debug($"speedMul: {TestEffectPack.Base.gci.Container.GetComponent<SongSpeedData>().speedMul}");
    }
}
*/



    [HarmonyPatch(typeof(GameNoteController), "Init")]
    class Patch11
    {
        public static void Prefix(ref float uniformScale)
        {
            if (HarmonyBase.scale != 0)
            {
                uniformScale = HarmonyBase.scale;
            }
        }
    }

    [HarmonyPatch(typeof(StandardLevelGameplayManager), "Start")]
    class Patch12
    {
        public static void Prefix(StandardLevelGameplayManager __instance)
        {
            Plugin.Log?.Debug($"Manager {__instance}");
            HarmonyBase.mgr = __instance;
        }
    }

    [HarmonyPatch(typeof(GameplayCoreInstaller), "InstallBindings")]
    class Patch13
    {
        public static void Prefix(GameplayCoreInstaller __instance)
        {
            Plugin.Log?.Debug($"Installer {__instance}");
            HarmonyBase.gci = __instance;
        }
    }


    [HarmonyPatch(typeof(AudioTimeSyncController), "Update")]
    class Patch14
    {
        public static void Prefix(AudioTimeSyncController __instance)
        {
            HarmonyBase.frame++;
            HarmonyBase.time += Time.deltaTime;

            /*var p = __instance.GetType().GetField("_timeScale", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        Plugin.Log?.Debug($"p: {p}");
        p.SetValue(__instance, 2.0f);
        Plugin.Log?.Debug($"scale: {__instance.timeScale}");
        */

            /*
        if (TestEffectPack.Base.speed != 0)
        {

            var p = __instance.GetType().GetField("_timeScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | BindingFlags.NonPublic);
            p.SetValue(__instance, TestEffectPack.Base.speed);

        } else if (TestEffectPack.Base.oldspeed != 0)
        {
            var p = __instance.GetType().GetField("_timeScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | BindingFlags.NonPublic);
            p.SetValue(__instance, TestEffectPack.Base.oldspeed);
            TestEffectPack.Base.oldspeed = 0;
        }
        */
        }
    }


    [HarmonyPatch(typeof(BasicBeatmapObjectManager), "ProcessNoteData")]
    class Patch15B
    {
        private static readonly Random RNG = new();
        private static readonly NoteCutDirection[] CUT_TYPES = {
            NoteCutDirection.Any,
            NoteCutDirection.UpLeft,
            NoteCutDirection.Up,
            NoteCutDirection.UpRight,
            NoteCutDirection.Left,
            NoteCutDirection.Right,
            NoteCutDirection.DownLeft,
            NoteCutDirection.Down,
            NoteCutDirection.DownRight
        };

        public static bool Prefix(BeatmapObjectManager __instance, NoteData noteData)
        {
            if (noteData.gameplayType == NoteData.GameplayType.Normal)
            {
                if(noteData.time < HarmonyBase.atc.songTime)
                {
                    Plugin.Log?.Debug("Skipped Note");
                    return false;
                }

                HarmonyBase.FireNextNoteActions();

                switch (HarmonyBase.arrowModifier)
                {
                    case HarmonyBase.ArrowDirection.Dot:
                        noteData.SetNoteToAnyCutDirection();
                        break;
                    case HarmonyBase.ArrowDirection.UpLeft:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.UpLeft);
                        break;
                    case HarmonyBase.ArrowDirection.Up:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.Up);
                        break;
                    case HarmonyBase.ArrowDirection.UpRight:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.UpRight);
                        break;
                    case HarmonyBase.ArrowDirection.Left:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.Left);
                        break;
                    case HarmonyBase.ArrowDirection.Right:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.Right);
                        break;
                    case HarmonyBase.ArrowDirection.DownLeft:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.DownLeft);
                        break;
                    case HarmonyBase.ArrowDirection.Down:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.Down);
                        break;
                    case HarmonyBase.ArrowDirection.DownRight:
                        noteData.ChangeNoteCutDirection(NoteCutDirection.DownRight);
                        break;
                    case HarmonyBase.ArrowDirection.Random:
                        noteData.ChangeNoteCutDirection(CUT_TYPES[RNG.Next(0, CUT_TYPES.Length)]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (HarmonyBase.colorrandom)
                {
                    noteData.TransformNoteAOrBToRandomType();
                }

                if (HarmonyBase.colorswap)
                {
                    var p = typeof(NoteData).GetProperty("colorType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    switch (noteData.colorType)
                    {
                        case ColorType.ColorA:
                            p.SetValue(noteData, ColorType.ColorB);
                            break;
                        case ColorType.ColorB:
                            p.SetValue(noteData, ColorType.ColorA);
                            break;
                    }
                }
            }

            if (noteData.gameplayType == NoteData.GameplayType.Normal || noteData.gameplayType == NoteData.GameplayType.Bomb)
            {
                /*if (TestEffectPack.Base.mirror)
            {
                var p = TestEffectPack.Base.boc.GetType().GetField("_beatmapData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                Plugin.Log?.Debug($"p: {p}");
                IReadonlyBeatmapData mapdat = (IReadonlyBeatmapData)p.GetValue(TestEffectPack.Base.boc);
                Plugin.Log?.Debug($"lines: {mapdat.numberOfLines}");
                noteData.Mirror(mapdat.numberOfLines);
            }*/
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(BeatmapDataTransformHelper), "CreateTransformedBeatmapData")]
    class Patch15C
    {
        public static void Prefix()
        {
            Plugin.Log?.Debug("CreateTransformedBeatmapData");
        }
    }

    [HarmonyPatch(typeof(BasicBeatmapObjectManager), "Init")]
    class Patch16
    {
        public static void Prefix(BasicBeatmapObjectManager __instance)
        {
            Plugin.Log?.Debug($"bom start {__instance}");
            HarmonyBase.bom = __instance;
        }
    }

    [HarmonyPatch(typeof(BeatmapCallbacksController), "ManualUpdate")]
    class Patch17
    {
        public static void Prefix(BeatmapCallbacksController __instance)
        {
            //Plugin.Log?.Debug($"Beatmap Callback {__instance}");
            HarmonyBase.boc = __instance;
        }
    }


    [HarmonyPatch(typeof(NoteController), "Update")]
    class Patch18
    {
        public static void Postfix(NoteController __instance)
        {
            Vector3 pos = __instance.transform.localPosition;

            if (HarmonyBase.wavey)
                pos.y += (float)Math.Sin((double)HarmonyBase.time * 5.0f) * 0.60f;

            if (HarmonyBase.wavex)
                pos.x += (float)Math.Sin((double)HarmonyBase.time * 5.0f) * 0.60f;

            __instance.transform.localPosition = pos;
        }
    }


    [HarmonyPatch(typeof(GameEnergyCounter), "ProcessEnergyChange")]
    class Patch19
    {
        public static void Prefix(GameEnergyCounter __instance)
        {
            /*System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
        Type calledFromType = trace.GetFrame(1).GetMethod().ReflectedType;
        Plugin.Log?.Debug($"Energy Called From 1: {calledFromType}");
        calledFromType = trace.GetFrame(2).GetMethod().ReflectedType;
        Plugin.Log?.Debug($"Energy Called From 2: {calledFromType}");*/
        }
    }


    public static class HarmonyBase
    {
        public enum ArrowDirection
        {
            Dot,

            UpLeft,
            Up,
            UpRight,
            Left,
            Right,
            DownLeft,
            Down,
            DownRight,

            Random
        }

        public static ArrowDirection? arrowModifier;

        public static bool colorswap = false;
        public static bool colorrandom = false;

        public static bool mirror = false;

        public static bool wavex = false;
        public static bool wavey = false;


        public static float njs;
        public static float jd;
        public static float scale;
        public static float speed = 2f;

        public static float oldspeed;
        public static float oldvolume;

        public static int frame;
        public static float time;

        public static StandardLevelGameplayManager mgr;
        public static GameplayCoreInstaller gci;
        public static BeatmapCallbacksController boc;
        public static AudioTimeSyncController atc;
        public static BasicBeatmapObjectManager bom;

        public static bool check = false;

        public static bool IsReady()
        {
            return true;
        }

        public static bool InLevel()
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                Plugin.Log?.Error(e);
                return false;
            }
        }

        private static readonly ConcurrentQueue<Action> m_nextNoteActions = new();
        public static void NextNoteOnce(Action action) => m_nextNoteActions.Enqueue(action);
        public static event Action NextNote;
        internal static void FireNextNoteActions()
        {
            try { NextNote?.Invoke(); }
            catch (Exception e) { Plugin.Log?.Error(e); }
            while (m_nextNoteActions.TryDequeue(out Action a))
            {
                try { a(); }
                catch (Exception e) { Plugin.Log?.Error(e); }
            }
        }
        public static Task NextNoteAsync()
        {
            TaskCompletionSource<object> tcs = new();
            m_nextNoteActions.Enqueue(() => tcs.SetResult(null));
            return tcs.Task;
        }
    }
}
