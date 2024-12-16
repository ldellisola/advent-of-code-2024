using System.Numerics;

namespace Day15.Challenge1;

public class Vector(int row, int col) : IAdditionOperators<Vector,Vector,Vector>
{ 
    public static readonly Vector Up = new(row: -1, col: 0);
    public static readonly Vector Down = new(row: 1, col: 0);
    public static readonly Vector Left = new(row: 0, col: -1);
    public static readonly Vector Right = new(row: 0, col: 1);

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
}