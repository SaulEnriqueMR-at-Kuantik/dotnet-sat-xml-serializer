using System.Xml.Serialization;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.XmlProcessing.Serializer;
using Xunit;

namespace KpacModels.Tests;

public class AddentaSerializer_Test
{
    [Fact]
    public void Test1()
    {
        var addenda = new Addenda()
        {
            InformacionAdicional = [
            new InformacionAdicionalAddenda()]
        };
        var info = addenda.InformacionAdicional.First();
        info.PTDAo =
        [
            new Ptdao()
            {
                Etiqueta1PO = "1",
                Etiqueta2PO = "2",
                Etiqueta3PO = "3",
                Etiqueta4PO = "4",
                Etiqueta5PO = "5",
                Valor1PO = "1",
                Valor2PO = "2",
                Valor3PO = "3",
                Valor4PO = "4",
            },
            new Ptdao()
            {
                Etiqueta1PO = "10",
                Etiqueta2PO = "20",
                Etiqueta3PO = "30",
                Etiqueta4PO = "40",
                Etiqueta5PO = "50",
                Valor1PO = "10",
                Valor2PO = "20",
                Valor3PO = "30",
                Valor4PO = "40",
            }

        ];
        
        var xml = CfdiSerializer.SerializeXml(addenda);
        Assert.NotNull(xml);
        
    }
}