using Common;

namespace Day12.Challenge1;

public class Garden(char[][] input)
{
    private readonly HashSet<Vector> _visitedPlaces = [];

    public int CalculateFencePrice()
    {

        var allPoints = input.SelectMany((r, ir) => r.Select((c, ic) => new Vector(ir, ic))).ToArray();

        var result = 0;
        foreach(var point in allPoints)
        {
            var (area,perimeter) = AnalizeRegion(point, 0, 0);
            result += area * perimeter;
        }

        return result;
    }

    public (int area, int perimeter) AnalizeRegion(Vector point, int area, int perimeter)
    {
        if (!_visitedPlaces.Add(point))
            return (area: 0, perimeter: 0);

        var plant = this[point];

        int newArea = area + 1;
        int newPerimeter = perimeter;
        
        var positionUp = point + Vector.Up;
        if(CanMove(positionUp) && this[positionUp] == plant)
        {
            var result = AnalizeRegion(positionUp, area, perimeter);
            newArea += result.area;
            newPerimeter += result.perimeter;
        }
        else
        {
            newPerimeter += 1;
        }
        
        var positionDown = point + Vector.Down;
        if(CanMove(positionDown) && this[positionDown] == plant)
        {
            var result = AnalizeRegion(positionDown, area, perimeter);
            newArea += result.area;
            newPerimeter += result.perimeter;
        }
        else
        {
            newPerimeter += 1;
        }
        
        var positionLeft = point + Vector.Left;
        if(CanMove(positionLeft) && this[positionLeft] == plant)
        {
            var result = AnalizeRegion(positionLeft, area, perimeter);
            newArea += result.area;
            newPerimeter += result.perimeter;
        }
        else
        {
            newPerimeter += 1;
        }
        
        var positionRight = point + Vector.Right;
        if(CanMove(positionRight) && this[positionRight] == plant)
        {
            var result = AnalizeRegion(positionRight, area, perimeter);
            newArea += result.area;
            newPerimeter += result.perimeter;
        }
        else
        {
            newPerimeter += 1;
        }
            
     
        return (area: newArea, perimeter: newPerimeter);
    }
    
    private bool CanMove(Vector position)
    {
        return position.Row >= 0 && position.Row <= input.Length - 1 &&
               position.Col >= 0 && position.Col <= input[0].Length - 1;
    }
    
    public char this[Vector v]
    {
        get
        {
            var (row, col) = v;
            return input[row][col];
        }
        set
        {
            var (row, col) = v;
            input[row][col] = value;
        }
    }
}