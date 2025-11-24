using KpacModels.Shared.Models.Comprobante.Complementos.Comercioexterior;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KPac.Domain.Mapping.JsonConverter;

public class ComercioExteriorConverter : JsonConverter<List<ComercioExterior20>>
{
    public override List<ComercioExterior20>? ReadJson(JsonReader reader, Type objectType, List<ComercioExterior20>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            // Caso: "ComercioExterior": { "ComercioExterior20": [ ... ] }
            var pagosArray = token["ComercioExterior20"] as JArray;
            if (pagosArray != null)
                return pagosArray.ToObject<List<ComercioExterior20>>(serializer);
        }
        else if (token.Type == JTokenType.Array)
        {
            // Caso: "ComercioExterior": [ ... ]
            return token.ToObject<List<ComercioExterior20>>(serializer);
        }

        // Por defecto, nulo
        return null;
    }

    public override void WriteJson(JsonWriter writer, List<ComercioExterior20>? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}