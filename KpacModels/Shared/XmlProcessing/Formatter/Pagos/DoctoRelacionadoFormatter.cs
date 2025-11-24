using System.Globalization;
using KPac.Application.Formatter;
using KpacModels.Shared.Models.Comprobante.Complementos.Pagos;

namespace KpacModels.Shared.XmlProcessing.Formatter.Pagos;

public class DoctoRelacionadoFormatter
{
    
    private readonly FormatContext _context;
    
    private DoctoRelacionado _doctoRelacionado;

    private string _section;

    private string _monedaP;
    
    private decimal _importeSaldoAnterior;
    
    private decimal _importePagado;
    
    //private RepositoryValidator<cfdi40_moneda> repositoryMoneda;
    
    public DoctoRelacionadoFormatter(FormatContext context
        //ClientValidator client)
        )
    {
        _context = context;
        //repositoryMoneda = new RepositoryValidator<cfdi40_moneda>(client);
    }

    public async Task Format(DoctoRelacionado doctoRelacionado, int numPago, int numDocto)
    {
        _monedaP = _context.GetValue("monedaP") ?? "MXN";
        _doctoRelacionado = doctoRelacionado;
        _section = $"Comprobante -> Complemento -> Pagos -> {numPago}. Pago -> {numDocto}. Documento Relacionado ";
        _importePagado = decimal.Parse(_doctoRelacionado.ImportePagado ?? "0");
        _importeSaldoAnterior = decimal.Parse(_doctoRelacionado.ImporteSaldoAnterior ?? "0");

        await FormatMoneda();
        FormatEquivalencia();
        
        if(_importePagado <= decimal.Zero)
            _context.AddError(_section, "El campo Importe Pagado debe mayor a cero.");
        
        if(_importeSaldoAnterior <= decimal.Zero)
            _context.AddError(_section, "El campo Importe Saldo Anterior debe mayor a cero.");
        
        FormatImporteSaldoInsoluto();
        FormatImpuestos();
    }



    private async Task FormatMoneda()
    {

        if (_doctoRelacionado.Moneda == null)
        {
            _doctoRelacionado.Moneda = "MXN";
            return;
        }

        // var monedaObject = await repositoryMoneda.GetAsync("c_moneda", _doctoRelacionado.Moneda);
        // if (monedaObject == null)
        // {
        //     _context.AddError(_section, "El campo MonedaDR no contiene un valor del catálogo c_Moneda.");
        //     return;
        // }
        // TODO
        //  Limitar BaseDr e ImporteDR al numero de decimales de la moneda

        if (_doctoRelacionado.Moneda == "XXX")
        {
            _context.AddError(_section, "El valor del campo MonedaDR debe ser distinto de 'XXX'");
            return;
        }
    }

    private void FormatEquivalencia()
    {
        if (_doctoRelacionado.Moneda != _monedaP && string.IsNullOrEmpty(_doctoRelacionado.Equivalencia))
        {
            _context.AddError(
                _section, 
                $"Si el atributo MonedaP  = {_monedaP} es diferente a MonedaDR = {_doctoRelacionado.Moneda}, se debe registrar el atributo EquivalenciaDR");
        }

        if (_doctoRelacionado.Moneda == _monedaP)
            _doctoRelacionado.Equivalencia = "1";
    }

    private void FormatImporteSaldoInsoluto()
    {
        // Importe insoluto = saldo anterior - importe pagado
        var importeSaldoInsoluto = _importeSaldoAnterior - _importePagado;
        _doctoRelacionado.ImporteSaldoInsoluto = importeSaldoInsoluto.ToString(CultureInfo.InvariantCulture);
    }
    
    private void FormatImpuestos()
    {
        var objetoImpuesto = _doctoRelacionado.ObjetoImpuesto;
        if (objetoImpuesto == "02" && _doctoRelacionado.Impuestos == null)
        {
            _context.AddError(_section, "Si el valor del atributo ObjetoImpuestoDR es “02” el nodo hijo ImpuestosDR del nodo DoctoRelacionado debe existir.");
        }

        if (objetoImpuesto is "01" or "03" or "04" or "05")
            _doctoRelacionado.Impuestos = null;

        PagosFormatHelper.FiltrarImpuestos(impuestos: _doctoRelacionado.Impuestos, objetoImpuesto: objetoImpuesto);
    }
}