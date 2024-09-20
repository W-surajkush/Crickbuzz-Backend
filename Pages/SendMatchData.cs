using Cricbuzz.Models;
using Cricbuzz_Backend.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Cricbuzz_Backend.Pages
{
    public class SendMatchData : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(1);
        private readonly ILogger<SendMatchData> _logger;
        private readonly IHubContext<MatchHub, IMatchClient> _context;

        public SendMatchData(ILogger<SendMatchData> logger, IHubContext<MatchHub, IMatchClient> context)
        {
            _logger = logger;
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string matchFileContent = await File.ReadAllTextAsync("JsonData/IndvsPak.json");
            MatchModel matchData = JsonConvert.DeserializeObject<MatchModel>(matchFileContent);

            if (matchData == null)
            {
                await SendErrorMessage();
                return;
            }

            BallByBallModel ball = InitializeBall();

            foreach (var inning in matchData.Innings)
            {
                await BroadcastInitialData(ball);
                ball.Team = inning.Team;

                foreach (var over in inning.Overs)
                {
                    foreach (var delivery in over.Deliveries)
                    {
                        await ProcessDelivery(ball, delivery);
                        await BroadcastLiveData(ball, matchData.Info);
                    }
                }

                ball.IsSecondInning = true;
                ball.FirstInniingWickets = ball.Wickets.Count;
                ball.FirstInningScore = ball.Runs.Total;
            }
        }

        private async Task ProcessDelivery(BallByBallModel ball, Delivery delivery)
        {
            await Task.Delay(TimeSpan.FromSeconds(new Random().Next(2, 5)));

            ball.Delivery = delivery;
            ball.Runs.Extras += delivery.Runs.Extras;
            ball.Runs.Batter += delivery.Runs.Batter;
            ball.Runs.Total += delivery.Runs.Total;

            if (delivery.Wickets != null)
            {
                ball.Wickets.AddRange(delivery.Wickets);
            }

            // Increment ball number if valid
            if (delivery.Runs.Extras == 0 || delivery.Extras?.LegByes != null)
            {
                ball.Over.BallNumber += 1;
            }

            // Update over and reset ball number after 6 deliveries
            if (ball.Over.BallNumber == 6)
            {
                ball.Over.OverNumber += 1;
                ball.Over.BallNumber = 0;
            }
        }

        private async Task BroadcastLiveData(BallByBallModel ball, Info matchInfo)
        {
            await _context.Clients.All.ReceiveLiveData(ball);
            await _context.Clients.All.ReceiveMatchInfo(matchInfo);
        }

        private async Task BroadcastInitialData(BallByBallModel ball)
        {
            await _context.Clients.All.ReceiveLiveData(ball);
            await Task.Delay(TimeSpan.FromSeconds(new Random().Next(5, 10)));
        }

        private BallByBallModel InitializeBall()
        {
            return new BallByBallModel
            {
                Runs = new Runs(),
                Wickets = new List<Wicket>(),
                Delivery = new Delivery(),
                Status = "Success",
                IsSecondInning = false,
                Over = new Over2()
            };
        }

        private async Task SendErrorMessage()
        {
            var ball = new BallByBallModel { Status = "Failed to get data" };
            await _context.Clients.All.ReceiveLiveData(ball);
        }
    }
}
