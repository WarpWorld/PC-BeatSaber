using System;
using System.Reflection;
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
        var p = __instance.GetType().GetField("_level", BindingFlags.Instance | BindingFlags.NonPublic);
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

            var p = __instance.GetType().GetProperty("gameplayCoreSceneSetupData", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
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

            /*var p = __instance.GetType().GetField("_timeScale", BindingFlags.Instance | BindingFlags.NonPublic);
        Plugin.Log?.Debug($"p: {p}");
        p.SetValue(__instance, 2.0f);
        Plugin.Log?.Debug($"scale: {__instance.timeScale}");
        */

            /*
        if (TestEffectPack.Base.speed != 0)
        {

            var p = __instance.GetType().GetField("_timeScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            p.SetValue(__instance, TestEffectPack.Base.speed);

        } else if (TestEffectPack.Base.oldspeed != 0)
        {
            var p = __instance.GetType().GetField("_timeScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            p.SetValue(__instance, TestEffectPack.Base.oldspeed);
            TestEffectPack.Base.oldspeed = 0;
        }
        */
        }
    }


    [HarmonyPatch(typeof(BasicBeatmapObjectManager), "ProcessNoteData")]
    class Patch15B
    {
        public static bool Prefix(BeatmapObjectManager __instance, NoteData noteData)
        {
            Plugin.Log?.Debug("SpawnBasicNote");

            if (noteData.gameplayType == NoteData.GameplayType.Normal)
            {
                if(noteData.time < HarmonyBase.atc.songTime)
                {
                    Plugin.Log?.Debug("Skipped Note");
                    return false;
                }

                if (HarmonyBase.alldown)
                {
                    noteData.ChangeNoteCutDirection(NoteCutDirection.Down);
                }
                if (HarmonyBase.allany)
                {
                    noteData.SetNoteToAnyCutDirection();
                }
                if (HarmonyBase.allrandom)
                {
                    NoteCutDirection[] list = { NoteCutDirection.Any, NoteCutDirection.Down, NoteCutDirection.DownLeft, NoteCutDirection.DownRight, NoteCutDirection.Left, NoteCutDirection.Right, NoteCutDirection.Up, NoteCutDirection.UpLeft, NoteCutDirection.UpRight };

                    Random random = new();
                    int ind = random.Next(0, list.Length);

                    noteData.ChangeNoteCutDirection(list[ind]);
                }

                if (HarmonyBase.colorrandom)
                {
                    noteData.TransformNoteAOrBToRandomType();
                }

                if (HarmonyBase.colorswap)
                {
                    if (noteData.colorType == ColorType.ColorA)
                    {
                        var p = noteData.GetType().GetMethod("set_colorType", BindingFlags.Instance | BindingFlags.NonPublic);
                        p.Invoke(noteData, new object[] { ColorType.ColorB });
                    }
                    else if (noteData.colorType == ColorType.ColorB)
                    {
                        var p = noteData.GetType().GetMethod("set_colorType", BindingFlags.Instance | BindingFlags.NonPublic);
                        p.Invoke(noteData, new object[] { ColorType.ColorA });
                    }
                }
            }

            if (noteData.gameplayType == NoteData.GameplayType.Normal || noteData.gameplayType == NoteData.GameplayType.Bomb)
            {
                /*if (TestEffectPack.Base.mirror)
            {
                var p = TestEffectPack.Base.boc.GetType().GetField("_beatmapData", BindingFlags.Instance | BindingFlags.NonPublic);
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


    public class HarmonyBase
    {
        public static bool patched = false;

        public static bool alldown = false;
        public static bool allany = false;
        public static bool allrandom = false;

        public static bool colorswap = false;
        public static bool colorrandom = false;
        public static bool mirror = false;

        public static bool wavex = false;
        public static bool wavey = false;

        public static float scale = 0;
        public static float speed = 2.0f;
        public static float oldspeed = 0;
        public static float oldvolume = 0;

        public static int frame;
        public static float time;

        public static StandardLevelGameplayManager mgr;
        public static GameplayCoreInstaller gci;
        public static BeatmapCallbacksController boc;
        public static AudioTimeSyncController atc;
        public static BasicBeatmapObjectManager bom;

        public static bool check = false;

        public static FieldInfo GetBackingField(Type type, string propertyName)
        {
            return type.GetField($"<{propertyName}>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public void start()
        {

        }
        


        public static bool isReady()
        {
            return true;
        }



        public static bool inLevel()
        {
            try
            {

            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

    }
}