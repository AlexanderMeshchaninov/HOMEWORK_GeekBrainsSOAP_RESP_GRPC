using PumpService.Services;
using System.ServiceModel;

namespace PumpService.Core
{
    [ServiceContract]
    public interface IPumpServiceCallback
    {
        [OperationContract]
        void UpdateStatistics(StatisticsService statistics);
    }
}