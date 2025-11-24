using KpacModels.Shared.XmlProcessing.Validator;

namespace KPac.Application.Validator;

/// <summary>
/// Clase para calcular limite inferior y limite superior 
/// </summary>
public static class DecimalOperatorLimites
{
    /// <summary>
    /// Calcular el límite inferior importe en el nodo Concepto, basado en la siguiente fórmula:
    /// (Cantidad - ((10^-NumDecimalesCantidad)/2)*(ValorUnitario - ((10^-NumDecimalesValorUnitario)/2)
    /// Fórmula creada apartir de la documentación (Anexo 20, página 76):
    /// <see href="http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/Anexo20_2022.pdf">Anexo 20 de la Resolución Miscelánea Fiscal para 2022</see>
    /// </summary>
    /// <param name="cantidad">Cantidad de productos</param>
    /// <param name="valorUnitario">Valor unitario del producto</param>
    /// <param name="decimalesMoneda">Número de decimales de la moneda registrada en el cfdi</param>
    /// <returns>Limite inferior importe en decimal</returns>
    public static decimal CalcularLimiteInferiorConcepto(
        decimal cantidad, 
        decimal valorUnitario,
        int decimalesMoneda)
    {
        var decimalesCantidad = ValidateHelper.CountDecimalPlaces(cantidad);
        var decimalesValorUnitario = ValidateHelper.CountDecimalPlaces(valorUnitario);
        decimal valorInferior = valorUnitario - ((Potencia10(- decimalesValorUnitario) / 2));
        decimal cantidadInferior = cantidad - ((Potencia10(- decimalesCantidad) / 2));
        decimal resultado = cantidadInferior * valorInferior;
        
        return TruncarDecimal(resultado, decimalesMoneda);
    }
    /// <summary>
    /// Calcular el límite superior importe en el nodo Concepto, basado en la siguiente fórmula:
    /// (Cantidad + ((10^-NumDecimalesCantidad)/2) - (10^-12)) * (ValorUnitario + ((10^-NumDecimalesValorUnitario)/2) - (10^-12))
    /// Fórmula creada apartir de la documentación (Anexo 20, página 76):
    /// <see href="http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/Anexo20_2022.pdf">Anexo 20 de la Resolución Miscelánea Fiscal para 2022</see>
    /// </summary>
    /// <param name="cantidad">Cantidad de productos</param>
    /// <param name="valorUnitario">Valor unitario del producto</param>
    /// <param name="decimalesMoneda">Número de decimales de la moneda registrada en el cfdi</param>
    /// <returns>Limite superior importe en decimal</returns>
    public static decimal CalcularLimiteSuperiorConcepto(
        decimal cantidad, 
        decimal valorUnitario,
        int decimalesMoneda)
    {
        var decimalesCantidad = ValidateHelper.CountDecimalPlaces(cantidad);
        var decimalesValorUnitario = ValidateHelper.CountDecimalPlaces(valorUnitario);
        decimal cantidadInferior = cantidad + ((Potencia10(- decimalesCantidad) / 2) - Potencia10(- 12));
        decimal valorInferior = valorUnitario + ((Potencia10(- decimalesValorUnitario) / 2) - Potencia10(- 12));
        decimal resultado = cantidadInferior * valorInferior;
        
        return RedondearArriba(resultado, decimalesMoneda);
    }
    
    /// <summary>
    /// Calcular el límite inferior importe en el nodo Impuesto, basado en la siguiente fórmula:
    /// (Base - 10^-NumDecimalesBase/2)*(TasaOCuota)
    /// Fórmula creada apartir de la documentación (Anexo 20, página 78):
    /// <see href="http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/Anexo20_2022.pdf">Anexo 20 de la Resolución Miscelánea Fiscal para 2022</see>
    /// </summary>
    /// <param name="base">Base del impuesto</param>
    /// <param name="tasaOCuota">TasaOCuota del impuesto</param>
    /// <param name="decimalesMoneda">Número de decimales que tenga registrado la moneda.</param>
    /// <returns>Limite inferior importe en decimal</returns>
    public static decimal CalcularLimiteInferiorImpuesto(
        decimal @base, 
        decimal tasaOCuota,
        int decimalesImporte)
    {
        var decimalesBase = ValidateHelper.CountDecimalPlaces(@base);
        decimal baseInferior = @base - (Potencia10(- decimalesBase) / 2);
        decimal resultado = baseInferior * tasaOCuota;
        
        return TruncarDecimal(resultado, decimalesImporte);
    }
    
    /// <summary>
    /// Calcular el límite superior importe en el nodo Importe, basado en la siguiente fórmula:
    /// (Base + (10^-NumDecimalesBase)/2 - 10^-12) *(TasaOCuota)
    /// Fórmula creada apartir de la documentación (Anexo 20, página 76):
    /// <see href="http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/Anexo20_2022.pdf">Anexo 20 de la Resolución Miscelánea Fiscal para 2022</see>
    /// </summary>
    /// <param name="base">Cantidad de productos</param>
    /// <param name="tasaOCuota">Valor unitario del producto</param>
    /// <param name="decimales">Número de decimales de la moneda registrada en el cfdi</param>
    /// <returns>Limite superior importe en decimal</returns>
    public static decimal CalcularLimiteSuperiorImpuesto(
        decimal @base, 
        decimal tasaOCuota,
        int decimales)
    {
        var decimalesBase = ValidateHelper.CountDecimalPlaces(@base);
        decimal baseInferior = @base + ((Potencia10(- decimalesBase) / 2) - Potencia10(- 12));
        decimal resultado = baseInferior * tasaOCuota;
        
        return RedondearArriba(resultado, decimales);
    }

    /// <summary>
    /// Calcular el límite inferior del campo Monto en el nodo Pago, basado en la siguiente fórmula:
    /// [ImportePagado - (10^-NumDecimalesImportePagado/2)] / [EquivalenciaDR + (10^-NumDecimalesEquivalenciaDR)/(2 - 0.0000000000000001)]
    /// Fórmula creada apartir de la Guía de llenado del Complemento Pagos 2.0 pagína 28
    /// <see href="http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/Guia_llenado_pagos.pdf">Guía de llenado del
    /// comprobante al que se le incorpore el complemento para recepción de pagos</see>
    /// </summary>
    /// <param name="importePagado">Atributo ImpPagado en decimal</param>
    /// <param name="equivalenciaDr">Atributo EquivalenciaDR en decimal</param>
    /// <param name="decimalesMonedaP">Número de decimales de la moneda registrada en el campo MonedaP</param>
    /// <returns></returns>
    public static decimal CalcularLimiteInferiorMonto(decimal importePagado, decimal equivalenciaDr, int decimalesMonedaP)
    {
        var noDecimalesImpPagado = ValidateHelper.CountDecimalPlaces(importePagado);
        var noDecimalesEquivalencia = ValidateHelper.CountDecimalPlaces(equivalenciaDr);
        var dividendo = importePagado - (Potencia10( - noDecimalesImpPagado) / 2);
        var divisor = equivalenciaDr + (Potencia10( - noDecimalesEquivalencia) / 2) - 0.0000000000000001m;
        var result = dividendo / divisor;
        return TruncarDecimal(result, decimalesMonedaP);
    }
    
    /// <summary>
    /// Calcular el límite superior del campo Monto en el nodo Pago, basado en la siguiente fórmula:
    /// [ImportePagado + (10^-NumDecimalesImportePagado/(2- 0.0000000000000001)] / [EquivalenciaDR - (10^-NumDecimalesEquivalenciaDR)/ 2]
    /// Fórmula creada apartir de la Guía de llenado del Complemento Pagos 2.0 pagína 28
    /// <see href="http://omawww.sat.gob.mx/tramitesyservicios/Paginas/documentos/Guia_llenado_pagos.pdf">Guía de llenado del
    /// comprobante al que se le incorpore el complemento para recepción de pagos</see>
    /// </summary>
    /// <param name="importePagado"></param>
    /// <param name="equivalenciaDr"></param>
    /// <param name="decimalesMonedaP"></param>
    /// <returns></returns>
    public static decimal CalcularLimiteSuperiorMonto(decimal importePagado, decimal equivalenciaDr, int decimalesMonedaP)
    {
        var noDecimalesImpPagado = ValidateHelper.CountDecimalPlaces(importePagado);
        var noDecimalesEquivalencia = ValidateHelper.CountDecimalPlaces(equivalenciaDr);
        var dividendo = importePagado + (Potencia10( - noDecimalesImpPagado) / 2) - 0.0000000000000001m;
        var divisor = equivalenciaDr - (Potencia10( - noDecimalesEquivalencia) / 2) ;
        var result = dividendo / divisor;
        return RedondearArriba(result, decimalesMonedaP);
    }

    /// <summary>
    /// Calcular el límite inferior del campo ImporteDr, basado en la siguiente fórmula:
    /// [BaseDR - (10^-NumDecimalesBaseDR/ 2)] * [TasaOCuotaDR]
    /// Y el resultado se debe truncar a la cantidad de decimales que tenga el atributo original ImporteDR
    /// Fórmula creada apartir de la siguiente documentación:
    /// <see href="https://developers.sw.com.mx/knowledge-base/25-abril-2023-calculo-para-importedr-tomando-en-cuenta-limite-inferior-y-superior/">Cálculo para ImporteDR tomando en cuenta límite inferior y superior.</see>
    /// </summary>
    /// <param name="baseDr"></param>
    /// <param name="tasaOCuotaDr"></param>
    /// <param name="importe"></param>
    /// <returns></returns>
    public static decimal CalcularLimiteInferiorImporteDr(decimal baseDr, decimal tasaOCuotaDr, decimal importe)
    {
        var decimalesBaseDr = ValidateHelper.CountDecimalPlaces(baseDr);
        var decimalesImporteDr = ValidateHelper.CountDecimalPlaces(importe);
        var result = (baseDr - Potencia10( - decimalesBaseDr) / 2) * tasaOCuotaDr;
        return TruncarDecimal(result, decimalesImporteDr);
    }

    /// <summary>
    /// Calcular el límite superior del campo ImporteDr, basado en la siguiente fórmula:
    /// [BaseDR + (10^-NumDecimalesBaseDR/ 2) - (10^12)] * [TasaOCuotaDR]
    /// Y el resultado se debe redondear arriba a la cantidad de decimales que tenga el atributo original ImporteDR
    /// Fórmula creada apartir de la siguiente documentación:
    /// <see href="https://developers.sw.com.mx/knowledge-base/25-abril-2023-calculo-para-importedr-tomando-en-cuenta-limite-inferior-y-superior/">Cálculo para ImporteDR tomando en cuenta límite inferior y superior.</see>
    /// </summary>
    /// <param name="baseDr"></param>
    /// <param name="tasaOCuotaDr"></param>
    /// <param name="importe"></param>
    /// <returns></returns>
    public static decimal CalcularLimiteSuperiorImporteDr(decimal baseDr, decimal tasaOCuotaDr, decimal importe)
    {
        var decimalesBaseDr = ValidateHelper.CountDecimalPlaces(baseDr);
        var decimalesImporteDr = ValidateHelper.CountDecimalPlaces(importe);
        var result = (baseDr + Potencia10( - decimalesBaseDr) / 2 - Potencia10( - 12))  * tasaOCuotaDr;
        return RedondearArriba(result, decimalesImporteDr);
    }

    public static decimal RedondearArriba(decimal valor, int decimales)
    {
        decimal factor = Potencia10(decimales);
        return Math.Ceiling(valor * factor) / factor;
    }
    
    private static decimal Potencia10(int exponente)
    {
        return (decimal)Math.Pow(10, exponente);
    }
    
    /// <summary>
    /// Truncar un valor decimal a un numero de decimales
    /// </summary>
    /// <param name="valor">Decimal</param>
    /// <param name="decimales">Integer</param>
    /// <returns></returns>
    public static decimal TruncarDecimal(decimal valor, int decimales)
    {
        decimal factor = Potencia10(decimales);
        return Math.Truncate(valor * factor) / factor;
    }
}