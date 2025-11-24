using KpacModels.Shared.Models.Comprobante.Complementos.Cartaporte;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KPac.Domain.Mapping.JsonConverter;

public class CartaPorteConverter : JsonConverter<List<CartaPorte31>>
{
    public override List<CartaPorte31>? ReadJson(JsonReader reader, Type objectType, List<CartaPorte31>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            // Caso: "CartaPorte": { "CartaPorte31": [ ... ] }
            var pagosArray = token["CartaPorte31"] as JArray;
            if (pagosArray != null)
                return pagosArray.ToObject<List<CartaPorte31>>(serializer);
        }
        else if (token.Type == JTokenType.Array)
        {
            // Caso: "CartaPorte": [ ... ]
            return token.ToObject<List<CartaPorte31>>(serializer);
        }

        // Por defecto, nulo
        return null;
    }

    public override void WriteJson(JsonWriter writer, List<CartaPorte31>? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}