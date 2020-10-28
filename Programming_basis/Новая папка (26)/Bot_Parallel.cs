using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace rocket_bot
{
    public partial class Bot
    {
        public Task<Tuple<Turn, double>> GetBestPath(Rocket rocket)
        {
            return Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsCount/threadsCount));
        }

        public Rocket GetNextMove(Rocket rocket)
        {
            var taskList = new List<Task<Tuple<Turn, double>>>();
            for (var i = 0; i < threadsCount; i++)
                taskList.Add(GetBestPath(rocket));

            var task = Task.WhenAll(taskList);
            var tempResult = task.Result;
            var maxScore = tempResult.Max(value => value.Item2);
            var result = tempResult.Where(value => value.Item2 == maxScore)
                .Take(1)
                .ToList();

            var bestMove = result[0];
            var newRocket = rocket.Move(bestMove.Item1, level);
            return newRocket;
        }
    }
}