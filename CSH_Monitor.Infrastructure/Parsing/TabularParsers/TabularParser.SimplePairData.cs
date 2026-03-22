using CSH_Monitor.Core.Entities.CommonListEntities;
using CSH_Monitor.Core.Generics.CommonGenerics.TwoParamGenerics;
using CSH_Monitor.Core.Responses;
using System.Globalization;

namespace CSH_Monitor.Infrastructure.Parsing.TabularParsers
{
    public partial class TabularParser 
    {
        public DataResponse<MarkedDoubleRecList> GetDoubleMeasuredData (string InputData)
        {
            var Result = new DataResponse<MarkedDoubleRecList>
            {
                Message = "Парсинг данных типа \"Двуячеечная строка\"",
                Data = new MarkedDoubleRecList()
            };

            try
            {
                if (String.IsNullOrEmpty(InputData)) throw new Exception("Входная строка оказалась пуста.");

                var splits = InputData.Replace("\r", "").Replace(",",".").Split("\n");

                foreach (var Row in splits)
                {
                    if (String.IsNullOrWhiteSpace(Row) || String.IsNullOrEmpty(Row)) continue;

                    var parts = Row.Split("\t");

                    if (parts.Length == 2)
                    {
                        double time = 0;
                        double measure = 0;

                        if (double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out time) 
                        &&  double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out measure))
                        {
                            Result.Data.Add(new MarkedRecord<double, double>(time, measure));
                        }
                    }
                }

                if (Result.Data.Count == 0) throw new Exception("Входная строка не содержала пригодных для парсинга и записи двуячеечных строк.");

                Result.Message += ": успешно.";
                Result.Status = true;
            }
            catch (Exception e)
            {
                Result.Message += ", возникла ошибка: \n" + e.Message;
                Result.Status = false;
            }
            return Result;
        }
    }
}
