using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KPac.Domain.Mapping.JsonConverter;

public class DateConverter : IsoDateTimeConverter
{
    
    public DateConverter()
    {
        base.DateTimeFormat = "yyyy'-'MM'-'dd";
    }
    
}