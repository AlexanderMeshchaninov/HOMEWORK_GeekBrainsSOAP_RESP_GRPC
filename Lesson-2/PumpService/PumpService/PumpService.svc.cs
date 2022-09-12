using PumpService.Core;
using PumpService.Services;
using System.ServiceModel;

namespace PumpService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        private readonly IScriptService _scriptService;
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _serviceSettings;

        public PumpService()
        {
            _statisticsService = new StatisticsService();
            _serviceSettings = new SettingsService();
            _scriptService = new ScriptService(Callback, _serviceSettings, _statisticsService);
        }

        public void RunScript()
        {
            _scriptService.Run(15);
        }

        public void UpdateAndCompileScript(string fileName)
        {
            _serviceSettings.FileName = fileName;
            _scriptService.Compile();
        }

        IPumpServiceCallback Callback
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    return OperationContext.Current.GetCallbackChannel<IPumpServiceCallback>();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
