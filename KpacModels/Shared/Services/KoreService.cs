using KpacModels.Shared.Config;
using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Services.Interfaces;
using Newtonsoft.Json;

namespace KpacModels.Shared.Services;

public class KoreService : IKoreService
{

    private readonly KuantikOptions _options;

    public KoreService(KuantikOptions options)
    {
        _options  = options;
    }

    public async Task<Comprobante40?> GetComprobanteFromKoreByUuid(string uuid, string rfc)
    {
        var url = $"{_options.Kore}metadata/{uuid.ToUpper()}";
  
        Dictionary<string, string>? headers = new()
        {
            {"Rfc", rfc}
        };

        var response = await Http.GetAsync(
            url: url, headers: headers );

        if (response.IsSuccessful)
        {
            if (response.Content != null)
            {
                
                var comprobante = JsonConvert.DeserializeObject<Comprobante40>(response.Content);
                return comprobante;
            }
        }
        return null;
    }
}