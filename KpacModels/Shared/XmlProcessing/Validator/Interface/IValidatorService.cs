using KpacModels.Shared.Models.Comprobante;
using KpacModels.Shared.Models.Core;

namespace KpacModels.Shared.XmlProcessing.Validator.Interface;

public interface IValidatorService
{
    public Task<(List<Warning>, List<Error>)> Validate(Comprobante40 comprobante);
}