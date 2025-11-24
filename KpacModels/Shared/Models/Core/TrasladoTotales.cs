using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

namespace KpacModels.Shared.Models.Core;


/// <summary>
/// Clase para guardar los totales de base e importe
/// </summary>
public class TrasladoTotales
{

    public TrasladoTotales(ImpuestoT traslado)
    {
        if (!string.IsNullOrEmpty(traslado.Base))
        {
            var baseTotal = decimal.Parse(traslado.Base);
            BaseTotal = baseTotal;
        }else
            BaseTotal = decimal.Zero;
        
        if (!string.IsNullOrEmpty(traslado.Importe))
        {
            var importeTotal = decimal.Parse(traslado.Importe);
            ImporteTotal = importeTotal;
        }else
            ImporteTotal = decimal.Zero;
    }
    
    public TrasladoTotales(TrasladoDR traslado)
    {
        if (!string.IsNullOrEmpty(traslado.Base))
        {
            var baseTotal = decimal.Parse(traslado.Base);
            BaseTotal = baseTotal;
        }else
            BaseTotal = decimal.Zero;
        
        if (!string.IsNullOrEmpty(traslado.Importe))
        {
            var importeTotal = decimal.Parse(traslado.Importe);
            ImporteTotal = importeTotal;
        }else
            ImporteTotal = decimal.Zero;
    }
    
    /// <summary>
    /// Base Total
    /// </summary>
    public decimal BaseTotal { get; set; }
    
    /// <summary>
    /// Importe Total
    /// </summary>
    public decimal ImporteTotal { get; set; }

    
    /// <summary>
    /// Devolver el redondeo de la suma de bases, se debe redondear hasta la cantidad de decimales que soporte la moneda
    /// </summary>
    /// <param name="decimalesMoneda"></param>
    /// <returns></returns>
    public decimal GetBaseTotal(int decimalesMoneda)
    {
        return Math.Round(BaseTotal, decimalesMoneda);
    }
    
    public decimal GetBaseTotal()
    {
        return BaseTotal;
    }
    
    /// <summary>
    /// Devolver el redondeo de la suma de importes, se debe redondear hasta la cantidad de decimales que soporte la moneda
    /// </summary>
    /// <param name="decimalesMoneda"></param>
    /// <returns></returns>
    public decimal GetImporteTotal(int decimalesMoneda)
    {
        return Math.Round(ImporteTotal, decimalesMoneda);
    }
    
    public decimal GetImporteTotal()
    {
        return ImporteTotal;
    }
    
}