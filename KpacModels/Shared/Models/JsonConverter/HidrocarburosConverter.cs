using KpacModels.Shared.Models.Comprobante.ComplementosConcepto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KpacModels.Shared.Models.JsonConverter;

public class HidrocarburosConverter : JsonConverter<List<HidrocarburosPetroliferos>>
{
    public override void WriteJson(JsonWriter writer, List<HidrocarburosPetroliferos>? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }

    public override List<HidrocarburosPetroliferos>? ReadJson(JsonReader reader, Type objectType, List<HidrocarburosPetroliferos>? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            // Caso: "ComercioExterior": { "HidroCarburosPetroliferos10": [ ... ] }
            var pagosArray = token["HidroCarburosPetroliferos10"] as JArray;
            if (pagosArray != null)
                return pagosArray.ToObject<List<HidrocarburosPetroliferos>>(serializer);
        }
        else if (token.Type == JTokenType.Array)
        {
            // Caso: "ComercioExterior": [ ... ]
            return token.ToObject<List<HidrocarburosPetroliferos>>(serializer);
        }

        // Por defecto, nulo
        return null;
    }
}