using System.Numerics;

namespace Common;

public class Vector(int row, int col) : IAdditionOperators<Vector,Vector,Vector>
{ 
    public static readonly Vector Up = new(row: -1, col: 0);
    public static readonly Vector Down = new(row: 1, col: 0);
    public static readonly Vector Left = new(row: 0, col: -1);
    public static readonly Vector Right = new(row: 0, col: 1);

    public int Row => row;
    public int Col => col;
    
    
    public static implicit operator Vector((int, int) tuple) 
        => new(tuple.Item1, tuple.Item2);
    
    public static Vector operator +(Vector left, Vector right)
    {
        return new Vector(left.Row + right.Row, left.Col + right.Col);
    }

    public int Distance(Vector other)
    {
        return Math.Abs(other.Row - Row) + Math.Abs(other.Col - Col);
    }
    
    public Vector GetDirection( Vector to)
    {
        return (Row.CompareTo(to.Row), Col.CompareTo(to.Col)) switch
        {
            (0, 0) => (0, 0),
            (_, > 0) => Left,
            (< 0, _) => Down,
            (> 0, _) => Up,
            (_, < 0) => Right,

            
        };
    }

    public Vector RotateLeft()
    {
        return this switch
        {
            (-1, 0) => Left,
            (1, 0) => Right,
            (0, -1) => Down,
            (0, 1) => Up,
            _ => throw new InvalidOperationException("Only scalars are supported")
        };
    }
    public Vector RotateRight()
    {
        return this switch
        {
            (-1, 0) => Right,
            (1, 0) => Left,
            (0, -1) => Up,
            (0, 1) => Down,
            _ => throw new InvalidOperationException("Only scalars are supported")
        };    
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

    public static bool operator ==(Vector v1, Vector v2)
    {
        return v1.Equals(v2);
    }
    public static bool operator !=(Vector v1, Vector v2)
    {
        return !v1.Equals(v2);
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
