using pathf;


var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Seed = 11
});

var startPoint = new Point(0, 0);
var endPoint = new Point(33, 88);

string[,] map = generator.Generate();
var path = GetShortestPath(map, startPoint, endPoint);
new MapPrinter().Print(map, path);



List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    List<Point> shortestPath = new List<Point>();
    shortestPath.Add(start);
    
    var distances = new Dictionary<Point, int>();
    var origins = new Dictionary<Point, Point>();

    var frontier = new PriorityQueue<Point, int>();
    frontier.Enqueue(start, 0);
    origins[start] = start;
    distances[start] = 0;
    
    while (frontier.Count != 0)
    {
        var current = frontier.Dequeue();
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
                var priority = newDistance;
                frontier.Enqueue(next, priority);
                origins[next] = current;
            }    
        }
    }

    var curr = new Point(32, 33);
    while (!curr.Equals(start))
    {
        shortestPath.Add(curr);
        curr = origins[curr];
    }
    
    shortestPath.Add(goal);
    return shortestPath;
    // your code here
}

List<Point> GetNeighbours(string[,] map, Point point)
{
    List<Point> neighbours = new List<Point>();
    var width = map.GetLength(0);
    var height = map.GetLength(1);
    
    if (point.Column - 1 != -1)
    {
        if (map[point.Column - 1, point.Row] != "█")
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

    if (point.Column + 1 >= 0 && point.Column + 1 < map.GetLength(0) - 1 && point.Row <= map.GetLength(0) - 1)
    {
        if (map[point.Column + 1, point.Row] != "█")
        {
            neighbours.Add(new Point(point.Column + 1, point.Row));
        }    
    }

    if (point.Row + 1 >= 0 && point.Row + 1 < map.GetLength(1) - 1 && point.Column <= map.GetLength(0) - 1)
    {
        if (map[point.Column, point.Row + 1] != "█")
        {
            neighbours.Add(new Point(point.Column, point.Row + 1));
        }            
    }

    return neighbours;
}

//Console.WriteLine(map.GetLength(0));
//Console.WriteLine(map.GetLength(1));