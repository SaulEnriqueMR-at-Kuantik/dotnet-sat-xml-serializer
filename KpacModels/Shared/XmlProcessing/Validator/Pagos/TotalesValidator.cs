using System.Globalization;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Validator.Pagos;

public class TotalesValidator(ValidatorContext context)
{
    private ValidatorContext _context = context;

    private Totales _totales;

    private TotalesExcel _totalesCalculados = new();

    private List<RetencionP> _retenciones;

    private List<TrasladoP> _traslados;

    private string _section = string.Empty;

    private bool _isValid = true;

    public void Validate(Totales totales, List<RetencionP> retenciones, List<TrasladoP> traslados)
    {
        _section = "Comprobante -> Complemento -> Pagos -> Totales";
        _totales = totales;
        _retenciones = retenciones;
        _traslados = traslados;
        CalculateTotalesRetenciones();
        CalculateTotalesTraslados();
        if (_isValid)
        {
            var montoTotalCalculado = Math.Round(decimal.Parse(_context.GetValue("monto") ?? "0"), 2);
            CompareTotals(montoTotalCalculado);
        }
    }

    
    private void CalculateTotalesRetenciones()
    {
        foreach(var retencion in _retenciones)
        {
            var tipoImpuesto = retencion.Impuesto;
            var importe = decimal.Parse(retencion.Importe, CultureInfo.InvariantCulture);
            switch (tipoImpuesto)
            {
                case "001":
                    if (string.IsNullOrEmpty(_totales.TotalRetencionesIsr))
                    {
                        _context.AddError(
                            code: "CRP20264",
                            message:
                            "Cuando existe una retención con impuesto 001 (ISR) debera existir el atributo TotalRetencionesIsr.",
                            section: _section);
                        _isValid = false;
                        return;
                    }

                    _totalesCalculados.TotalRetencionesIsr += importe;
                    break;
                case "002":
                    if (string.IsNullOrEmpty(_totales.TotalRetencionesIva))
                    {
                        _context.AddError(
                            code: "CRP20264",
                            message:
                            "Cuando existe una retención con impuesto 002 (IVA) debera existir el atributo TotalRetencionesIva.",
                            section: _section);
                        _isValid = false;
                        return;
                    }

                    _totalesCalculados.TotalRetencionesIva += importe;
                    break;
                case "003":
                    if (string.IsNullOrEmpty(_totales.TotalRetencionesIeps))
                    {
                        _context.AddError(
                            code: "CRP20264",
                            message:
                            "Cuando existe una retención con impuesto 003 (IEPS) debera existir el atributo TotalRetencionesIeps.",
                            section: _section);
                        _isValid = false;
                        return;
                    }

                    _totalesCalculados.TotalRetencionesIeps += importe;
                    break;
            }
        }
    }

    private void CalculateTotalesTraslados()
    {
        foreach (var traslado in _traslados)
        {
            var tasaOCuota = decimal.Parse(traslado.TasaOCuota ?? "0", CultureInfo.InvariantCulture);
            var tipoFactor = traslado.TipoFactor;
            var impuesto = traslado.Impuesto;
            if (impuesto != "002") continue;
            var @base = decimal.Parse(traslado.Base, CultureInfo.InvariantCulture);
            var importe = decimal.Parse(traslado.Importe ?? "0", CultureInfo.InvariantCulture);
            if (tipoFactor == "Exento")
            {
                if (string.IsNullOrEmpty(_totales.TotalTrasladosBaseIvaExento))
                {
                    _context.AddError(
                        code: "CRP20273",
                        message:
                        "Cuando existe un traslado con impuesto 002 (IVA) y con tipoFactor Exento, debera existir el atributo TotalTrasladosBaseIvaExento.",
                        section: _section);
                    _isValid = false;
                    continue;
                }
                _totalesCalculados.TotalTrasladosBaseIvaExento += @base;
                continue;
            }
            switch (tasaOCuota)
            {
                case decimal.Zero:
                    if (string.IsNullOrEmpty(_totales.TotalTrasladosBaseIva0) ||
                        string.IsNullOrEmpty(_totales.TotalTrasladosImpuestoIva0))
                    {
                        _context.AddError(
                            code: "CRP20273",
                            message:
                            "Cuando existe un traslado con impuesto 002 (IVA) y con TasaOCuota con el valor 0 (cero), debera existir el atributo TotalTrasladosBaseIva0 y TotalTrasladosImpuestoIva0.",
                            section: _section);
                        _isValid = false;
                        continue;
                    }

                    _totalesCalculados.TotalTrasladosBaseIva0 += @base;
                    _totalesCalculados.TotalTrasladosImpuestoIva0 += importe;
                    break;
                case 0.08m:
                    if (string.IsNullOrEmpty(_totales.TotalTrasladosBaseIva8) ||
                        string.IsNullOrEmpty(_totales.TotalTrasladosImpuestoIva8))
                    {
                        _context.AddError(
                            code: "CRP20273",
                            message:
                            "Cuando existe un traslado con impuesto 002 (IVA) y con TasaOCuota con el valor 0.08 debera existir el atributo TotalTrasladosBaseIva8 y TotalTrasladosImpuestoIva8.",
                            section: _section);
                        _isValid = false;
                        continue;
                    }

                    _totalesCalculados.TotalTrasladosBaseIva8 += @base;
                    _totalesCalculados.TotalTrasladosImpuestoIva8 += importe;
                    continue;
                case 0.16m:
                    if (string.IsNullOrEmpty(_totales.TotalTrasladosBaseIva16) ||
                        string.IsNullOrEmpty(_totales.TotalTrasladosImpuestoIva16))
                    {
                        _context.AddError(
                            code: "CRP20273",
                            message:
                            "Cuando existe un traslado con impuesto 002 (IVA) y con TasaOCuota con el valor 0.16 debera existir el atributo TotalTrasladosBaseIva16 y TotalTrasladosImpuestoIva16.",
                            section: _section);
                        _isValid = false;
                        continue;
                    }

                    _totalesCalculados.TotalTrasladosBaseIva16 += @base;
                    _totalesCalculados.TotalTrasladosImpuestoIva16 += importe;
                    continue;
            }
        }

    }
    
    private void CompareTotals(decimal montoTotalCalculado)
    {
        if (!string.IsNullOrEmpty(_totales.TotalTrasladosBaseIvaExento))
        {
            var totalTrasladosBaseIvaExento = Math.Round(decimal.Parse(_totales.TotalTrasladosBaseIvaExento), 2);
            var totalTrasladoBaseIvaExentoCalculado = Math.Round(_totalesCalculados.TotalTrasladosBaseIvaExento, 2);
            if (totalTrasladosBaseIvaExento != totalTrasladoBaseIvaExentoCalculado)
            {
                _context.AddError(
                    code: "CRP20210",
                    message: "El valor del campo TotalTrasladosBaseExento no es igual al redondeo de la suma del " +
                             "resultado de multiplicar cada uno de los importes de los atributos BaseP de los impuestos" +
                             " trasladados registrados en el elemento TrasladoP donde los atributos contengan en ImpuestoP" +
                             " el valor IVA y en TipoFactorP el valor Exento, por el valor registrado en el atributo " +
                             $"TipoCambioP de cada nodo Pago. Valor registrado {totalTrasladosBaseIvaExento}. Valor esperado {totalTrasladoBaseIvaExentoCalculado}.",
                    section: _section);
            }
        }

        if (!string.IsNullOrEmpty(_totales.TotalRetencionesIva))
        {
            var totalRetencionesIva = Math.Round(decimal.Parse(_totales.TotalRetencionesIva), 2);
            var totalRetencionesIvaCalculado = Math.Round(_totalesCalculados.TotalRetencionesIva, 2);
            if (totalRetencionesIva != totalRetencionesIvaCalculado)
            {
                _context.AddError(
                    code: "CRP20201",
                    message: "El valor del campo TotalRetencionesIVA no es igual al redondeo de la suma del resultado" +
                             " de multiplicar cada uno de los importes de los atributos ImporteP de los impuestos " +
                             "retenidos registrados en el elemento RetencionP donde el atributo ImpuestoP contenga el " +
                             "valor IVA por el valor registrado en el atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalRetencionesIva}. Valor esperado {totalRetencionesIvaCalculado}.",
                    section: _section);
            }
        }

        if (!string.IsNullOrEmpty(_totales.TotalRetencionesIsr))
        {
            var totalRetencionesIsr = Math.Round(decimal.Parse(_totales.TotalRetencionesIsr), 2);
            var totalRetencionesIsrCalculado = Math.Round(_totalesCalculados.TotalRetencionesIsr, 2);
            if (totalRetencionesIsr!= totalRetencionesIsrCalculado)
            {
                _context.AddError(
                    code: "CRP20202",
                    message: "El valor del campo TotalRetencionesISR no es igual al redondeo de la suma del resultado  " +
                             "de multiplicar cada uno de los importes de los atributos ImporteP de los impuestos retenidos " +
                             "registrados en el elemento RetencionP donde el atributo ImpuestoP contenga el valor ISR por " +
                             "el valor registrado en el atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalRetencionesIsr}. Valor esperado {totalRetencionesIsrCalculado}.",
                    section: _section);
            }
        }

        if (!string.IsNullOrEmpty(_totales.TotalRetencionesIeps))
        {
            var totalRetencionesIeps = Math.Round(decimal.Parse(_totales.TotalRetencionesIeps), 2);
            var totalRetencionesIepsCalculado = Math.Round(_totalesCalculados.TotalRetencionesIeps, 2);
            if (totalRetencionesIeps != totalRetencionesIepsCalculado)
            {
                _context.AddError(
                    code: "CRP20203",
                    message: "El valor del campo TotalRetencionesIEPS no es igual al redondeo de la suma del resultado " +
                             "de multiplicar cada uno de los importes de los atributos ImporteP de los impuestos " +
                             "retenidos registrados en el elemento RetencionP donde el atributo ImpuestoP contenga el " +
                             "valor IEPS por el valor registrado en el atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalRetencionesIeps}. Valor esperado {totalRetencionesIepsCalculado}.",
                    section: _section);
            }
        }

        if (!string.IsNullOrEmpty(_totales.TotalTrasladosBaseIva0))
        {
            var totalTrasladosBaseIva0 = Math.Round(decimal.Parse(_totales.TotalTrasladosBaseIva0), 2);
            var totalTrasladosBaseIva0Calculado = Math.Round(_totalesCalculados.TotalTrasladosBaseIva0, 2);
            if (totalTrasladosBaseIva0 != totalTrasladosBaseIva0Calculado)
                _context.AddError(
                    code: "CRP20203",
                    message: "El valor del campo TotalTrasladosBaseIVA0 no es igual al redondeo de la suma del resultado de" +
                             " multiplicar cada uno de los importes de los atributos BaseP de los impuestos trasladados " +
                             "registrados en el elemento TrasladoP donde los atributos contengan en ImpuestoP el valor IVA," +
                             " en TipoFactorP el valor Tasa y en TasaOCuotaP el valor 0.000000, por el valor registrado en el " +
                             "atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalTrasladosBaseIva0}. Valor esperado {totalTrasladosBaseIva0Calculado}.",
                    section: _section);
        }
        
        if(!string.IsNullOrEmpty(_totales.TotalTrasladosImpuestoIva0))
        {
            var totalTrasladosImpuestoIva0 = Math.Round(decimal.Parse(_totales.TotalTrasladosImpuestoIva0), 2);
            var totalTrasladosImpuestoIva0Calculado = Math.Round(_totalesCalculados.TotalTrasladosImpuestoIva0, 2);
            if (totalTrasladosImpuestoIva0 != totalTrasladosImpuestoIva0Calculado)
            {
                _context.AddError(
                    code: "CRP20209",
                    message: "El valor del campo TotalTrasladosImpuestoIVA0 no es  igual al redondeo de la suma del resultado " +
                             "de multiplicar cada uno de los importes de los atributos ImporteP de los impuestos trasladados" +
                             " registrados en el elemento TrasladoP donde los atributos contengan en ImpuestoP el valor IVA," +
                             " en TipoFactorP el valor Tasa y en TasaOCuotaP el valor 0.000000, por el valor registrado en " +
                             "el atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalTrasladosImpuestoIva0}. Valor esperado {totalTrasladosImpuestoIva0Calculado}.",
                    section: _section);
            }
        }

        if (!string.IsNullOrEmpty(_totales.TotalTrasladosBaseIva8))
        {
            var totalTrasladosBaseIva8 =  Math.Round(decimal.Parse(_totales.TotalTrasladosBaseIva8), 2);
            var totalTrasladosBaseIva8Calculado = Math.Round(_totalesCalculados.TotalTrasladosBaseIva8, 2);
            if (totalTrasladosBaseIva8 != totalTrasladosBaseIva8Calculado)
            {
                _context.AddError(
                    code: "CRP20206",
                    message: "El valor del campo TotalTrasladosBaseIVA8 no es igual al redondeo de la suma del resultado" +
                             " de multiplicar cada uno de los importes de los atributos BaseP de los impuestos trasladados " +
                             "registrados en el elemento TrasladoP donde los atributos contengan en ImpuestoP el valor IVA," +
                             " en TipoFactorP el valor Tasa y en TasaOCuotaP el valor 0.080000, por el valor registrado en" +
                             " el atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalTrasladosBaseIva8}. Valor esperado {totalTrasladosBaseIva8Calculado}.",
                    section: _section);
            }
        }
            
        if(!string.IsNullOrEmpty(_totales.TotalTrasladosImpuestoIva8))
        {
            var totalTrasladosImpuestoIva8 = Math.Round(decimal.Parse(_totales.TotalTrasladosImpuestoIva8), 2);
            var totalTrasladosImpuestoIva8Calculado = Math.Round(_totalesCalculados.TotalTrasladosImpuestoIva8, 2);
            if (totalTrasladosImpuestoIva8 != totalTrasladosImpuestoIva8Calculado)
            {
                _context.AddError(
                    code: "CRP20207",
                    message: "El valor del campo TotalTrasladosImpuestoIVA8 no es igual al redondeo de la suma del resultado" +
                             " de multiplicar  cada uno de los importes de los atributos ImporteP de los impuestos trasladados" +
                             " registrados en el elemento TrasladoP donde los atributos contengan en ImpuestoP el valor IVA," +
                             " en TipoFactorP el valor Tasa y TasaOCuotaP el valor 0.080000, por el valor registrado en el " +
                             "atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalTrasladosImpuestoIva8}. Valor esperado {totalTrasladosImpuestoIva8Calculado}.",
                    section: _section);
            }
        }

        if (!string.IsNullOrEmpty(_totales.TotalTrasladosBaseIva16))
        {
            var totalTrasladosBaseIva16 = Math.Round(decimal.Parse(_totales.TotalTrasladosBaseIva16), 2);
            var totalTrasladosBaseIva16Calculado = Math.Round(_totalesCalculados.TotalTrasladosBaseIva16, 2);
            if (totalTrasladosBaseIva16 != totalTrasladosBaseIva16Calculado)
            {
                _context.AddError(
                    code: "CRP20204",
                    message: "El valor del campo TotalTrasladosBaseIVA16 no es igual al redondeo de la suma del resultado" +
                             " de multiplicar cada uno de los importes de los atributos BaseP de los impuestos trasladados " +
                             "registrados en el elemento TrasladoP donde los atributos contengan en ImpuestoP el valor IVA," +
                             " en TipoFactorP el valor Tasa y en TasaOCuotaP el valor 0.160000, por el valor registrado en" +
                             " el atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalTrasladosBaseIva16}. Valor esperado {totalTrasladosBaseIva16Calculado}.",
                    section: _section);
            }
        }
        if(!string.IsNullOrEmpty(_totales.TotalTrasladosImpuestoIva16))
        {
            var totalTrasladosImpuestoIva16 =  Math.Round(decimal.Parse(_totales.TotalTrasladosImpuestoIva16), 2);
            var totalTrasladosImpuestoIva16Calculado = Math.Round(_totalesCalculados.TotalTrasladosImpuestoIva16, 2);
            if (totalTrasladosImpuestoIva16 != totalTrasladosImpuestoIva16Calculado)
            {
                _context.AddError(
                    code: "CRP20205",
                    message: "El valor del campo TotalTrasladosImpuestoIVA16 no es  igual al redondeo de la suma del resultado" +
                             " de multiplicar cada uno de los importes de los atributos ImporteP de los impuestos trasladados" +
                             " registrados en el elemento TrasladoP donde los atributos contengan en ImpuestoP el valor IVA," +
                             " en TipoFactorP el valor Tasa y en TasaOCuotaP el valor 0.160000, por el valor registrado en el" +
                             " atributo TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {totalTrasladosImpuestoIva16}. Valor esperado {totalTrasladosImpuestoIva16Calculado}.",
                    section: _section);
            }
        }

        if (montoTotalCalculado > 0)
        {
            var montoTotal = decimal.Parse(_totales.MontoTotalPagos);
            if (montoTotalCalculado != montoTotal)
            {
                _context.AddError(
                    code: "CRP20211",
                    message: "El valor del campo MontoTotalPagos no es igual al redondeo de la suma del resultado de " +
                             "multiplicar cada uno de los atributos Monto por el valor registrado en el atributo " +
                             "TipoCambioP de cada nodo Pago. Valor " +
                             $"registrado {montoTotal}. Valor esperado {montoTotalCalculado}.",
                    section: _section);
            }
        }
    }

}