using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;
using Emisor = KpacModels.Shared.Models.Comprobante.Emisor;
using Receptor = KpacModels.Shared.Models.Comprobante.Receptor;

namespace KpacModels.Shared.XmlProcessing.Validator.Interface;

public interface IVisitor
{
    Task VisitBasic(Comprobante40 root);
    void Visit(InformacionGlobal? informacionGlobal);
    void Visit(Emisor emisor);
    Task Visit(CfdiRelacionado cfdiRelacionados, int noCfdi);
    void Visit(Receptor receptor);
    Task Visit(Concepto concepto, int numConcepto);
    void Visit(Impuestos? impuestos);
    
    void Visit(Comprobante40 root);
    
    bool HasErrors();
    
    (List<Warning>, List<Error>) GetValidationResult();
}