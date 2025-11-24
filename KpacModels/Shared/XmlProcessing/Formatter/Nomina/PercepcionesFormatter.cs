using KPac.Application.Validator.Catalogos;
using KpacModels.Shared.Models.Comprobante.Complementos.Nomina;
using KpacModels.Shared.XmlProcessing.Formatter;
using KpacModels.Shared.XmlProcessing.Formatter.Nomina.Helper;

namespace KPac.Application.Formatter.Nomina;

public class PercepcionesFormatter
{
    private TotalesPercepcionHelper _helper;

    private readonly FormatContext _context;

    private static readonly string _section = "Comprobante -> Complemento -> Nomina -> Percepciones -> {0}. Percepción";
    
    private static readonly string _sectionAcciones = "Comprobante -> Complemento -> Nomina -> Percepciones -> {0}. Percepción -> Acciones o titulos";
    
    private static readonly string _sectionHoras = "Comprobante -> Complemento -> Nomina -> Percepciones -> {0}. Percepción -> {1}. Horas extra";
    
    private static readonly string _sectionJubilacion = "Comprobante -> Complemento -> Nomina -> Percepciones -> JubilacionPensionRetiro";
    public PercepcionesFormatter(FormatContext context)
    {
        _helper = new TotalesPercepcionHelper();
        _context = context;
    }
    
    public void Format(Percepciones percepciones)
    {
        var count = percepciones.Percepcion?.Count;
        for (var i = 0; i < count; i++)
        {
            var percepcion = percepciones.Percepcion?[i];
            if (percepcion == null)
                continue;
            FormatPercepcion(percepcion, i +1);
            _helper.AddPercepcion(percepcion);
        }

        SetTotales(percepciones);
        FormatJubilacionPensionRetiro(percepciones.JubilacionPensionRetiro, percepciones.TotalJubilacionPensionRetiro);
        
        FormatSeparacionIndemnizacion(percepciones.SeparacionIndemnizacion, percepciones.TotalSeparacionIndemnizacion);
    }

    private void FormatSeparacionIndemnizacion(SeparacionIndemnizacion? separacion, string? totalSeparacion)
    {
        if (string.IsNullOrEmpty(totalSeparacion))
        {
            separacion = null;
            return;
        }

        if (separacion is null)
        {
            _context.AddError(
                _sectionJubilacion, 
                "Existen Percepciones con la clave TipoPercepcion 022, 023 o 025, por lo que debe existir el elemento SeparacionIndemnizacion");
            return;
        }

        var totalPagado = separacion.TotalPagado;
        separacion.TotalPagado = FormatHelper.FormatStringToImporteSat(totalPagado) ?? "0";

        var numAniosServicio = decimal.Parse(separacion.NumeroAniosServicio);
        separacion.NumeroAniosServicio = numAniosServicio.ToString("F0");
        
        var ultimoSueldo = separacion.UltimoSueldoMensualOrdinario;
        separacion.UltimoSueldoMensualOrdinario = FormatHelper.FormatStringToImporteSat(ultimoSueldo) ?? "0";
        
        var ingresoAcumulable = separacion.IngresoAcumulable;
        separacion.IngresoAcumulable = FormatHelper.FormatStringToImporteSat(ingresoAcumulable) ?? "0";
        
        var ingresoNoAcumulable = separacion.IngresoNoAcumulable;
        separacion.IngresoNoAcumulable = FormatHelper.FormatStringToImporteSat(ingresoNoAcumulable) ?? "0";
    }

    private void FormatJubilacionPensionRetiro(
        JubilacionPensionRetiro? jubilacion,
        string? totalJubilacion)
    {
        if (string.IsNullOrEmpty(totalJubilacion))
        {
            jubilacion = null;
            return;
        }

        if (jubilacion is null)
        {
            _context.AddError(
                _sectionJubilacion, 
                "Existen Percepciones con la clave TipoPercepcion 039 o 044, por lo que debe existir el elemento JubilacionPensionRetiro");
            return;
        }

        var totalUnaExhibicion =
            jubilacion.TotalUnaExhibicion;
        jubilacion.TotalUnaExhibicion = FormatHelper.FormatStringToImporteSat(totalUnaExhibicion);

        var totalParcialidad = jubilacion.TotalParcialidad;
        jubilacion.TotalParcialidad =  FormatHelper.FormatStringToImporteSat(totalParcialidad);
        
        var montoDiario =  jubilacion.MontoDiario;
        jubilacion.MontoDiario = FormatHelper.FormatStringToImporteSat(montoDiario);
        
        var ingresoAcumulable = jubilacion.IngresoAcumulable;
        jubilacion.IngresoAcumulable = FormatHelper.FormatStringToImporteSat(ingresoAcumulable) ?? "0";
        
        var ingresoNoAcumulable = jubilacion.IngresoNoAcumulable;
        jubilacion.IngresoNoAcumulable = FormatHelper.FormatStringToImporteSat(ingresoNoAcumulable) ?? "0";

        if (!string.IsNullOrEmpty(jubilacion.TotalUnaExhibicion))
        {
            jubilacion.MontoDiario = null;
            jubilacion.TotalParcialidad = null;
        }

        if (!string.IsNullOrEmpty(jubilacion.TotalParcialidad))
        {
            if (string.IsNullOrEmpty(jubilacion.MontoDiario))
            {
                _context.AddError(_sectionJubilacion, "Si existe valor en el atributo TotalParcialidad el atributo MontoDiario debe existir");
                return;
            }
            jubilacion.TotalUnaExhibicion = null;
        }
    }

    private void SetTotales(Percepciones percepciones)
    {
        var totalSuedos = _helper.GetTotalSueldos();
        percepciones.TotalSueldos = totalSuedos != decimal.Zero ? totalSuedos.ToString("F2") : null;

        var totalSeparacion = _helper.GetTotalSeparacionIndemnizacion();
        percepciones.TotalSeparacionIndemnizacion = totalSeparacion != decimal.Zero ? totalSeparacion.ToString("F2") : null;
        
        var totalJubilacion = _helper.GetTotalJubilacion();
        percepciones.TotalJubilacionPensionRetiro = totalJubilacion != decimal.Zero ? totalJubilacion.ToString("F2") : null;
        
        var totalGravado  = _helper.GetTotalGravado();
        percepciones.TotalGravado = totalGravado.ToString("F2");
        
        var totalExento = _helper.GetTotalExento();
        percepciones.TotalExento = totalExento.ToString("F2");
    }

    private void FormatPercepcion(Percepcion percepcion, int index)
    {
        var importeGravado = decimal.Parse(percepcion.ImporteGravado);
        var importeExento = decimal.Parse(percepcion.ImporteExento);
        var total = importeGravado + importeExento;
        if (total < 0)
        {
            _context.AddError(
                string.Format(_section, index), 
                "La suma de los importes de los atributos ImporteGravado e ImporteExento no es mayor que cero");
            return;
        }

        if (!CatalogosNomina.c_TipoPercepcion.Contains(percepcion.Tipo))
        {
            _context.AddError(string.Format(_section, index), "El valor del atributo TipoPercepcion, debe ser una clave del catálogo de c_TipoPercepcion");
            return;
        }
        percepcion.ImporteGravado = importeGravado.ToString("F2");
        percepcion.ImporteExento = importeExento.ToString("F2");
        
        FormatAccionesOTitulos(percepcion.AccionesOTitulos, percepcion.Tipo, index);
        
        FormatHorasExtra(percepcion.HorasExtra, percepcion.Tipo, index);
        
        _context.AddTipoPercepcion(percepcion.Tipo);
        
        
        
    }

    private void FormatAccionesOTitulos(AccionesOTitulos? accionesOTitulos, string percepcionTipo, int index)
    {
        if (percepcionTipo is "045" && accionesOTitulos == null)
        {
            _context.AddError(
                string.Format(_sectionAcciones, index), 
                "El elemento AccionesOTitulos debe existir, ya que la clave expresada en el atributo Nomina.Percepciones.Percepcion.TipoPercepcion es 045");
            return;
        }
        if(accionesOTitulos == null)
            return;
        var valor = decimal.Parse(accionesOTitulos.ValorMercado);
        accionesOTitulos.ValorMercado = valor.ToString("F6");
        var precio = decimal.Parse(accionesOTitulos.PrecioAlOtorgarse);
        accionesOTitulos.PrecioAlOtorgarse = precio.ToString("F6");

    }

    private void FormatHorasExtra(List<HorasExtras>? horasExtra, string percepcionTipo, int index)
    {
        if (percepcionTipo is "019" && horasExtra == null)
        {
            _context.AddError(
                string.Format(_section, index), 
                "El elemento HorasExtra, debe existir, ya que la clave expresada en el atributo Nomina.Percepciones.Percepcion.TipoPercepcion es 019");
            return;
        }
        if (horasExtra == null || horasExtra.Count == 0)
            return;
        var count =  horasExtra.Count;
        for (var i = 0; i < count; i++)
        {
            var hora = horasExtra[i];
            FormatHoraExtra(hora, index, i + 1);
        }
        
    }

    private void FormatHoraExtra(HorasExtras hora, int indexPercepcion, int indexHora)
    {
        var section = string.Format(_sectionHoras, indexPercepcion, indexHora);
        if (!CatalogosNomina.c_TipoHoras.Contains(hora.TipoHoras))
        {
            _context.AddError(section, "El valor del atributo TipoHoras no cumple con un valor del catálogo c_TipoHoras");
        }
        var dias =  decimal.Parse(hora.Dias);
        hora.Dias = dias.ToString("F0");
        var horasExtra = decimal.Parse(hora.HorasExtra);
        hora.HorasExtra = horasExtra.ToString("F0");

        var importe = decimal.Parse(hora.ImportePagado);
        hora.ImportePagado = importe.ToString("F2");
    }
}