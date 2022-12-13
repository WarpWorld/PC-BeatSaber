using System.Reflection;
using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [EffectData(
        ID = 3,
        Name = "Fail Song"
    )]
    class BlackWhite : Effect//bug not working
    {
        public override bool StartActions(SchedulerContext context)
        {
            if (!HarmonyBase.IsReady()) return false;

            FieldInfo f_id = typeof(StandardLevelGameplayManager).GetField("_initData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (f_id == null)
            {
                NotSupported(context, $"{nameof(StandardLevelGameplayManager)}._initData could not be found.");
                return false;
            }
            StandardLevelGameplayManager.InitData id = (StandardLevelGameplayManager.InitData)f_id.GetValue(HarmonyBase.mgr);
            
            FieldInfo f_fo0e = typeof(StandardLevelGameplayManager.InitData).GetField("failOn0Energy");
            if (f_fo0e == null)
            {
                NotSupported(context, $"{nameof(StandardLevelGameplayManager.InitData)}.failOn0Energy could not be found.");
                return false;
            }
            f_fo0e.SetValue(id, true);

            FieldInfo f_gec = typeof(StandardLevelGameplayManager).GetField("_gameEnergyCounter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (f_gec == null)
            {
                NotSupported(context, $"{nameof(StandardLevelGameplayManager)}._gameEnergyCounter could not be found.");
                return false;
            }
            GameEnergyCounter gec = (GameEnergyCounter)f_gec.GetValue(HarmonyBase.mgr);
            
            FieldInfo f_dr0e = typeof(GameEnergyCounter).GetField("_didReach0Energy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (f_dr0e == null)
            {
                NotSupported(context, $"{nameof(GameEnergyCounter)}._didReach0Energy could not be found.");
                return false;
            }
            f_dr0e.SetValue(gec, false);

            PropertyInfo p_nf = typeof(GameEnergyCounter).GetProperty("noFail", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (p_nf == null)
            {
                NotSupported(context, $"{nameof(GameEnergyCounter)}.noFail could not be found.");
                return false;
            }
            p_nf.SetValue(gec, false);

            PropertyInfo p_if = typeof(GameEnergyCounter).GetProperty("instaFail", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (p_if == null)
            {
                NotSupported(context, $"{nameof(GameEnergyCounter)}.instaFail could not be found.");
                return false;
            }
            p_if.SetValue(gec, true);

            PropertyInfo p_nrg = typeof(GameEnergyCounter).GetProperty("energy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (p_nrg == null)
            {
                NotSupported(context, $"{nameof(GameEnergyCounter)}.energy could not be found.");
                return false;
            }
            p_nrg.SetValue(gec, 1.0f);
            
            MethodInfo m_pec = typeof(GameEnergyCounter).GetMethod("ProcessEnergyChange", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (m_pec == null)
            {
                NotSupported(context, $"{nameof(GameEnergyCounter)}.ProcessEnergyChange could not be found.");
                return false;
            }
            m_pec.Invoke(gec, new object[] { -100.0f });
            return true;
        }
    }
}
