using PumpClient.PumpServiceReference;
using System;

namespace PumpClient
{
    public class CallbackHandler : IPumpServiceCallback
    {
        public void UpdateStatistics(StatisticsService statistics)
        {
            Console.Clear();
            Console.WriteLine("Обновление по статистике выполнения скрипта:");
            Console.WriteLine($"Всего     запусков: {statistics.AllCounts}");
            Console.WriteLine($"Успешных  запусков: {statistics.SuccessCounts}");
            Console.WriteLine($"Ошибочных запусков: {statistics.ErrorCounts}");
        }
    }
}
