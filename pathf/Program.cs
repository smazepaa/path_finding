using pathf;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Seed = 16,
    AddTraffic = true,
    TrafficSeed = 1234
});

var startPoint = new Point(0, 0);
var endPoint = new Point(16, 30);

string[,] map = generator.Generate();
var path = GetShortestPath(map, startPoint, endPoint);
new MapPrinter().Print(map, path);


List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    List<Point> shortestPath = new List<Point>();
    shortestPath.Add(start);
    
    var distances = new Dictionary<Point, int>(); // dict with distances between points (weight of each edge)
    var origins = new Dictionary<Point, Point>(); // create to find later its neighbours (from which we go)
    
    var frontier = new PriorityQueue<Point, int>(); 
    frontier.Enqueue(start, 0);
    origins[start] = start;
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
                distances[next] = newDistance;
                var priority = newDistance + Geometrical(goal, next);
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

int Manhattan(Point goal, Point next)
{
    return Math.Abs(goal.Row - next.Row) + Math.Abs(goal.Column - next.Column);
}

int Geometrical(Point goal, Point next)
{
    return (goal.Row - next.Row) ^ 2 + (goal.Column - next.Column) ^ 2;
}

List<Point> GetNeighbours(string[,] map, Point point)
{
    List<Point> neighbours = new List<Point>();
    var width = map.GetLength(0);
    var height = map.GetLength(1);
    if (point.Column - 1 != -1) // west
    {
        if (map[point.Column - 1, point.Row] != "█") 
        {
            neighbours.Add(new Point(point.Column - 1, point.Row));
        }
    }

    if (point.Column - 1 != -1 && point.Row - 1 != -1) // north-west
    {
        if (map[point.Column - 1, point.Row - 1 ] != "█") 
        {
            neighbours.Add(new Point(point.Column - 1, point.Row - 1));
        }
    }
    
    if (point.Column - 1 != -1 && point.Row + 1 < height) // south-west
    {
        if (map[point.Column - 1, point.Row + 1 ] != "█") 
        {
            neighbours.Add(new Point(point.Column - 1, point.Row + 1));
        }
    }

    if (point.Row - 1 != -1) // north
    {
        if (map[point.Column, point.Row - 1] != "█")
        {
            neighbours.Add(new Point(point.Column, point.Row - 1));
        }
    }

    if (point.Column + 1 >= 0 && point.Column + 1 < width && point.Row < height) // east
    {
        if (map[point.Column + 1, point.Row] != "█")
        {
            neighbours.Add(new Point(point.Column + 1, point.Row));
        }    
    }

    if (point.Row + 1 >= 0 && point.Row + 1 < height && point.Column < width) // south
    {
        if (map[point.Column, point.Row + 1] != "█")
        {
            neighbours.Add(new Point(point.Column, point.Row + 1));
        }            
    }
    
    if (point.Row + 1 < height && point.Column + 1 < width) // south-east
    {
        if (map[point.Column + 1, point.Row + 1] != "█")
        {
            neighbours.Add(new Point(point.Column + 1, point.Row + 1));
        }
    }
    
    if (point.Row - 1 != -1 && point.Column + 1 < width) // north-east
    {
        if (map[point.Column + 1, point.Row - 1] != "█")
        {
            neighbours.Add(new Point(point.Column + 1, point.Row - 1));
        }
    }
    
    return neighbours;
}