// using KPac.Domain.CatalgosSAT;
//
namespace KpacModels.Shared.XmlProcessing.Validator.Catalogos;
//
public static class CatalogosComprobante
{
     public static readonly List<string> c_TipoDeComprobante =
     [
         "P", "T", "N", "E", "I"
     ];
     public static readonly List<string> c_FormaPago = [
         "01", "02", "03", "04", "05", "06", "07", "08", 
         "12", "13", "14", "15", "17", "23", "24", "25", 
         "26", "27", "28", "29", "30", "31", "99"];
     
     public static readonly List<string> c_Exportacion =
     [
         "01", "02", "03", "04"
     ];

     public static readonly List<string> c_MetodoPago =
     [
         "PUE", "PPD"
     ];
     
     public static readonly List<string> c_RegimenFiscal = 
     [
         "601", "603", "605", "606", "607", 
         "608", "610", "611", "612", "614", 
         "615", "616", "620", "621", "622", 
         "623", "624", "625", "626"
     ];

     public static readonly List<string> listRegimenFiscalFisica =
     [
         "605", "606", "607", "608", "610", "611", "612", "614", "615", "616", "621", "625", "626"
     ];
     
     public static readonly List<string> listRegimenFiscalMoral = 
     [
         "601", "603", "610", "620", "622", "623", "624", "626"
     ];
     
     public static readonly List<string> c_Pais = 
             [
         "AFG", "ALA", "ALB", "DEU", "AND", "AGO", "AIA", "ATA", "ATG", "SAU",
         "DZA", "ARG", "ARM", "ABW", "AUS", "AUT", "AZE", "BHS", "BGD", "BRB",
         "BHR", "BEL", "BLZ", "BEN", "BMU", "BLR", "MMR", "BOL", "BIH", "BWA",
         "BRA", "BRN", "BGR", "BFA", "BDI", "BTN", "CPV", "KHM", "CMR", "CAN",
         "QAT", "BES", "TCD", "CHL", "CHN", "CYP", "COL", "COM", "PRK", "KOR",
         "CIV", "CRI", "HRV", "CUB", "CUW", "DNK", "DMA", "ECU", "EGY", "SLV",
         "ARE", "ERI", "SVK", "SVN", "ESP", "USA", "EST", "ETH", "PHL", "FIN",
         "FJI", "FRA", "GAB", "GMB", "GEO", "GHA", "GIB", "GRD", "GRC", "GRL",
         "GLP", "GUM", "GTM", "GUF", "GGY", "GIN", "GNB", "GNQ", "GUY", "HTI",
         "HND", "HKG", "HUN", "IND", "IDN", "IRQ", "IRN", "IRL", "BVT", "IMN",
         "CXR", "NFK", "ISL", "CYM", "CCK", "COK", "FRO", "SGS", "HMD", "FLK",
         "MNP", "MHL", "PCN", "SLB", "TCA", "UMI", "VGB", "VIR", "ISR", "ITA",
         "JAM", "JPN", "JEY", "JOR", "KAZ", "KEN", "KGZ", "KIR", "KWT", "LAO",
         "LSO", "LVA", "LBN", "LBR", "LBY", "LIE", "LTU", "LUX", "MAC", "MDG",
         "MYS", "MWI", "MDV", "MLI", "MLT", "MAR", "MTQ", "MUS", "MRT", "MYT",
         "MEX", "FSM", "MDA", "MCO", "MNG", "MNE", "MSR", "MOZ", "NAM", "NRU",
         "NPL", "NIC", "NER", "NGA", "NIU", "NOR", "NCL", "NZL", "OMN", "NLD",
         "PAK", "PLW", "PSE", "PAN", "PNG", "PRY", "PER", "PYF", "POL", "PRT",
         "PRI", "GBR", "CAF", "CZE", "MKD", "COG", "COD", "DOM", "REU", "RWA",
         "ROU", "RUS", "ESH", "WSM", "ASM", "BLM", "KNA", "SMR", "MAF", "SPM",
         "VCT", "SHN", "LCA", "STP", "SEN", "SRB", "SYC", "SLE", "SGP", "SXM",
         "SYR", "SOM", "LKA", "SWZ", "ZAF", "SDN", "SSD", "SWE", "CHE", "SUR",
         "SJM", "THA", "TWN", "TZA", "TJK", "IOT", "ATF", "TLS", "TGO", "TKL",
         "TON", "TTO", "TUN", "TKM", "TUR", "TUV", "UKR", "UGA", "URY", "UZB",
         "VUT", "VAT", "VEN", "VNM", "WLF", "YEM", "DJI", "ZMB", "ZWE", "ZZZ"
     ];
     
     public static readonly List<string> c_UsoCFDI = 
     [
         "G01", "G02", "G03",
         "I01", "I02", "I03", "I04", "I05", "I06", "I07", "I08",
         "D01", "D02", "D03", "D04", "D05", "D06", "D07", "D08", "D09", "D10",
         "S01",
         "CP01",
         "CN01"
     ];
     
     public static readonly List<string> listUsoCfdiFisica = 
     [
         "G01", "G02", "G03",
         "I01", "I02", "I03", "I04", "I05", "I06", "I07", "I08",
         "D01", "D02", "D03", "D04", "D05", "D06", "D07", "D08", "D09", "D10",
         "S01",
         "CP01",
         "CN01"
     ];
     
     public static readonly List<string> listUsoCfdiExcepcionesMoral = 
     [
         "D01", "D02", "D03", "D04", "D05", "D06", "D07", "D08", "D09", "D10",
         "CN01"
     ];
     public static readonly List<string> c_Periodicidad = 
     [
         "01", "02", "03", "04", "05"
     ];
     
     public static readonly List<string> c_Meses = [
         "01", "02", "03", "04", "05", "06", 
         "07", "08", "09", "10", "11", "12",
         "13", "14", "15", "16", "17", "18"
     ];
     
     public static readonly List<string> mesesBimestrales = 
     [
         "13", "14", "15", "16", "17", "18"
     ];

     public static readonly List<string> meses =
     [
         "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"
     ];

     public static readonly List<string> c_ObjetoImp =
     [
         "01", "02", "03", "04", "05", "06", "07", "08", 
     ];
     
     public static readonly List<string> c_ObjetoImpNoImpuesto =
     [
         "01", "03", "04", "05"
     ];

     public static readonly Dictionary<string, string> c_Impuesto = new()
     {
         { "001", "ISR" },
         { "002", "IVA" },
         { "003", "IEPS" },
     };

     public static readonly List<string> c_TipoFactor =
     [
         "Tasa", "Cuota", "Exento", 
     ];

     public static readonly List<string> c_TipoRelacion =
     [
         "01", "02", "03", "04", "05", "06", "07", 
     ];

     public static readonly List<TasaOCuota> c_TasaOCuota =
     [
         new(isRango: false, valorMaximo: 0, impuesto: "IVA", factor: "Tasa", traslado: true, retencion: false),
         new(isRango: false, valorMaximo: 0.265m, impuesto: "IEPS", factor: "Tasa", traslado: true, retencion: true),
         new(isRango: false, valorMaximo: 0.5m, factor: "Tasa", impuesto: "IEPS", traslado: true, retencion: true),
         new(isRango: false, valorMaximo: 0.08m, factor: "Tasa", impuesto: "IEPS", traslado: true, retencion: true),
         new(isRango: false, valorMaximo: 0.07m, impuesto: "IEPS", factor: "Tasa", traslado: true, retencion: true),
         new(isRango: true, valorMinimo: 0, valorMaximo: 66.5062m, impuesto: "IEPS", factor: "Cuota", traslado: true, retencion: true),
         new(isRango: false, valorMaximo: 0.16m, impuesto: "IVA", factor: "Tasa", traslado: true, retencion: false),
         new(isRango: true, valorMinimo: 0, valorMaximo: 0.16m, impuesto: "IVA", factor: "Tasa", traslado: false, retencion: true),
         new(isRango: false, valorMaximo: 0.08m, impuesto: "IVA", factor: "Tasa", traslado: true, retencion: false),
         new(isRango: false, valorMaximo: 0.53m, impuesto: "IEPS", factor: "Tasa", traslado: true, retencion: true),
         new(isRango: false, valorMaximo: 0, impuesto: "IEPS", factor: "Tasa", traslado: true, retencion: false),
         new(isRango: true, valorMinimo: 0, valorMaximo: 0.35m, impuesto: "ISR", factor: "Tasa", traslado:false, retencion: true),
         new(isRango: false, valorMaximo: 0.3m, impuesto:"IEPS", factor:"Tasa", traslado:true, retencion:true),
         new(isRango: false, valorMaximo: 0.304m, impuesto: "IEPS", factor:"Tasa", traslado:true, retencion:true),
         new(isRango: false, valorMaximo:0.25m, impuesto: "IEPS", factor: "Tasa", traslado:true, retencion:true),
         new(isRango: false, valorMaximo: 0.09m, impuesto: "IEPS", factor: "Tasa", traslado:true, retencion: true),
         new(isRango: false, valorMaximo:0.06m, impuesto: "IEPS", factor: "Tasa", traslado:true, retencion: true),
         new(isRango: false, valorMaximo: 0.03m, impuesto: "IEPS", factor: "Tasa", traslado: true, retencion: false),
         new(isRango: false, valorMaximo: 1.6m, impuesto: "IEPS", factor: "Tasa", traslado:true, retencion: true),
     ];
     
     /// <summary>
     /// Lista de FormaPago que tienen patrón para cuenta ordenante
     /// </summary>
     public static readonly List<string> formaPagoListCtaOrd = [
         "02", "03", "04", "05", "06", "28", "29"];
     
     /// <summary>
     /// Lista de FormaPago que tienen patrón para cuenta beneficiario
     /// </summary>
     public static readonly List<string> formaPagoListCtaBen = [
         "02", "03", "04", "05", "28", "29"];
}