using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdventOfCode2022;

internal class Day2 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        const int lost = 0;
        const int draw = 3;
        const int win = 6;
        var opponentShape = new Dictionary<string, RPS>
        {
            ["A"] = RPS.Rock,
            ["B"] = RPS.Paper,
            ["C"] = RPS.Scissors,
        };
        var myShape = new Dictionary<string, RPS>
        {
            ["X"] = RPS.Rock,
            ["Y"] = RPS.Paper,
            ["Z"] = RPS.Scissors,
        };
        var scoreRPS = new Dictionary<RPS, int>
        {
            [RPS.Rock] = 1,
            [RPS.Paper] = 2,
            [RPS.Scissors] = 3,
        };
        var scoreSecondPart = new Dictionary<string, int>
        {
            ["X"] = lost,
            ["Y"] = draw,
            ["Z"] = win,
        };
        var result = 0;
        var resultSecondPart = 0;
        foreach (var line in inputLines.Where(l => !string.IsNullOrEmpty(l)))
        {
            var round = InputReader.ProcessStringLine(line!);
            result += CalculateRoundScore(opponentShape[round[0]], myShape[round[1]]);
            resultSecondPart += CalculateSecondRoundOutcome(opponentShape[round[0]], scoreSecondPart[round[1]]!);
        }

        Console.WriteLine($"Day 2 result part 1: {result}");
        Console.WriteLine($"Day 2 result part 2: {resultSecondPart}");

        int CalculateRoundScore(RPS opponent, RPS mine) => scoreRPS[mine] + CalculateRoundOutcome(opponent, mine);

        int CalculateRoundOutcome(RPS opponent, RPS mine)
        {
            var outcomeOfRound = lost;

            if (opponent == mine)
            {
                outcomeOfRound = draw;
            }
            else if (mine == RPS.Rock && opponent == RPS.Scissors 
                  || mine == RPS.Scissors && opponent == RPS.Paper
                  || mine == RPS.Paper && opponent == RPS.Rock)
            {
                outcomeOfRound = win;
            }

            return outcomeOfRound;
        }

        int CalculateSecondRoundOutcome(RPS opponent, int roundOutcomeSecondPart)
        {
            var outcomeOfRound = roundOutcomeSecondPart;

            if (roundOutcomeSecondPart == draw)
            {
                outcomeOfRound += scoreRPS[opponent];
            }
            else if (roundOutcomeSecondPart == lost)
            {
                switch (opponent)
                {
                    case RPS.Rock:
                        outcomeOfRound += scoreRPS[RPS.Scissors];
                        break;
                    case RPS.Scissors:
                        outcomeOfRound += scoreRPS[RPS.Paper];
                        break;
                    case RPS.Paper:
                        outcomeOfRound += scoreRPS[RPS.Rock];
                        break;
                    default:
                        outcomeOfRound += 0;
                        break;
                }
            }
            else
            {
                switch (opponent)
                {
                    case RPS.Scissors:
                        outcomeOfRound += scoreRPS[RPS.Rock];
                        break;
                    case RPS.Paper:
                        outcomeOfRound += scoreRPS[RPS.Scissors];
                        break;
                    case RPS.Rock:
                        outcomeOfRound += scoreRPS[RPS.Paper];
                        break;
                    default:
                        outcomeOfRound += 0;
                        break;
                }
            }

            return outcomeOfRound;
        }
    }
}

internal enum RPS
{
    Rock,
    Paper,
    Scissors
}
