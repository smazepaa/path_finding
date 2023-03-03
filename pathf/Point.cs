namespace pathf;

public struct Point
{
    public int Column { get; }
    public int Row { get; }

    public Point(int column, int row)
    {
        Column = column;
        Row = row;
    }
    
    public bool Equals(Point other)
    {
        return Column == other.Column && Row == other.Row;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Column, Row);
    }
}