using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Formatter.Interface;

public interface IVisitorFormatter
{
        void SaveAttributeBase(Comprobante40 root);
        void Visit(Comprobante40 root);
        void Visit(InformacionGlobal? informacionGlobal);
        void Visit(Emisor emisor);
        void Visit(CfdiRelacionado cfdiRelacionados, int noCfdi);
        void Visit(Receptor receptor);
        void Visit(Concepto concepto, int numConcepto);
        void Visit( Impuestos? impuestos);

        List<Warning> Errors();

        void Clean();
}