namespace KpacModels.Shared.XmlProcessing;

public class DecimalOperator
{

    public static decimal Suma(string firstValue, string secondValue)
    {
        decimal result = decimal.Parse(firstValue) + decimal.Parse(secondValue);
        return Math.Round(result, 2, MidpointRounding.AwayFromZero);
    }

    public static decimal Suma(decimal firstValue, decimal secondValue)
    {
        decimal result = firstValue + secondValue;
        return Math.Round(result, 2, MidpointRounding.AwayFromZero);
    }

    public static decimal Substract(decimal firstValue, decimal secondValue)
    {
        decimal result = firstValue - secondValue;
        return Math.Round(result, 2, MidpointRounding.AwayFromZero);
    }

    public static decimal Substract(string firstValue, string secondValue)
    {
        decimal result = decimal.Parse(firstValue) - decimal.Parse(secondValue);
        return Math.Round(result, 2, MidpointRounding.AwayFromZero);
    }

    // public static decimal CalculateTax(decimal value, decimal taxRate)
    // {
    // decimal result = Math.Round(value, 2, MidpointRounding.AwayFromZero) * Math.Round(taxRate, 2, MidpointRounding.AwayFromZero);
    // result = Math.Round(result, 2, MidpointRounding.AwayFromZero);
    // return result;
    //}

    public static decimal CalculateTax(decimal value, decimal taxRate)
    {

            // if (ValidateHelper.CountDecimalPlaces(value) > 3 || ValidateHelper.CountDecimalPlaces(taxRate) > 3)
            // {
            //     return Math.Round(value * taxRate, 3, MidpointRounding.AwayFromZero);
            // }
            var result = Math.Round(value * taxRate, 2, MidpointRounding.AwayFromZero);
            return result;
    }
}