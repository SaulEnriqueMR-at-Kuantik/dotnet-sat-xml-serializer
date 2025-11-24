// using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;
//
// namespace KPac.Domain.CatalgosSAT;
//
// public class cfdi40_claveprodserv
// {
//         [BsonId]
//         [BsonRepresentation(BsonType.ObjectId)]
//         public string Id { get; set; }
//         public string c_claveprodserv { get; set; }
//         public string descripcion { get; set; }
//         public string incluir_iva_trasladado { get; set; }
//         public string incluir_ieps_trasladado { get; set; }
//         public DateTime fechainiciovigencia { get; set; }
//         public int estimulo_franja_fronteriza { get; set; }
//         public string palabras_similares { get; set; }
// }
//
// public class cfdi40_claveunidad
// {
//         [BsonId]
//         [BsonRepresentation(BsonType.ObjectId)]
//         public string Id { get; set; }
//         public string c_claveunidad { get; set; }
//         public string nombre { get; set; }
//         public object descripcion { get; set; }
//         public string nota { get; set; }
//         public DateTime fecha_de_inicio_de_vigencia { get; set; }
//         public object simbolo { get; set; }
// }
//
// public class cfdi40_tasaocuota
// {
//         [BsonId]
//         [BsonRepresentation(BsonType.ObjectId)]
//         public string Id { get; set; }
//         public string rango_o_fijo { get; set; }
//         public decimal? valor_minimo { get; set; }
//         public decimal valor_maximo { get; set; }
//         public string impuesto { get; set; }
//         public string factor { get; set; }
//         public string traslado { get; set; }
//         public string retencion { get; set; }
//         public DateTime fecha_inicio_de_vigencia { get; set; }
// }
//
public class TasaOCuota
{
        public TasaOCuota(bool isRango, decimal valorMaximo, string impuesto, string factor,
                bool traslado, bool retencion, decimal? valorMinimo = null)
        {
                IsRango = isRango;
                ValorMinimo = valorMinimo;
                ValorMaximo = valorMaximo;
                Impuesto = impuesto;
                Factor = factor;
                Traslado = traslado;
                Retencion = retencion;
        }
        
        /// <summary>
        /// Determina si es de tipo Fijo (True) o Rango (False)
        /// </summary>
        public bool IsRango { get; set; }
        /// <summary>
        /// Valor mínimo de la tasa (Opcional)
        /// </summary>
        public decimal? ValorMinimo { get; set; }
        /// <summary>
        /// Valor máximo de la tasa
        /// </summary>
        public decimal ValorMaximo { get; set; }
        /// <summary>
        /// Tipo de impuesto
        /// </summary>
        public string Impuesto { get; set; }
        /// <summary>
        /// Tipo factor
        /// </summary>
        public string Factor { get; set; }
        /// <summary>
        /// Nos dice si es compatible con Impuestos Traslado 
        /// </summary>
        public bool Traslado { get; set; }
        /// <summary>
        /// Nos dice si es compatible con Impuestos Retención 
        /// </summary>
        public bool Retencion { get; set; }
}