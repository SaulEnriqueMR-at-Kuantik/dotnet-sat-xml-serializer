namespace KpacModels.Shared.XmlProcessing.Formatter.Common.Models;

public class TotalesDecimal
{
    public decimal TotalRetencionesIva { get; set; }
    public decimal TotalRetencionesIsr { get; set; }
    public decimal TotalRetencionesIeps { get; set; }
    public decimal TotalTrasladosBaseIva16 { get; set; }
    public decimal TotalTrasladosImpuestoIva16 { get; set; }
    public decimal TotalTrasladosBaseIva8 { get; set; }
    public decimal TotalTrasladosImpuestoIva8 { get; set; }
    public decimal TotalTrasladosBaseIva0 { get; set; }
    public decimal TotalTrasladosImpuestoIva0 { get; set; }
    public decimal TotalTrasladosBaseIvaExento { get; set; }

    public void Clear()
    {
        TotalRetencionesIva = decimal.Zero;
        TotalRetencionesIsr = decimal.Zero;
        TotalRetencionesIeps = decimal.Zero;
        TotalRetencionesIeps = decimal.Zero;
        TotalTrasladosBaseIva16 = decimal.Zero;
        TotalTrasladosImpuestoIva16 = decimal.Zero;
        TotalTrasladosBaseIva8 = decimal.Zero;
        TotalTrasladosImpuestoIva8 = decimal.Zero;
        TotalTrasladosBaseIva0 = decimal.Zero;
        TotalTrasladosImpuestoIva0 = decimal.Zero;
        TotalTrasladosBaseIvaExento = decimal.Zero;
    }
}