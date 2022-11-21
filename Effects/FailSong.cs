using System.Reflection;

namespace CrowdControl.BeatSaber.Effects
{
    [EffectData(
        ID = 3,
        Name = "Fail Song"
    )]
    class BlackWhite : Effect
    {
        public override bool Start()
        {
            if (!HarmonyBase.isReady()) return false;

            var p = HarmonyBase.mgr.GetType().GetField("_initData", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            if (p != null)
            {
                StandardLevelGameplayManager.InitData id = (StandardLevelGameplayManager.InitData)p.GetValue(HarmonyBase.mgr);
                p = id.GetType().GetRuntimeField("failOn0Energy");
                p.SetValue(id, true);
            }

            p = HarmonyBase.mgr.GetType().GetField("_gameEnergyCounter", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            if (p != null)
            {
                GameEnergyCounter gec = (GameEnergyCounter)p.GetValue(HarmonyBase.mgr);

                p = gec.GetType().GetField("_didReach0Energy", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                p.SetValue(gec, false);

                
                var m2 = gec.GetType().GetMethod("set_noFail", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (m2 != null) m2.Invoke(gec, new object[] { false });

                m2 = gec.GetType().GetMethod("set_instaFail", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (m2 != null) m2.Invoke(gec, new object[] { true });
                
                //m2 = typeof(GameEnergyCounter).GetRuntimeMethod("ProcessEnergyChange", new[] { typeof(float) });
                m2 = gec.GetType().GetMethod("ProcessEnergyChange", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                
                var m3 = gec.GetType().GetMethod("set_energy", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                m3.Invoke(gec, new object[] { 1.0f });
                

                if (m2 != null) m2.Invoke(gec, new object[] { -100.0f });
            }


            return true;
        }
    }
}
