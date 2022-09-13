
namespace PumpService.Core
{
    public interface IStatisticsService
    {
        int SuccessCounts { get; set; }
        int ErrorCounts { get; set; }
        int AllCounts { get; set; }
    }
}