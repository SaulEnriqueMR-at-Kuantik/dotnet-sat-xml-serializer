using KPac.Domain.Mapping.Xml.Comprobante.Complementos.Impuestoslocales;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KPac.Domain.Mapping.JsonConverter;

public class ImpuestosLocalesConverter : JsonConverter<List<ImpuestosLocales10>>
{
    public override List<ImpuestosLocales10>? ReadJson(JsonReader reader, Type objectType, List<ImpuestosLocales10>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            // Caso: "ImpuestosLocales": { "ImpuestoLocales10": [ ... ] }
            var pagosArray = token["ImpuestoLocales10"] as JArray;
            if (pagosArray != null)
                return pagosArray.ToObject<List<ImpuestosLocales10>>(serializer);
        }
        else if (token.Type == JTokenType.Array)
        {
            // Caso: "ImpuestosLocales": [ ... ]
            return token.ToObject<List<ImpuestosLocales10>>(serializer);
        }

        // Por defecto, nulo
        return null;
        
    }

    public override void WriteJson(JsonWriter writer, List<ImpuestosLocales10>? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}