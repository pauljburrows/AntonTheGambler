using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Anton.TheGambler
{
    class Program
    {
        static void Main(string[] args)
        {
            const decimal startingBalance = 500;
            const decimal minimumBet = 0.5m;
            const int simulationCount = 1000;

            var antons = new List<Anton>();

            for (var i = 0; i < simulationCount; i++)
            {
                var simulation = new Simulation(startingBalance, minimumBet);
                var anton = simulation.Run();
                antons.Add(anton);
            }

            var averageEndingBalance = antons.Average(x => x.Balance);
            var averageBetCount = antons.Average(x => x.BetCount);

            var output = new
            {
                startingBalance,
                minimumBet,
                simulationCount,
                averageEndingBalance,
                averageBetCount
            };

            Console.WriteLine(JsonConvert.SerializeObject(output));            
            Console.ReadLine();
        }
    }

    public class Anton
    {        
        private static readonly Random Random = new Random();
        public decimal Balance { get; private set; }
        public int BetCount { get; private set; }

        public Anton(decimal startingBalance)
        {
            Balance = startingBalance;
        }

        public bool Gamble(decimal amount)
        {
            if (Balance < amount)
                throw new Exception("Anton is broke!");

            BetCount++;

            if (Random.Next() % 2 == 0)
            {
                Balance += amount;
                return true;
            }
            else
            {
                Balance -= amount;
                return false;
            }
        }
    }

    public class Simulation
    {
        public readonly decimal StartingBalance;
        public readonly decimal MinimumBet;

        public Simulation(decimal startingBalance, decimal minimumBet)
        {
            MinimumBet = minimumBet;
            StartingBalance = startingBalance;
        }

        public Anton Run()
        {
            var anton = new Anton(StartingBalance);
            var stake = MinimumBet;

            while (anton.Balance > stake && anton.BetCount < 100)
            {
                var winner = anton.Gamble(stake);
                if (winner)
                {
                    stake = MinimumBet;
                }
                else
                {
                    stake = stake * 2;
                }
            }

            return anton;            
        }
    }
}
