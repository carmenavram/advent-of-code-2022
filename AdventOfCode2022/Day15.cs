using System.Numerics;

namespace AdventOfCode2022;

internal class Day15 : IDay
{
    private const byte S = 0;
    private const byte B = 1;
    private const byte Dot = 2;
    private const byte NoDistress = 3;

    public void Solve(IList<string?> inputLines)
    {
        var result = 0;
        (int Min, int Max) distressLimit = (0, 4000000);

        (var sensorsDict, var beacons) = ReadSensorsAndBeacons(inputLines);
        var sensors = sensorsDict.Values.ToList();
        sensors = sensors.OrderBy(s => s.Position.y).ThenBy(s => s.Position.x).ToList();

        var yLineValues = GetLineValues(2000000);
        result = yLineValues.Count(v => v.Value == NoDistress);
        Console.WriteLine($"Day 15 result part 1: {result}");

        foreach (var sensor in sensors)
        {
            sensor.CalculateNoDistressLimits(distressLimit);
        }

        for (int line = distressLimit.Min; line <= distressLimit.Max; line++)
        {
            var distressX = FindDistressPointInLine(line);
            if (distressX.HasValue)
            {
                var distressPoint = new Point(distressX.Value, line);
                Console.WriteLine($"Distress beacon: {distressPoint.x} {distressPoint.y}");
                var multiply = BigInteger.Multiply(distressPoint.x, 4000000);
                BigInteger resultPart2 = multiply + distressPoint.y;
                Console.WriteLine($"Day 15 result part 2: {resultPart2}");
                break;
            }
        }

        int? FindDistressPointInLine(int yLine)
        {
            var sensorsThatCoverYLine = sensors.Where(s => s.NoDistressLimits.ContainsKey(yLine)).ToList();

            for (int i = 0; i < sensorsThatCoverYLine.Count; i++)
            {
                (int X, bool Overlap) outsideLimitPointLeft = (sensorsThatCoverYLine[i].NoDistressLimits[yLine].LowestX - 1, false);
                (int X, bool Overlap) outsideLimitPointRight = (sensorsThatCoverYLine[i].NoDistressLimits[yLine].HighestX + 1, false);

                if (outsideLimitPointLeft.X < distressLimit.Min || IsSensorOrBeacon(outsideLimitPointLeft.X, yLine))
                {
                    outsideLimitPointLeft.Overlap = true;
                }

                if (outsideLimitPointRight.X > distressLimit.Max || IsSensorOrBeacon(outsideLimitPointRight.X, yLine))
                {
                    outsideLimitPointRight.Overlap = true;
                }

                for (int j = 0; j < sensorsThatCoverYLine.Count; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }

                    if (outsideLimitPointLeft.Overlap && outsideLimitPointRight.Overlap)
                    {
                        break;
                    }

                    if (!outsideLimitPointLeft.Overlap && IsPointInsideBoundaries(outsideLimitPointLeft.X, sensorsThatCoverYLine[j].NoDistressLimits[yLine]))
                    {
                        outsideLimitPointLeft.Overlap = true;
                    }

                    if (!outsideLimitPointRight.Overlap && IsPointInsideBoundaries(outsideLimitPointRight.X, sensorsThatCoverYLine[j].NoDistressLimits[yLine]))
                    {
                        outsideLimitPointRight.Overlap = true;
                    }
                }

                if (!outsideLimitPointLeft.Overlap)
                {
                    return outsideLimitPointLeft.X;
                }

                if (!outsideLimitPointRight.Overlap)
                {
                    return outsideLimitPointRight.X;
                }
            }

            return null;
        }

        static bool IsPointInsideBoundaries(int point, (int LowestX, int HighestX) noDistressLimits) => point >= noDistressLimits.LowestX && point <= noDistressLimits.HighestX;

        Dictionary<int, byte> GetLineValues(int yLine)
        {
            var yLineValues = new Dictionary<int, byte>();
            foreach (var sensor in sensors)
            {
                var lowestY = sensor.Position.y - sensor.Distance;
                var highestY = sensor.Position.y + sensor.Distance;
                if (yLine >= lowestY && yLine <= highestY)
                {
                    var diff = Math.Abs(sensor.Position.y - yLine);
                    for (int x = sensor.Position.x - sensor.Distance + diff; x <= sensor.Position.x + sensor.Distance - diff; x++)
                    {
                        var cellValue = GetCellValue(x, yLine);
                        if (cellValue != S && cellValue != B)
                        {
                            yLineValues[x] = NoDistress;
                        }
                        else
                        {
                            yLineValues[x] = cellValue;
                        }
                    }
                }
            }

            return yLineValues;
        }

        static (Dictionary<Point, Sensor>, HashSet<Point>) ReadSensorsAndBeacons(IList<string?> inputLines)
        {
            Dictionary<Point, Sensor> sensors = new();
            HashSet<Point> beacons = new();
            var nonEmptyLines = inputLines.Where(l => !string.IsNullOrEmpty(l)).ToList();
            foreach (var line in nonEmptyLines)
            {
                var numbers = InputReader.GetNumbersFromLine(line!);
                var sensorPosition = new Point(numbers[0], numbers[1]);
                var closestBeacon = new Point(numbers[2], numbers[3]);
                var distance = GetManhattanDistance(sensorPosition, closestBeacon);
                var sensor = new Sensor(sensorPosition, closestBeacon, distance);
                sensors[sensorPosition] = sensor;
                beacons.Add(closestBeacon);
                Console.WriteLine(sensor);
            }

            return (sensors, beacons);
        }

        bool IsSensorOrBeacon(int x, int y)
        {
            var point = new Point(x, y);
            if (sensorsDict.ContainsKey(point))
            {
                return true;
            }
            if (beacons.Contains(point))
            {
                return true;
            }

            return false;
        }

        byte GetCellValue(int x, int y)
        {
            var point = new Point(x, y);
            if (sensorsDict.ContainsKey(point))
            {
                return S;
            }
            if (beacons.Contains(point))
            {
                return B;
            }

            return Dot;
        }

        static int GetManhattanDistance(Point point1, Point point2) => Math.Abs(point1.x - point2.x) + Math.Abs(point1.y - point2.y);
    }

    internal class Sensor
    {
        public Point Position { get; set; }

        public Point ClosestBeacon { get; set; }

        public int Distance { get; set; }

        public Dictionary<int, (int LowestX, int HighestX)> NoDistressLimits { get; set; }

        public Sensor(Point position, Point closestBeacon, int distance)
        {
            Position = position;
            ClosestBeacon = closestBeacon;
            Distance = distance;
            NoDistressLimits = new();
        }

        public override string ToString()
        {
            return $"{Position} Closest beacon:{ClosestBeacon} distance:{Distance}";
        }

        public void CalculateNoDistressLimits((int Min, int Max) distressLimit)
        {
            this.NoDistressLimits = new();
            var lowestY = Math.Max(this.Position.y - this.Distance, distressLimit.Min);
            var highestY = Math.Min(this.Position.y + this.Distance, distressLimit.Max);

            for (int y = lowestY; y <= highestY; y++)
            {
                var diff = Math.Abs(this.Position.y - y);
                var lowestX = Math.Max(this.Position.x - this.Distance + diff, distressLimit.Min);
                var highestX = Math.Min(this.Position.x + this.Distance - diff, distressLimit.Max);
                this.NoDistressLimits[y] = (lowestX, highestX);
            }
        }
    }
}
