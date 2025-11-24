namespace KpacModels.Shared.XmlProcessing.Formatter.Nomina.Helper;

public static class AntiguedadHelper
{
    public static string CalcularSemanas(DateTime inicio, DateTime fin)
    {
        int totalDias = (fin - inicio).Days + 1;
        int semanas = totalDias / 7;
        return $"P{semanas}W";
    }
    public static string CalcularYMD(DateTime inicio, DateTime fin)
    {
        int years = 0, months = 0, days = 0;
        DateTime temp = inicio;

        while (temp.AddYears(1) <= fin)
        {
            years++;
            temp = temp.AddYears(1);
        }

        while (temp.AddMonths(1) <= fin)
        {
            months++;
            temp = temp.AddMonths(1);
        }

        days = (fin - temp).Days;

        return $"P{(years > 0 ? years + "Y" : "")}{(months > 0 ? months + "M" : "")}{days}D";
    }
    
}