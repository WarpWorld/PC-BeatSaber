using System;

namespace CrowdControl.BeatSaber.Effects
{
    public enum EffectResult
    {
        Success,
        Retry,
        Fail
    }

    public class CCEffectInstance
    {

    }

    internal abstract class Effect
    {
        public abstract EffectResult OnStart(CCEffectInstance effectInstance);

        public virtual bool ShouldRun() => false;

        public virtual void OnLoad() { }
    }

    
    [AttributeUsage(AttributeTargets.Class)]
    internal class EffectData : Attribute
    {
        public uint ID;
        public string Name;
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class TimedEffectData : EffectData
    {
        public double Duration;
    }

    internal abstract class TimedEffect : Effect
    {
        public abstract bool OnStop(CCEffectInstance effectInstance, bool force);
    }
}
