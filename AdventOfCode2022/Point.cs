namespace AdventOfCode2022;

internal struct Point : IEquatable<Point>
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Point other) => this.x == other.x && this.y == other.y;

    public override string ToString() => $"{this.x} {this.y}";
}
