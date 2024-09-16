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

        private int _counter = 0;

        public SendMatchData(ILogger<SendMatchData> logger, IHubContext<MatchHub, IMatchClient> context)
        {
            _logger = logger;
            _context = context;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string mfile = File.ReadAllText("JsonData/IndvsPak.json");
            MatchModel matchData = JsonConvert.DeserializeObject<MatchModel>(mfile);
            var ball = new BallByBallModel();
            ball.Runs = new Runs();
            ball.Wickets = new List<Wicket> { new Wicket() };
            ball.Delivery = new Delivery();
            ball.Runs.Extras = 0;
            ball.Runs.Total = 0;
            ball.Runs.Batter = 0;
            ball.Delivery.Batter = "";
            ball.Delivery.Non_striker = "";
            ball.Delivery.Bowler = "";

            if (matchData != null)
            {
                ball.Runs.Extras = 0;
                ball.Runs.Total = 0;
                ball.Runs.Batter = 0;
                ball.Delivery.Batter = "";
                ball.Delivery.Non_striker = "";
                ball.Status = "Success";

                await _context.Clients.All.ReceiveMatchInfo(matchData.Info);
                await _context.Clients.All.ReceiveLiveData(ball);

                foreach (var inning in matchData.Innings)
                {
                    await Task.Delay(TimeSpan.FromSeconds(new Random().Next(5, 10)));
                    ball.Team = inning.Team;
                    ball.Runs = new Runs();
                    ball.Wickets = new List<Wicket> { new Wicket() };
                    ball.Over = new Over2();
                    ball.Over.OverNumber = 0;
                    ball.Over.BallNumber = 0;
                    foreach (var over in inning.Overs)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(new Random().Next(2, 5)));
                        foreach (var delivery in over.Deliveries)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(new Random().Next(2, 5)));
                            ball.Delivery = delivery;
                            ball.Runs.Extras += delivery.Runs.Extras;
                            ball.Runs.Batter += delivery.Runs.Batter;
                            ball.Runs.Total += delivery.Runs.Total;
                            if (delivery.Wickets != null)
                            {
                                foreach (var wickets in delivery.Wickets)
                                    ball.Wickets.Add(wickets);
                            }
                            if (delivery.Runs.Extras == 0 || (delivery.Runs.Extras > 0 && delivery.Extras?.LegByes != null))
                            {
                                ball.Over.BallNumber += 1;
                            }
                            if (ball.Over.BallNumber == 6)
                            {
                                ball.Over.OverNumber += 1;
                                ball.Over.BallNumber = 0;
                            }
                            await _context.Clients.All.ReceiveLiveData(ball);
                        }

                    }

                    ball.IsSecondInning = true;
                    ball.FirstInniingWickets = ball.Wickets.Count;
                    ball.FirstInningScore = ball.Runs.Total;

                }
            }
            ball.Status = "Failed to get data";
            await _context.Clients.All.ReceiveLiveData(ball);

        }
    }
}