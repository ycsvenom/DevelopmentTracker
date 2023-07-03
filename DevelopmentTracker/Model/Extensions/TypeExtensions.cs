using System;
using System.Linq;

namespace DevelopmentTracker.Model.Extensions;

public static partial class TypeExtensions
{
    public static string[] GetInheritedClasses(this Type type)
    {
        return type.Assembly
                   .GetTypes()
                   .Where(t => t.IsSubclassOf(type) && !t.IsAbstract)
                   .Select(t => t.Name)
                   .ToArray();
    }
}
