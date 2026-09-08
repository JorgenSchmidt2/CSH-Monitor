using CSH_Monitor.Core.Entities.CommonEnumerableEntities;
using CSH_Monitor.Core.Entities.DataEntities.StabilityDataEntities;
using CSH_Monitor.Core.Interfaces.Infrastructure;
using CSH_Monitor.Core.Responses;

namespace CSH_Monitor.Infrastructure.Parsing.TabularParsers
{
    public partial class TabularParser : ITabularParser
    {
        public EntityResponse<StabilityInputData> ReadStabilityData (string InputData)
        {
            EntityResponse<StabilityInputData> Result = new();
            Result.Data = new();

            if (string.IsNullOrWhiteSpace(InputData))
            {
                Result.Status = false;
                Result.Message = "Входные данные пусты.";
                return Result;
            }

            try
            {
                var Data = new StabilityInputData();

                var splits = InputData.Replace(".", ",")
                                 .Replace("\r", "")
                                 .Split ("\n", StringSplitOptions.RemoveEmptyEntries);

                int additionalFields = 0;

                foreach (var splited in splits)
                {
                    var curVal = splited.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

                    if (curVal.Length == 0) continue;

                    if (curVal.Length == 1)
                    {
                        if (!double.TryParse(curVal[0], out double value)) 
                        {
                            Result.Message += "\n\nНе удалось считать одно из дополнительных значений";
                        }
                        else
                        {
                            additionalFields++;
                            if (additionalFields == 1) { Data.TargetExpirationDate = value; }
                            if (additionalFields == 2) { Data.TargetUncertainty = value; }
                            if (additionalFields == 3) { Data.TargetError = value; }
                        }
                    }
                    else
                    {
                        var Item = new DataSeries<double, double>();
                        for (int i = 0; i < curVal.Length; i++) 
                        {
                            if (!double.TryParse(curVal[i], out double value))
                            {
                                Result.Message += "\nНе удалось считать значение из набора данных в строке " + splited + "\n";
                            }
                            else
                            {
                                if (i == 0)
                                    Item.Definitor = value;
                                else
                                    Item.Values.Add(value);
                            }
                        }

                        if (Item.Values.Count == 0)
                            Result.Message += "\n\n Одна из строк не считана \n\n";
                        else 
                            Data.Values.Add(Item);
                    }
                }

                Result.Status = true;
                Result.Data = Data;
            }
            catch (Exception e)
            {
                Result.Status = false;
                Result.Message = "Обработка входных данных измерений стабильности СО:\n\n" 
                    + Result.Message 
                    + "\n\nКритическая ошибка при обработке входных данных результатов измерения стабильности:\n" 
                    + e.Message + "\n\n";
            }

            return Result;
        }
    }
}