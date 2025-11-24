using KpacModels.Shared.Models.Comprobante;

namespace KpacModels.Shared.Services.Interfaces;

public interface IKoreService
{
    public Task<Comprobante40?> GetComprobanteFromKoreByUuid(string uuid, string rfc);
}