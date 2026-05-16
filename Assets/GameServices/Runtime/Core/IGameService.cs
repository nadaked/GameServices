using System.Threading.Tasks;

namespace GameServices.GameServices.Runtime.Core
{
    public interface IGameService
    {
        string ServiceId { get; }
        GameServiceStatus Status { get; }
        bool IsReady { get; }

        Task InitializeAsync(GameServiceContext context);
    }
}


