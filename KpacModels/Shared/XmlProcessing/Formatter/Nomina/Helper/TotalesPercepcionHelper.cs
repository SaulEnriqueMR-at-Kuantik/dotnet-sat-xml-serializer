using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;

namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina.Helper;

public class TotalesPercepcionHelper
{
    /// <summary>
    /// 022, 023, 025, 039, 044
    /// </summary>
    private decimal _totalSueldos = decimal.Zero;
    /// <summary>
    /// 022, 023, 025
    /// </summary>
    private decimal _totalSeparacionIndemnizacion = decimal.Zero;
    /// <summary>
    /// 039, 044
    /// </summary>
    private decimal _totalJubilacionPensionRetiro = decimal.Zero;
    /// <summary>
    /// Suma de los atributos ImporteGravado de los nodos Percepcion
    /// </summary>
    private decimal _totalGravado = decimal.Zero;
    /// <summary>
    /// Suma de los atributos ImporteExento de los nodos Percepcion
    /// </summary>
    private decimal _totalExento = decimal.Zero;


    
    public void AddPercepcion(Percepcion percepcion)
    {
        var importeGravado = decimal.Parse(percepcion.ImporteGravado);
        var importeExento = decimal.Parse(percepcion.ImporteExento);
        
        var tiposNoValidosTotalSueldos = new[] { "022", "023", "025", "039", "044" };
        if (!tiposNoValidosTotalSueldos.Contains(percepcion.Tipo))
        {
            // El valor del atributo Nomina.Percepciones.TotalSueldos, debe ser igual a la suma de los atributos
            // ImporteGravado e ImporteExento donde la clave expresada en el atributo TipoPercepcion sea distinta
            // de "022", "023", "025", "039" y "044"
            var total = importeGravado + importeExento;
            _totalSueldos +=  total;
        }

        if (percepcion.Tipo is "022" or "023" or "025")
        {
            // El valor del atributo Nomina.Percepciones.TotalSeparacionIndemnizacion, debe ser igual a la suma de los
            // atributos ImporteGravado e ImporteExento donde la clave expresada en el atributo TipoPercepcion sea igual
            // a "022", "023" o "025"
            var total = importeGravado + importeExento;
            _totalSeparacionIndemnizacion +=  total;
        }

        if (percepcion.Tipo is "039" or "044")
        {
            // El valor del atributo Nomina.Percepciones.TotalJubilacionPensionRetiro, debe ser igual a la suma de los
            // atributos ImporteGravado e importeExento donde la clave expresada en el atributo TipoPercepcion sea
            // igual a "039" o "044"
            var total = importeGravado + importeExento;
            _totalJubilacionPensionRetiro +=  total;
        }
        // El valor del atributo Nomina.Percepciones.TotalGravado, debe ser igual a la suma de los atributos
        // ImporteGravado de los nodos Percepcion
        _totalGravado += importeGravado;
        
        // El valor del atributo Nomina.Percepciones.TotalExento, debe ser igual a la suma de los atributos
        // ImporteExento de los nodos Percepcion
        _totalExento += importeExento;
    }

    public decimal GetTotalSueldos()
    {
        return _totalSueldos;
    }

    public decimal GetTotalSeparacionIndemnizacion()
    {
        return _totalSeparacionIndemnizacion;
    }

    public decimal GetTotalJubilacion()
    {
        return _totalJubilacionPensionRetiro;
    }

    public decimal GetTotalExento()
    {
        return _totalExento;
    }

    public decimal GetTotalGravado()
    {
        return _totalGravado;
    }
}