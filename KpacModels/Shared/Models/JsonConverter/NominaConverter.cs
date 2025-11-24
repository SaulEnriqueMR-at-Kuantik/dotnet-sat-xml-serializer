using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KPac.Domain.Mapping.JsonConverter;

public class NominaConverter : JsonConverter<List<Nomina12>>
{
    public override List<Nomina12>? ReadJson(JsonReader reader, Type objectType, List<Nomina12>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JObject.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            // Caso: "Nomina": { "Nomina12": [ ... ] }
            var pagosArray = token["Nomina12"] as JArray;
            if (pagosArray != null)
                return pagosArray.ToObject<List<Nomina12>>(serializer);
        }
        else if (token.Type == JTokenType.Array)
        {
            // Caso: "Nomina": [ ... ]
            return token.ToObject<List<Nomina12>>(serializer);
        }

        // Por defecto, nulo
        return null;
    }

    public override void WriteJson(JsonWriter writer, List<Nomina12>? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}