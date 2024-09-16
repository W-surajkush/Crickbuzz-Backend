using Cricbuzz.Models;
using Microsoft.AspNetCore.SignalR;

namespace Cricbuzz_Backend.Hubs;

public class MatchHub : Hub<IMatchClient>
{
    public override async Task OnConnectedAsync()
    {
        //await Clients.All.ReceiveLiveData("Connected to the Server :)");
        await base.OnConnectedAsync();
    }
}
public interface IMatchClient
{
    Task ReceiveLiveData(BallByBallModel liveData);
    Task ReceiveMatchInfo(Info _info);
}

