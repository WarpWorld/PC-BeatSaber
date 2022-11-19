using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrowdControl.BeatSaber
{
    internal static class Extensions
    {
        public static IEnumerable<(Type, T)> GetTypesWithAttribute<T>(this Assembly assembly) where T : Attribute
        {
            foreach (Type type in assembly.GetTypes())
            {
                T attribute = type.GetCustomAttributes(typeof(T), true).OfType<T>().FirstOrDefault();
                if (attribute != null)
                {
                    yield return (type, attribute);
                }
            }
        }
    }
}
