using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class Series
    {
        public int GamesPlayed { get; set; }
        public BHTeam Winner { get; set; }
        public int HomeWins { get; set; }
        public int AwayWins { get; set; }
        public BHTeam HomeTeam { get; set; }
        public BHTeam AwayTeam { get; set; }
        public List<GameStat> SeriesResult { get; set; }


        public Series(BHTeam home, BHTeam away, int? num)
        {
            GamesPlayed = 0;
            HomeWins = 0;
            AwayWins = 0;
            HomeTeam = home;
            AwayTeam = away;
            SeriesResult = new List<GameStat>();

            if (num == null)
            {
                var gameStat = new GameStat();
                while ((HomeWins == 0 || AwayWins == 0 || GamesPlayed < 3) && GamesPlayed < 100000)
                {
                    PlayGame(HomeTeam, AwayTeam, gameStat);
                    GamesPlayed++;
                    gameStat.GameNum = GamesPlayed;
                }
                SeriesResult.Add(gameStat);
            }
            else
            {
                while (Math.Max(HomeWins, AwayWins) <= num / 2)
                {
                    var gameStat = new GameStat();
                    PlayGame(HomeTeam, AwayTeam, gameStat);
                    GamesPlayed++;
                    gameStat.GameNum = GamesPlayed;
                    SeriesResult.Add(gameStat);
                }
            }

            Winner = null;
            if (HomeWins > AwayWins) Winner = home;
            if (HomeWins < AwayWins) Winner = away;
        }

        public void PlayGame(BHTeam home, BHTeam away, GameStat gameStat)
        {
            var tied = true;
            while (tied)
            {
                int score = 0;
                foreach (var player in home.Players)
                {
                    score += ApplyGauss(player.PPG);
                    score -= ApplyGauss(player.Steals);
                    score -= ApplyGauss(player.Rebounds);
                }
                gameStat.HomeScore = score;

                score = 0;
                foreach (var player in away.Players)
                {
                    score += ApplyGauss(player.PPG);
                    score -= ApplyGauss(player.Steals);
                    score -= ApplyGauss(player.Rebounds);
                }
                gameStat.AwayScore = score;
                if (gameStat.HomeScore > gameStat.AwayScore)
                {
                    HomeWins++;
                    tied = false;
                }
                if (gameStat.HomeScore < gameStat.AwayScore)
                {
                    AwayWins++;
                    tied = false;
                }
            }
        }

        public int ApplyGauss(float mean)
        {
            float stdDev = mean / 3;
            Random rand = new Random();
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdGauss = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randGauss = mean + stdDev * randStdGauss;

            return (int)randGauss;
        }
    }

    public class GameStat
    {
        public int GameNum { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

    }
}
