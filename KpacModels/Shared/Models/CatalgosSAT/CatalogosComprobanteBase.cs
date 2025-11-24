// using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;
//
// namespace KPac.Domain.CatalgosSAT;
//
// public class cfdi40_moneda
// {
//     [BsonId]
//     [BsonRepresentation(BsonType.ObjectId)]
//     public string Id { get; set; }
//     public string c_moneda { get; set; }
//     public string descripcion { get; set; }
//     public string decimales { get; set; }
//     public int? porcentaje_variacion { get; set; }
//     public DateTime fecha_inicio_de_vigencia { get; set; }
// }
//
// public class cfdi40_estado
// {
//     [BsonId]
//     [BsonRepresentation(BsonType.ObjectId)]
//     public string Id { get; set; }
//     public string c_estado {  get; set; }
//     public string c_pais {  get; set; }
//     public string nombre_del_estado { get; set; }
//     public DateTime fecha_inicio_de_vigencia { get; set; }
// }