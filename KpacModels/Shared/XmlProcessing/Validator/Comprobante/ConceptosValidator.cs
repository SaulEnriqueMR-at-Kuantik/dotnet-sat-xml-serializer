using KPac.Application.Validator;
using KPac.Domain.CatalgosSAT;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Validator.Catalogos;
using KpacModels.Shared.XmlProcessing.Validator.Comprobante.ConceptoValidate;
using CuentaPredial = KpacModels.Shared.Models.Comprobante.CuentaPredial;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante;

public class ConceptosValidator
{
    private string _section;
    //private RepositoryValidator<cfdi40_claveprodserv> _repositoryClaveProdServ;
    //private RepositoryValidator<cfdi40_claveunidad> _repositoryClaveUnidad;
    private ValidatorContext _context;
    private int _numConcepto;
    private decimal _cantidad = decimal.Zero;
    private decimal _valorUnitario = decimal.Zero;
    private decimal _importe = decimal.Zero;
    private int _decimales = 0;
    //private ClientValidator _clientValidator;

    public ConceptosValidator(int numConcepto, ValidatorContext context )
        //ClientValidator clientValidator, int numConcepto, ValidatorContext context)
    {
        //_repositoryClaveProdServ = new RepositoryValidator<cfdi40_claveprodserv>(clientValidator);
        //_repositoryClaveUnidad = new RepositoryValidator<cfdi40_claveunidad>(clientValidator);
        _numConcepto = numConcepto;
        _context = context;
        //_clientValidator = clientValidator;
        _section = $"Comprobante -> {_numConcepto}.-Concepto";
        _decimales = int.Parse(_context.GetValue("monedaDecimales") ?? "2");
    }
    public async Task Validate(Concepto concepto)
    {
        await ValidateClaveProdServ(concepto.ClaveProdServ);
        ValidateNoIdentificacion(concepto.NoIdentificacion);
        ValidateCantidad(concepto.Cantidad);
        await ValidateClaveUnidad(concepto.ClaveUnidad, concepto.Unidad);
        ValidateDescripcion(concepto.Descripcion);
        ValidateValorUnitario(concepto.ValorUnitario);
        ValidateImporte(concepto.Importe);
        ValidateDescuento(concepto.Descuento);
        ValidateObjetoImpuesto(concepto.ObjetoImpuesto, concepto.Impuestos);
        await ValidateImpuestosConcepto(concepto.Impuestos);
        ValidateCuentaTerceros(concepto.ACuentaTerceros);
        ValidateInformacionAduanera(concepto.InformacionAduanera);
        ValidateCuentaPredial(concepto.CuentaPredial);
        await ValidatePartes(concepto.Parte);
    }

    private async Task ValidateClaveProdServ(string claveProdServ)
    {
        // if (!await _repositoryClaveProdServ.ExistsAsync("c_claveprodserv", claveProdServ))
        // {
        //     _context.AddError(
        //         code: "CFDI40162",
        //         section: _section,
        //         message: "El campo ClaveProdServ, no contiene un valor del catálogo c_ClaveProdServ.");
        //     return;
        // }
    }

    private void ValidateNoIdentificacion(string? noIdentificacion)
    {
        // > **Descripción:** Atributo **OPCIONAL**, registra el número de parte, identificador del producto o del
        // servicio, la clave de producto o servicio, SKU o equivalente, **PUEDE** estar conformado de **1 a 100
        // caracteres alfanuméricos**.
        // > **Valor:** "UT421510" | ...
    }

    private void ValidateCantidad(string cantidad)
    {
        if (!decimal.TryParse(cantidad, out var cantidadDecimal))
        {
            _context.AddWarning(
                section: _section,
                message: $"El campo Cantidad no contiene un valor valido, debe ser un numero con hasta 6 decimales. " +
                         $"Valor registrado: {cantidad}");
        }

        if (cantidadDecimal <= 0)
        {
            _context.AddWarning(
                section: _section,
                message: $"El campo Cantidad no debe contener valores negativos o 0. Valor minimo valido 0.000001, " +
                         $"valor registrado: {cantidad}.");
        }
        _cantidad = cantidadDecimal;
    }

    private async Task ValidateClaveUnidad(string claveUnidadString, string? unidad)
    {
        // var claveUnidad = await _repositoryClaveUnidad.GetAsync("c_claveunidad", claveUnidadString);
        // if (claveUnidad == null)
        // {
        //     _context.AddError(
        //         code: "CFDI40165",
        //         section: _section,
        //         message: "El campo ClaveUnidad no contiene un valor del catálogo c_ClaveUnidad.");
        //     return;
        // }
        // TODO
        //  if (!string.IsNullOrEmpty(unidad) && claveUnidad.nombre != unidad)
        //  {
        //     _context.AddWarning(
        //         section: _section,
        //         message: $"El campo Unidad no concuerda con en nombre del catalogo c_ClaveUnidad, Valor Registrado " +
        //                  $"{unidad}, valor esperado {claveUnidad.nombre}.");
        //  }
    }

    private void ValidateDescripcion(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
        {
            _context.AddWarning(
                section: _section,
                message: "El campo Descripcion no puede ser nulo o vacio.");
            return;
        }

        if (descripcion.Length > 1000)
        {
            _context.AddWarning(
                section: _section,
                message: "El campo Descripcion no puede tener más de 1000 caracteres.");
            return;
        }
    }

    private void ValidateValorUnitario(string valorUnitario)
    {
        var tipoComprobante = _context.GetValue("tipoComprobante");
        if(tipoComprobante == null) return;
        if (!decimal.TryParse(valorUnitario, out var valorUnitarioDecimal))
        {
            _context.AddWarning(
                section: _section,
                message: $"El campo ValorUnitario no contiene un valor valido, debe ser un numero con hasta 6 decimales. Valor registrado: {valorUnitario}.");
            return;
        }
        _valorUnitario = valorUnitarioDecimal;
        // SI el Atributo TipoDeComprobante registra el valor I, E o N este Atributo DEBE registrar un valor mayor a cero.
        if (tipoComprobante is "I" or "E" or "N" && valorUnitarioDecimal <= 0)
        {
            _context.AddError(
                section: _section,
                code: "CFDI40166",
                message: "El valor del campo ValorUnitario debe ser mayor que cero (0) cuando el tipo de " +
                         "comprobante es Ingreso, Egreso o Nomina.");
            return;
        }
        // SI el Atributo TipoDeComprobante registra el valor T este Atributo PUEDE registrar un valor mayor o igual a cero.
        if (tipoComprobante is "T" && valorUnitarioDecimal < 0)
        {
            _context.AddWarning(
                section: _section,
                message: "El valor del campo ValorUnitario debe registrar un valor mayor o igual a cero (0) cuando el tipo de " +
                         "comprobante es Traslado.");
            return;
        }
        
        // SI el Atributo TipoDeComprobante registra el valor P este Atributo DEBE ser igual a cero.
        if (tipoComprobante is "P" && valorUnitarioDecimal != decimal.Zero)
        {
            _context.AddWarning(
                section: _section,
                message: "El valor del campo ValorUnitario debe registrar un valor de cero (0) cuando el tipo de " +
                         "comprobante es Pago.");
            return;
        }
    }

    private void ValidateImporte(string importeString)
    {
        _importe = decimal.Parse(importeString);
        
        var limiteInferior = DecimalOperatorLimites.CalcularLimiteInferiorConcepto(cantidad: _cantidad,
            valorUnitario: _valorUnitario, decimalesMoneda: _decimales);
        var limiteSuperior = DecimalOperatorLimites.CalcularLimiteSuperiorConcepto(cantidad: _cantidad,
            valorUnitario: _valorUnitario, decimalesMoneda: _decimales);
        
        // El valor de importe debe ser mayor o igual que el límite inferior y menor o igual que el límite superior.
        if (_importe < limiteInferior || _importe > limiteSuperior)
        {
            _context.AddError(
                code: "CFDI40167", 
                section: _section, 
                message: $"El valor del campo Importe no se encuentra entre el limite inferior y superior permitido." +
                         $" Limite inferior: {limiteInferior}. Limite superior: {limiteSuperior}. Valor ingresado: {_importe}.");
            return;
        }
    }

    private void ValidateDescuento(string? descuentoString)
    {
        if(string.IsNullOrWhiteSpace(descuentoString)) return;
        var descuento = decimal.Parse(descuentoString);
        if(descuento > _importe)
        {
            _context.AddError(
                code: "CFDI40169",
                section: _section,
                message: $"El campo Descuento debe ser menor o igual al atributo Importe. Descuento: {descuento}. " +
                         $"Importe: {_importe}.");
            return;
        }
        var decimalPlacesDecuento = ValidateHelper.CountDecimalPlaces(descuento);
        var decimalPlacesImporte = ValidateHelper.CountDecimalPlaces(_importe);
        if (decimalPlacesDecuento > decimalPlacesImporte)
        {
            _context.AddError(
                code: "CFDI40168", 
                section: _section, 
                message: $"El valor del campo Descuento no contiene la misma cantidad de decimales registrados en" +
                         $" el campo  Importe del concepto. Descuento tiene {decimalPlacesDecuento} decimales, valor registrado: {descuento}." +
                         $" Importe tiene {decimalPlacesImporte} decimales, valor registrado: {_importe}");
            return;
        }
    }

    private void ValidateObjetoImpuesto(string objetoImpuesto, ConceptoImpuestos? conceptoImpuestos)
    {
        if (!CatalogosComprobante.c_ObjetoImp.Contains(objetoImpuesto))
        {
            _context.AddError(
                code: "CFDI40170",
                section: _section,
                message: $"El campo ObjetoImp, no contiene un valor del catálogo c_ObjetoImp. Valor registrado: {objetoImpuesto}."
                );
            return;
        }

        if (objetoImpuesto == "02" && conceptoImpuestos == null)
        {
            _context.AddError(
                code: "CFDI40171",
                section: _section,
                message: "Si el atributo ObjetoImp contiene el valor '02' el nodo hijo Impuestos del nodo concepto debe existir.");
        }
        if (CatalogosComprobante.c_ObjetoImpNoImpuesto.Contains(objetoImpuesto) && conceptoImpuestos != null)
        {
            _context.AddError(
                code: "CFDI40172",
                section: _section,
                message: "Si el atributo ObjetoImp contiene el valor '01', '03', '04' o '05', el nodo " +
                         "hijo Impuestos del nodo Concepto no debe existir.");
            return;
        }
        
        // Si el atributo tiene el valor "06" o "08", Impuestos debe tener al menos una retention con el valor "001" en el atributo impuesto
        if (objetoImpuesto is "06" or "08")
        {
            if (conceptoImpuestos == null)
                return;
            
            var retenciones = conceptoImpuestos.Retenciones;
            var traslados = conceptoImpuestos.Traslados;
            
            if ((retenciones == null || retenciones.Count == 0) && (traslados == null || traslados.Count == 0))
                return;
            
            var isValid = false;
            foreach (var retencion in retenciones)
            {
                if(retencion.Impuesto == "001")
                    isValid = true;
                else
                {
                    isValid = false;
                    break;
                }
            }
            
            
            if(traslados != null && traslados?.Count > 0)
                isValid = false;
            
            if(!isValid)
                _context.AddError(
                    code: "CFDI140226",
                    message: "Si el atributo ObjetoImpuesto contiene el valor '06' o '08' no debe existir el nodo Traslados, puede existir el nodo Retenciones con el valor '001' en el atributo Impuesto",
                    section: _section);
        }
        // Si este atributo contiene el valor “07", en el nodo hijo Impuestos del nodo Concepto no deben existir los nodos
        // hijo “Retencion” y “Traslado” con el atributo “Impuesto” con el valor "002"; puede existir el nodo “Retenciones”,
        // con al menos un nodo hijo “Retencion” con el valor "001" en el atributo “Impuesto”; debe existir el nodo hijo
        // “Traslado” con el valor  "003" en el atributo “Impuesto” y puede existir el nodo hijo "Retencion" con el
        // valor "003" en el atributo "Impuesto" .
        if (objetoImpuesto is "07")
        {
            
            if (conceptoImpuestos == null)
            {
                _context.AddError(
                    code: "CFDI140227",
                    message: "Cuando se registre  el valor '07' en el campo ObjetoImpuesto, en el nodo hijo Impuestos del " +
                             "nodo Concepto debe existir al menos un nodo hijo Traslado con el valor '003' (IEPS).",
                    section: _section);
                return;
            }
            var retenciones = conceptoImpuestos.Retenciones;
            var traslados = conceptoImpuestos.Traslados;
            
            // En traslados debe existir al menos un hijo con el valor 003 
            if (traslados == null || traslados.Count == 0)
            {
                _context.AddError(
                    code: "CFDI140227",
                    message: "Cuando se registre  el valor '07' en el campo ObjetoImpuesto, en el nodo hijo Impuestos del " +
                             "nodo Concepto debe existir al menos un nodo hijo Traslado con el valor '003' (IEPS).",
                    section: _section);
                return;
            }
            var isValid = false;
            // Validar Retenciones
            if (retenciones != null || retenciones?.Count > 0)
            {
                // No deben existir retenciones con el valor 002
                foreach (var retencion in retenciones)
                {
                    if (retencion.Impuesto == "002")
                    {
                        _context.AddError(
                            code: "CFDI140227",
                            message: "Cuando se registre  el valor '07' en el campo ObjetoImpuesto, en el nodo hijo Impuestos " +
                                     "del nodo Concepto no deben existir los nodos hijo Retencion con el valor '002' en el atributo Impuesto.",
                            section: _section);
                        return;
                    }
                    // Debe existir al menos un nodo hijo Retencion con el valor "001" en el atributo ImpuestoDR
                    if (retencion.Impuesto is "001" or "003")
                    {
                        isValid = true;
                    }
                    
                }
                // Si no encontro ninguna Retencion con el valor "001" o "003" en el atributo ImpuestoDR agregar error
                if (!isValid)
                {
                    _context.AddError(
                        code: "CFDI140227",
                        message: "Cuando se registre  el valor '07' en el campo ObjetoImpuesto, en el nodo hijo Impuestos " +
                                 "del nodo Concepto el nodo Retenciones debe tener al menos un nodo hijo Retencion con el valor '001' o '003' en el atributo Impuesto",
                        section: _section);
                    return;
                }
            }
            isValid = false;
            // No deben existir traslados con el valor 002 y debe existir al menos un traslado con el valor 003
            foreach (var traslado in traslados)
            {
                if (traslado.Impuesto == "002")
                {
                    _context.AddError(
                        code: "CRP20279",
                        message: "Cuando se registre  el valor '07' en el campo ObjetoImpuesto, en el nodo hijo Impuestos" +
                                 " del nodo Concepto no deben existir los nodos hijo Traslado con el valor '002' en el atributo Impuesto.",
                        section: _section);
                    return;
                }
                if(traslado.Impuesto == "003")
                    isValid = true;
                
            }
            if (!isValid)
            {
                _context.AddError(
                    code: "CRP20279",
                    message: "Cuando se registre  el valor '07' en el campo ObjetoImpuesto, en el nodo hijo Impuestos" +
                             " del nodo Concepto debe existir al menos un nodo hijo Traslado con el valor '003' (IEPS).",
                    section: _section);
                return;
            }
        }
    }
    
    public async Task ValidateImpuestosConcepto(ConceptoImpuestos? impuestos)
    {
        if(impuestos == null) return;
        //var validador = new ImpuestoConcepto(_context, _numConcepto, _clientValidator);
        //await validador.Validate(impuestos);
    }

    private void ValidateCuentaTerceros(ACuentaTerceros? cuentaTerceros)
    {
        if(cuentaTerceros == null) return;
        //var validador = new ACuentaTercerosConcepto(_context, _numConcepto, _clientValidator);
        //validador.Validate(cuentaTerceros);
    }

    private void ValidateInformacionAduanera(List<InformacionAduanera>? informacionAduanera)
    {
        if(informacionAduanera == null) return;
        var validador = new InformacionAduaneraConcepto(_context, _numConcepto);
        validador.Validate(informacionAduanera);
    }
    
    private void ValidateCuentaPredial(List<CuentaPredial>? conceptoCuentasPredial)
    {
        if(conceptoCuentasPredial == null || conceptoCuentasPredial.Count == 0) return;
        var validator = new CuentaPredialValidator(_context, _numConcepto);
        validator.Validate(conceptoCuentasPredial);
    }

    private async Task ValidatePartes(List<Parte>? conceptoPartes)
    {
        if(conceptoPartes == null || conceptoPartes.Count == 0) return;
        // var validator = new ParteValidate(_context, _numConcepto, _clientValidator);
        // var count = conceptoPartes.Count;
        // for (int i = 0; i < count; i++)
        // {
        //     var parte = conceptoPartes[i];
        //     await validator.Validate(parte: parte, noParte: i + 1);
        // }
    }
    
}