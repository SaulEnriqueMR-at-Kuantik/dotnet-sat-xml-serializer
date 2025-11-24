using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KPac.Domain.Mapping.JsonConverter;

public class PagosConverter : JsonConverter<List<Pagos20>>
{
    public override List<Pagos20>? ReadJson(JsonReader reader, Type objectType, List<Pagos20>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            // Caso: "Pagos": { "Pagos20": [ ... ] }
            var pagosArray = token["Pagos20"] as JArray;
            if (pagosArray != null)
                return pagosArray.ToObject<List<Pagos20>>(serializer);
        }
        else if (token.Type == JTokenType.Array)
        {
            // Caso: "Pagos": [ ... ]
            return token.ToObject<List<Pagos20>>(serializer);
        }

        // Por defecto, nulo
        return null;
    }

    public override void WriteJson(JsonWriter writer, List<Pagos20>? value, JsonSerializer serializer)
    {
        // Siempre serializa como array directo sin envolver en objeto "Pagos20"
        serializer.Serialize(writer, value);
    }
}
