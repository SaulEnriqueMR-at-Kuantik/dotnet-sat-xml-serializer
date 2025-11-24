using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KPac.Domain.Mapping.JsonConverter;

public class IgnoreJsonPropertyOnDeserializeResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        // var prop = base.CreateProperty(member, memberSerialization);
        // // Forzar el uso del nombre C# de la propiedad
        // prop.PropertyName = member.Name;
        // return prop;
        
        var prop = base.CreateProperty(member, memberSerialization);
        prop.Converter = null; // Ignora el atributo [JsonConverter]
        return prop;
    }
}