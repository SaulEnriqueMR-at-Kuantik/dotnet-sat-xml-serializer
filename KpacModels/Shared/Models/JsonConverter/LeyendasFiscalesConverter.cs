using KpacModels.Shared.Models.Comprobante.Complementos.LeyendasFiscales;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KPac.Domain.Mapping.JsonConverter;

public class LeyendasFiscalesConverter : JsonConverter<List<LeyendasFiscales10>>
{
    public override List<LeyendasFiscales10>? ReadJson(JsonReader reader, Type objectType, List<LeyendasFiscales10>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            // Caso: "LeyendasFiscales": { "LeyendasFiscales10": [ ... ] }
            var pagosArray = token["LeyendasFiscales10"] as JArray;
            if (pagosArray != null)
                return pagosArray.ToObject<List<LeyendasFiscales10>>(serializer);
        }
        else if (token.Type == JTokenType.Array)
        {
            // Caso: "LeyendasFiscales": [ ... ]
            return token.ToObject<List<LeyendasFiscales10>>(serializer);
        }

        // Por defecto, nulo
        return null;
    }

    public override void WriteJson(JsonWriter writer, List<LeyendasFiscales10>? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}