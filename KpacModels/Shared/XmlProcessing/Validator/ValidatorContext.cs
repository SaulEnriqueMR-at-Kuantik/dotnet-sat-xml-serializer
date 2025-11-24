using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Validator;

public class ValidatorContext
{
    private readonly List<Error> _errors = [];

    private readonly List<Warning> _warnings = [];

    private readonly Dictionary<string, string> _bag = new ();
    
    private readonly Dictionary<string, decimal> _bagDecimal = new ();
    
    private readonly Dictionary<string, decimal> _retenciones = new();
    
    private readonly Dictionary<string, TrasladoTotales> _traslados = new();

    private readonly List<TrasladoP> _trasladosList = [];

    public void CleanContext()
    {
        _errors.Clear();
        _warnings.Clear();
        _traslados.Clear();
        _trasladosList.Clear();
        _bag.Clear();
        _bagDecimal.Clear();
        _retenciones.Clear();
        
    }
    public void AddError(string code, string message, string section)
    {
        _errors.Add(new Error(){Code = code, Message = message, Section = section});
    }

    public List<Error> GetErrors()
    {
        return _errors;
    }

    public bool HasErrors() => _errors.Count > 0 || _warnings.Count > 0;
    
    public void AddWarning(string section, string message)
    {
        _warnings.Add(new Warning(section, message));
    }

    public void AddErrorByList(List<Error> errors)
    {
        _errors.AddRange(errors);
    }

    public List<Warning> GetWarnings()
    {
        return _warnings;
    }

    public bool HasWarnings() => _warnings.Count > 0;

    
    public void AddValue(string key, string value)
    {
        _bag[key] = value;
    }
    
    public string? GetValue(string key)
    {
        return _bag.TryGetValue(key, out var value) ? value: null;
    }

    public void RemoveValue(string key)
    {
        _bag.Remove(key);
    }
    
    public void AddRetencion(string key, decimal value)
    {
        _retenciones[key] = value;
    }
    
    public bool TryGetRetencion(string key, out decimal total)
    {
        if (_retenciones.TryGetValue(key, out var value))
        {
            total = value;
            return true;
        }
        total = 0;
        return false;
    }

    public void DeleteRetencion(string key)
    {
        _retenciones.Remove(key);
    }
    
    public int CountRetencion() => _retenciones.Count;
    
    public void AddTraslado(string key, TrasladoTotales value)
    {
        _traslados[key] = value;
    }
    
    public bool TryGetTraslado(string key, out TrasladoTotales? traslado)
    {
        if (_traslados.TryGetValue(key, out var value))
        {
            traslado = value;
            return true;
        }
        traslado = null;
        return false;
    }

    public void DeleteTraslado(string key)
    {
        _traslados.Remove(key);
    }
    
    public int CountTraslados() => _traslados.Count;
    
    
    public void SaveTraslado(TrasladoP traslado)
    {
        _trasladosList.Add(traslado);
    }
    
    public decimal? GetValueDecimal(string key)
    {
        return _bagDecimal.TryGetValue(key, out var value) ? value: null;
    }

    public void ClearTraslados()
    {
        _traslados.Clear();
    }

    public void ClearRetenciones()
    {
        _traslados.Clear();
    }

}