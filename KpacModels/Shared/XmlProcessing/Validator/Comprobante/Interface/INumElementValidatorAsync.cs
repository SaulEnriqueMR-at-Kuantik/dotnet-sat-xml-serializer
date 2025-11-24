using KPac.Application.Validator;

namespace KpacModels.Shared.XmlProcessing.Validator.Comprobante.Interface;


/// <summary>
/// This interface is used for make a validation async to an element from a list, and requires 
/// </summary>
/// <typeparam name="T">Name of the class to Validate</typeparam>
public interface INumElementValidatorAsync<T>
{
    Task Validate(T element, int num, ValidatorContext context);
}