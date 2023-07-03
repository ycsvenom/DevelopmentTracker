using System;
using System.Linq;
using System.Text.Json;

namespace DevelopmentTracker.Model.Extensions;

public static class JsonElementExtensions
{
    public static object Deserialize(this JsonElement element, string typeName)
    {
        var type = typeof(JsonSerializer);
        var method = type.GetMethods()
                         .Where(m => m.Name == "Deserialize")
                         .FirstOrDefault(m => m.IsGenericMethod && m.IsStatic && m.GetParameters()[0].ParameterType == typeof(JsonElement))!;
        var generic = method.MakeGenericMethod(Type.GetType($"{typeof(Tracker).Namespace}.{typeName}")!);
        return generic.Invoke(element, new object[] { element, new JsonSerializerOptions() })!;
    }
}
