
namespace PumpService.Core
{
    public interface IScriptService
    {
        bool Compile();
        void Run(int count);
    }
}