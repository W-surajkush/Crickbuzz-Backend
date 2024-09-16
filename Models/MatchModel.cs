namespace Cricbuzz.Models
{
    public class Meta
    {
        public string DataVersion { get; set; }
        public string Created { get; set; }
        public int Revision { get; set; }
    }

    public class Event
    {
        public int Group { get; set; }
        public int MatchNumber { get; set; }
        public string Name { get; set; }
    }

    public class Officials
    {
        public List<string> MatchReferees { get; set; }
        public List<string> ReserveUmpires { get; set; }
        public List<string> TvUmpires { get; set; }
        public List<string> Umpires { get; set; }
    }

    public class By
    {
        public int Wickets { get; set; }
    }

    public class Outcome
    {
        public By By { get; set; }
        public string Winner { get; set; }
    }

    public class Players
    {
        public List<string> India { get; set; }
        public List<string> Pakistan { get; set; }
    }

    public class Registry
    {
        public Dictionary<string, string> People { get; set; }
    }

    public class Toss
    {
        public string Decision { get; set; }
        public string Winner { get; set; }
    }

    public class Info
    {
        public int BallsPerOver { get; set; }
        public string City { get; set; }
        public List<string> Dates { get; set; }
        public Event Event { get; set; }
        public string Gender { get; set; }
        public string MatchType { get; set; }
        public int MatchTypeNumber { get; set; }
        public Officials Officials { get; set; }
        public Outcome Outcome { get; set; }
        public int Overs { get; set; }
        public List<string> PlayerOfMatch { get; set; }
        public Players Players { get; set; }
        public Registry Registry { get; set; }
        public string Season { get; set; }
        public string TeamType { get; set; }
        public List<string> Teams { get; set; }
        public Toss Toss { get; set; }
        public string Venue { get; set; }
    }

    public class Runs
    {
        public int Batter { get; set; }
        public int Extras { get; set; }
        public int Total { get; set; }
    }

    public class Fielder
    {
        public string Name { get; set; }
    }

    public class Wicket
    {
        public string Kind { get; set; }
        public string Player_out { get; set; }
        public List<Fielder> Fielders { get; set; }
    }

    public class Delivery
    {
        public string Batter { get; set; }
        public string Bowler { get; set; }
        public string Non_striker { get; set; }
        public Runs Runs { get; set; }
        public List<Wicket> Wickets { get; set; }
        public Extras Extras { get; set; }

    }
    public class Extras
    {
        public int? Byes { get; set; }
        public int? LegByes { get; set; }
        public int? NoBalls { get; set; }
        public int? Penalty { get; set; }
        public int? Wides { get; set; }
    }
    public class Over
    {
        public int OverNumber { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }

    public class Powerplay
    {
        public double From { get; set; }
        public double To { get; set; }
        public string Type { get; set; }
    }

    public class Inning
    {
        public string Team { get; set; }
        public List<Over> Overs { get; set; }
        public List<Powerplay> Powerplays { get; set; }
    }
    public class Over2
    {
        public int OverNumber { get; set; }
        public int BallNumber { get; set; }
    }

    public class MatchModel
    {
        public Meta Meta { get; set; }
        public Info Info { get; set; }
        public List<Inning> Innings { get; set; }
    }
    public class BallByBallModel
    {
        public Delivery Delivery { get; set; }
        public List<Wicket> Wickets { get; set; }
        public Runs Runs { get; set; }

        public string Status { get; set; }

        public int FirstInningScore { get; set; }
        public int FirstInniingWickets { get; set; }
        public bool IsSecondInning { get; set; } = false;
        public string Team { get; set; }
        public Over2 Over { get; set; }

    }

}
