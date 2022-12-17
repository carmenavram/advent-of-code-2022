namespace AdventOfCode2022;

internal class Day14 : IDay
{
    public void Solve(IList<string?> inputLines)
    {
        var result = 0;
        var resultSecondPart = 0;
        var nonEmptyLines = inputLines.Where(l => !string.IsNullOrEmpty(l)).ToList();
        (var heightMap, var destination, var origin) = CreateHeightMap(nonEmptyLines!);

        SortedSet<int> paths = new();
        var originNextSquares = new List<Point>();
        var destinationReached = false;
        var processed = new Stack<Point>();
        FindPaths(origin, 0);

        void FindPaths(Point origin, int numberOfSteps)
        {
            processed.Push(origin);
            Console.WriteLine($"processed {origin}");
            var nextSquares = GetNextSquares(heightMap, origin, processed, numberOfSteps);
            foreach (var square in nextSquares)
            {
                if (square.Point.Equals(destination))
                {
                    paths.Add(square.NumberOfSteps);
                    Console.WriteLine($"destination reached {square.Point} {square.NumberOfSteps}");
                }
                else
                {
                    FindPaths(square.Point, square.NumberOfSteps);
                    Console.WriteLine($"out of find paths {square.Point} {square.NumberOfSteps}");
                    processed.Pop();
                }
                //processed = ResetProcessedExcept(origin, processed);
            }
        }

        //if (originNextSquares.Any())
        //{
        //    var originNextSquare = originNextSquares[0];
        //    var next = originNextSquare;
        //    var nextSquares = new List<Point>();
        //    do
        //    {
        //        processed.Enqueue(next);
        //        numberOfSteps++;
        //        if (next.Equals(destination))
        //        {
        //            destinationReached = true;
        //            paths.Add(numberOfSteps);
        //        }
        //        else
        //        {
        //            nextSquares = GetNextSquares(heightMap, next, processed);
        //            next = nextSquares.FirstOrDefault();
        //        }
        //    }
        //    while (nextSquares.Any() && !destinationReached);

        //    processed = ResetProcessedExcept(originNextSquare, processed);
        //}

        //do
        //{
        //    destinationReached = false;
        //    var numberOfSteps = 0;
        //    originNextSquares = GetNextSquares(heightMap, origin, processed);
        //    processed.Enqueue(origin);

        //    if (originNextSquares.Any())
        //    {
        //        var originNextSquare = originNextSquares[0];
        //        var next = originNextSquare;
        //        var nextSquares = new List<Point>();
        //        do
        //        {
        //            processed.Enqueue(next);
        //            numberOfSteps++;
        //            if (next.Equals(destination))
        //            {
        //                destinationReached = true;
        //                paths.Add(numberOfSteps);
        //            }
        //            else
        //            {
        //                nextSquares = GetNextSquares(heightMap, next, processed);
        //                next = nextSquares.FirstOrDefault();
        //            }
        //        }
        //        while (nextSquares.Any() && !destinationReached);

        //        processed = ResetProcessedExcept(originNextSquare, processed);
        //    }
        //}
        //while (originNextSquares.Any());

        result = paths.Min;
        Console.WriteLine($"Day 12 result part 1: {result} {destinationReached}");
        Console.WriteLine($"Day 12 result part 2: {resultSecondPart}");

        static (Dictionary<Point, Value> Map, Point Destination, Point Origin) CreateHeightMap(List<string> lines)
        {
            var map = new Dictionary<Point, Value>();
            var destination = new Point(-1, -1);
            var origin = new Point(-1, -1);

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                destination = destination.x == -1 ? new Point(line.IndexOf("E"), i) : destination;
                origin = origin.x == -1 ? new Point(line.IndexOf("S"), i) : origin;
                line = line.Replace("E", "z").Replace("S", "a");
                int j = 0;
                foreach (var elevation in line)
                {
                    var coordinate = new Point(j++, i);
                    map[coordinate] = new Value(elevation, false);
                }
            }

            return (map, destination, origin);
        }

        static List<(Point Point, int NumberOfSteps)> GetNextSquares(Dictionary<Point, Value> heightMap, Point current, IEnumerable<Point> processed, int numberOfSteps)
        {
            var result = new List<(Point, int)>();
            var left = new Point(current.x - 1, current.y);
            var right = new Point(current.x + 1, current.y);
            var top = new Point(current.x, current.y - 1);
            var bottom = new Point(current.x, current.y + 1);

            AddNeighbour(heightMap, current, result, left, processed, numberOfSteps);
            AddNeighbour(heightMap, current, result, right, processed, numberOfSteps);
            AddNeighbour(heightMap, current, result, top, processed, numberOfSteps);
            AddNeighbour(heightMap, current, result, bottom, processed, numberOfSteps);

            return result;

            static void AddNeighbour(Dictionary<Point, Value> heightMap, Point current, List<(Point, int)> result, Point neighbour, IEnumerable<Point> processed, int numberOfSteps)
            {
                if (CanGoToNext(heightMap, current, neighbour, processed))
                {
                    result.Add((neighbour, ++numberOfSteps));
                }
            }
        }

        static Queue<Point> ResetProcessedExcept(Point except, Queue<Point> processed)
        {
            var found = false;
            var newQueue = new Queue<Point>();

            while (processed.Any() && !found)
            {
                var current = processed.Dequeue();
                newQueue.Enqueue(current);
                found = current.Equals(except);
            }

            return newQueue;

            //foreach (var key in heightMap.Keys)
            //{
            //    heightMap[key] = new Value(heightMap[key].elevation, false);
            //}
            //heightMap[except] = new Value(heightMap[except].elevation, true);
        }

        static bool CanGoToNext(Dictionary<Point, Value> heightMap, Point current, Point next, IEnumerable<Point> processed) =>
            (!processed.Contains(next) && heightMap.ContainsKey(next) && (heightMap[next].elevation - heightMap[current].elevation <= 1));
    }
}
