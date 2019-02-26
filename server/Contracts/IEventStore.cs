using System.Threading.Tasks;

namespace Contracts
{
    public interface IEventStore
    {
        Task StoreEventAsync(Event evt);
        Task StoreExceptionAsync(ClientException exc);
    }
}