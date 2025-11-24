using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Formatter;

public class FormatContext
{
    private readonly Dictionary<string, string> _bag = new ();
    
    private readonly List<Warning> _errors = [];
    
    private readonly List<ImpuestoT> _impuestosTraslados = [];
    
    private readonly List<ImpuestoT> _impuestosRetenciones = [];

    private readonly List<string> _tipoPercepciones = [];
    
    private decimal _subtotal = decimal.Zero;

    private SettingsFormatter? _settings = new();

    public void AddImporteToSubtotal(decimal importe)
    {
        _subtotal = DecimalOperator.Suma(_subtotal, importe);
    }

    public decimal GetSubtotal() => _subtotal;
    
    public void AddValue(string key, string value)
    {
        _bag[key] = value;
    }
    
    public string? GetValue(string key)
    {
        return _bag.TryGetValue(key, out var value) ? value: null;
    }
    
    public void AddError(string section, string message, string? messageDetail = null)
    {
        _errors.Add(new Warning(section, message, messageDetail));
    }

    public List<Warning> GetErrors() => _errors;
    
    public bool HasErrors() => _errors.Count > 0;

    /// <summary>
    /// Save the Impuestos Concepto Traslados in the context to format after in the node Impuestos
    /// </summary>
    /// <param name="impuestosTraslados">List of Impuestos Traslados</param>
    public void AddImpuestosTraslados(List<ImpuestoT> impuestosTraslados)
    {
        _impuestosTraslados.AddRange(impuestosTraslados);
    }
    
    public List<ImpuestoT> GetImpuestosTraslados() => _impuestosTraslados;
    
    public void AddImpuestosRetenciones(List<ImpuestoT> impuestosRetenciones)
    {
        _impuestosRetenciones.AddRange(impuestosRetenciones);
    }
    
    public List<ImpuestoT> GetImpuestosRetenciones() => _impuestosRetenciones;

    public void Clear()
    {
        _bag.Clear();
        _errors.Clear();
        _impuestosTraslados.Clear();
        _impuestosRetenciones.Clear();
        _subtotal = decimal.Zero;
    }

    public void SetSettings(SettingsFormatter? settings)
    {
        _settings = settings;
    }
    
    public SettingsFormatter? GetSettings() => _settings;

    public void AddTipoPercepcion(string tipo)
    {
        _tipoPercepciones.Add(tipo);
    }
    
    public bool ExistPercepcion(string tipo) => _tipoPercepciones.Contains(tipo);
}