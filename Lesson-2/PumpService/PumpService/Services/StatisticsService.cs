using PumpService.Core;

namespace PumpService.Services
{
    public class StatisticsService : IStatisticsService
    {
        public int SuccessCounts { get; set; }
        public int ErrorCounts { get; set; }
        public int AllCounts { get; set; }
    }
}