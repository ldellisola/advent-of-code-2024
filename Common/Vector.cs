using System.Numerics;

namespace Common;
public class Vector(int row, int col) : IAdditionOperators<Vector,Vector,Vector>
{
    public int Row => row;
    public int Col => col;
    
    public static Vector operator +(Vector left, Vector right)
    {
        return new Vector(left.Row + right.Row, left.Col + right.Col);
    }

    public void Deconstruct(out int row, out int col)
    {
        row = Row;
        col = Col;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Vector v)
            return false;
        return Row == v.Row && Col == v.Col;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }

    public override string ToString()
    {
        return $"({Row}, {Col})";
    }
    
    public static readonly Vector Left = new(0, -1);
    public static readonly Vector Right = new(0, 1);
    public static readonly Vector Up = new(-1, 0);
    public static readonly Vector Down = new(1, 0);
}