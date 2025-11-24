using System.Globalization;
using KPac.Application.Validator;

namespace KpacModels.Shared.XmlProcessing.Validator.Pagos;


/// <summary>
/// Este helper sirve para ir sumando los límites de atributo ImpPagado del nodo DocumentosRelacionados, guardarlos para
/// realizar la validación con el campo Monto del nodo Pago
/// </summary>
public class MontoHelper
{
    private readonly ValidatorContext _context;
    
    public MontoHelper(ValidatorContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Sumar el límite inferior del campo Importe Pagado 
    /// </summary>
    /// <param name="impPagadoLimiteInferior"></param>
    public void AddImpPagadoLimiteInferior(decimal impPagadoLimiteInferior)
    {
        var impPagadoTotal = decimal.Parse(_context.GetValue("limiteInferior") ?? "0");
        impPagadoTotal += impPagadoLimiteInferior;
        _context.AddValue("limiteInferior", impPagadoTotal.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Obtener el límite inferior de la suma de Importe Pagado
    /// </summary>
    /// <returns></returns>
    public decimal GetImpPagadoLimiteInferiorMonto()
    {
        return decimal.Parse(_context.GetValue("limiteInferior") ?? "0");
    }

    /// <summary>
    /// Sumar el límite superior del campo Importe Pagado 
    /// </summary>
    /// <param name="impPagadoLimiteSuperior"></param>
    public void AddImpPagadoLimiteSuperior(decimal impPagadoLimiteSuperior)
    {
        var impPagadoTotal = decimal.Parse(_context.GetValue("limiteSuperior") ?? "0");
        impPagadoTotal += impPagadoLimiteSuperior;
        _context.AddValue("limiteSuperior", impPagadoTotal.ToString(CultureInfo.InvariantCulture));
    }

    
    /// <summary>
    /// Obtener el límite superior de la suma de Importe Pagado
    /// </summary>
    /// <returns></returns>
    public decimal GetImpPagadoSuperiorMonto()
    {
        return decimal.Parse(_context.GetValue("limiteSuperior") ?? "0");
    }

    
    /// <summary>
    /// Limpiar las llaves "limiteInferior" y "limiteSuperior" del contexto
    /// </summary>
    public void CleanLimites()
    {
        _context.RemoveValue("limiteInferior");
        _context.RemoveValue("limiteSuperior");
    }

    /// <summary>
    /// Agrega un Monto del nodo Pago al contexto para realizar la comparación con el atributo MontoTotalPagos de nodo Totales.
    /// </summary>
    /// <param name="tipoCambio">Tipo cambio de Pago</param>
    /// <param name="monto">Monto de Pago</param>
    public void AddMontoPagado(decimal tipoCambio, decimal monto)
    {
        var montoConvertido = tipoCambio * monto;
        var montoContextString = _context.GetValue("monto");
        if (montoContextString != null)
        {
            var montoContext = decimal.Parse(montoContextString);
            _context.AddValue("monto", (montoContext + montoConvertido).ToString(CultureInfo.InvariantCulture));
            return;
        }
        _context.AddValue("monto", montoConvertido.ToString(CultureInfo.InvariantCulture));
    }
}