using pathf;

// generating the map
var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Seed = 10,
    Noise = .1f,
    AddTraffic = true,
    TrafficSeed = 1234
});

var startPoint = new Point(0, 0);
var endPoint = new Point(16, 30);

List<float> times = new List<float>();
string[,] map = generator.Generate();
var path = GetShortestPath(map, startPoint, endPoint);
new MapPrinter().Print(map, path);


List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    // creating list of points of the path
    List<Point> shortestPath = new List<Point>();
    shortestPath.Add(start);
    
    var distances = new Dictionary<Point, int>(); // dict with distances between points (weight of each edge)
    var origins = new Dictionary<Point, Point>(); // create to find later its neighbours (from which we go)
    var velocities = new Dictionary<Point, float>();


    var frontier = new PriorityQueue<Point, int>(); 
    frontier.Enqueue(start, 0);
    origins[start] = start; // to remember point (0;0)
    distances[start] = 0;

    Point current = default;
    while (frontier.Count != 0)
    {
        current = frontier.Dequeue();
        if (current.Equals(goal))
        {
            break;
        }
        var neighbours = GetNeighbours(map, current);
        foreach (var next in neighbours)
        {
            var n = Convert.ToInt32(map[next.Column, next.Row]);
            
            var newDistance = distances[current] + n;
            if (!origins.ContainsKey(next) || newDistance < distances[next])
            {
                velocities[next] = 60 - (n - 1) * 6;
                times.Add(1/velocities[next]);
                distances[next] = newDistance;
                var priority = newDistance;
                frontier.Enqueue(next, priority);
                origins[next] = current;
            }
        }
    }

    var curr = current;
    while (!curr.Equals(start))
    {
        shortestPath.Add(curr);
        curr = origins[curr];
    }
    
    shortestPath.Add(goal);
    return shortestPath;
}

List<Point> GetNeighbours(string[,] map, Point point)
{
    List<Point> neighbours = new List<Point>();
    var width = map.GetLength(0);
    var height = map.GetLength(1);
    
    if (point.Column - 1 != -1) // if we go left, !=-1 so we don't go outside the map
    {
        if (map[point.Column - 1, point.Row] != "█") // if it's not a wall
        {
            neighbours.Add(new Point(point.Column - 1, point.Row));
        }
    }

    if (point.Row - 1 != -1)
    {
        if (map[point.Column, point.Row - 1] != "█")
        {
            neighbours.Add(new Point(point.Column, point.Row - 1));
        }
    }

    if (point.Column + 1 >= 0 && point.Column + 1 < width && point.Row < height)
    {
        if (map[point.Column + 1, point.Row] != "█")
        {
            neighbours.Add(new Point(point.Column + 1, point.Row));
        }    
    }

    if (point.Row + 1 >= 0 && point.Row + 1 < height && point.Column < width)
    {
        if (map[point.Column, point.Row + 1] != "█")
        {
            neighbours.Add(new Point(point.Column, point.Row + 1));
        }            
    }

    foreach (var n in neighbours)
    {
        if (map[n.Column, n.Row] == "B")
        {
            Console.WriteLine("B");
        }
    }

    return neighbours;
}


float totalTime = times.Sum();
Console.WriteLine();
Console.WriteLine("total travel time:" + Math.Round(totalTime, 2) + "hours");