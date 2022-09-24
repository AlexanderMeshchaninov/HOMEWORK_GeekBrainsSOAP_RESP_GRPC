using RootServiceReference;

namespace SampleService.Core
{
    public interface IRootServiceClient<T> where T : class
    {
        public RootServiceClient Client { get; }
        public Task<ICollection<T>> Get();
    }
}
