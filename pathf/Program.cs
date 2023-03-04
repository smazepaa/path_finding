using pathf;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Seed = 10,
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
    var time = new Dictionary<Point, float>();
    

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
            var newDistance = distances[current] + 1;
            if (!origins.ContainsKey(next) || newDistance < distances[next])
            {
                distances[next] = newDistance;
                var priority = newDistance + Manhattan(goal, next);
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


    return neighbours;
}
Console.WriteLine(path.Count);