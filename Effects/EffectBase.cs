using System;
using CrowdControl.Client.Binary;

namespace CrowdControl.BeatSaber.Effects
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class EffectData : Attribute
    {
        public uint ID;
        public string Name;
    }

    internal abstract class Effect : CrowdControl.Client.Binary.Effect
    {
        protected EffectData m_effectData;

        protected Effect() => m_effectData = GetType().GetEffectData();

        public override uint ID => m_effectData.ID;

        public override string Code => m_effectData.Name;

        public override EffectType Type => EffectType.Instant;

        private static readonly TimeSpan MAX_NOTE_WAIT = TimeSpan.FromSeconds(5d);
        public override bool Start(SchedulerContext context)
            => HarmonyBase.NextNoteAsync().Wait(MAX_NOTE_WAIT) && StartActions(context);

        public abstract bool StartActions(SchedulerContext context);

        public override bool IsReady() => false;

        public static void NotSupported(SchedulerContext context, string message = "")
        {
            context.Cancel(EffectStatus.FailPermanent, message);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class TimedEffectData : EffectData
    {
        public double Duration;
    }

    internal abstract class TimedEffect : Effect
    {
        public override EffectType Type => EffectType.Timed;

        public override TimeSpan DefaultDuration => TimeSpan.FromSeconds((m_effectData as TimedEffectData)?.Duration ?? 10d);

        public override bool Stop() => StopActions(false);
        public abstract bool StopActions(bool force);
    }

    internal static class EffectEx
    {
        public static EffectData GetEffectData(this Type type)
        {
            if (type.IsAssignableTo(typeof(TimedEffect)))
                return type.TryGetAttribute(out TimedEffectData tData) ? tData : null;
            return type.TryGetAttribute(out EffectData data) ? data : null;
        }
    }
}
