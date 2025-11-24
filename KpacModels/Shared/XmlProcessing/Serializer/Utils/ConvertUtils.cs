using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace KpacModels.Shared.XmlProcessing.Serializer.Utils;

/// <summary>
/// Clase para funciones que utilizamos para serializar Json a Objetos // Json a XML // Objetos a Xml.
/// </summary>
public class ConvertUtils

{
    /// <summary>
    /// Convierte la fecha a formato ISO cuando se serializa de Json a Object.
    /// </summary>
    /// <returns>Fecha en formato ISO.</returns>
    public class DateFormatConverter : Newtonsoft.Json.JsonConverter
    {
        private const string IsoDateFormat = "yyyy-MM-ddTHH:mm:ss"; // ISO 8601 format

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Check for null or empty value
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
            {
                return DateTime.Now.ToString(IsoDateFormat); // return the current date time
            }
            DateTime date = (DateTime)reader.Value;

            // Parse and convert to ISO format
            string isoDate = date.ToString(IsoDateFormat);
            return isoDate;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }

    /// <summary>
    /// Valida que el valor a convertir tenga la cantidad de decimales requeridos, y los agrega si no lo cumple.
    /// </summary>
    /// <param name="NoDecimales"> Numero de decimales que necesita para que sea valido.</param>
    /// <returns>Valor con los decimales necesarios.</returns>
    public class DecimalValidator : Newtonsoft.Json.JsonConverter
    { 
        private int _no_decimales;
        public DecimalValidator(int decimales)
        {
            _no_decimales = decimales;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(float);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Check for null or empty value
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
            {
                return ""; // return empty value
            }
            // Definir el regex de acuerdo al numero de decimales que entra como parametro
            string pattern = $@"^\d+\.\d{{{{{_no_decimales}$";
            Regex regex = new Regex(pattern);

            // Si el string matchea con el regex se regresa
            if (regex.IsMatch(reader.Value.ToString()))
            {
                return reader.Value;
            }

            // Si no tiene decimales o tiene menos de 3, corregimos
            if (reader.Value.ToString().Contains("."))
            {
                // Separar la parte entera de la parte decimal
                string[] parts = reader.Value.ToString().Split('.');

                // Si la parte decimal tiene menos de 3 dígitos, rellenamos con ceros
                string decimals = parts[1].PadRight(_no_decimales, '0');

                // Retornar la parte entera con los decimales corregidos
                return parts[0] + "." + decimals;
            }
            else
            {
                // Si no hay decimales, agregamos ".000"
                return reader.Value + ".000";
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
    
    public static string AddSchemaLocation(string xml, string schemaLocations)
    {
        // Cargar el XML generado en un XDocument para manipularlo
        var doc = XDocument.Parse(xml);

        // Agregar el schemaLocation al root
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance"; // Namespace xsi
        doc.Root.SetAttributeValue(xsi + "schemaLocation", schemaLocations);

        // Retornar el XML como string
        return doc.ToString();
    }
    
}