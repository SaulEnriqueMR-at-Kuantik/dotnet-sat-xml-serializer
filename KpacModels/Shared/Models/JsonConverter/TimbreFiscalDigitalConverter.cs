using KPac.Domain.Mapping.Xml.TimbreFiscalDigital;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KPac.Domain.Mapping.JsonConverter;

public class TimbreFiscalDigitalConverter: JsonConverter<TimbreFiscalDigital11>
{
    public override TimbreFiscalDigital11? ReadJson(JsonReader reader, Type objectType, TimbreFiscalDigital11? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        var inner = obj["TimbreFiscalDigital11"];
        if (inner == null)
            return null;

        return inner.ToObject<TimbreFiscalDigital11>(serializer);
    }

    public override void WriteJson(JsonWriter writer, TimbreFiscalDigital11? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var wrapper = new JObject
        {
            ["TimbreFiscalDigital11"] = JToken.FromObject(value, serializer)
        };

        wrapper.WriteTo(writer);
    }
}