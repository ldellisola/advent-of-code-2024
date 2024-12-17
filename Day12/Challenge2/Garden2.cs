using Common;

namespace Day12.Challenge2;

class Region(char Letter, HashSet<Vector> Cells)
{
    public char Letter { get; } = Letter;
    public HashSet<Vector> Cells { get; } = Cells;
    public int Sides = 0;


    private static readonly IEqualityComparer<HashSet<Vector>> Comparer = HashSet<Vector>.CreateSetComparer();
    public override int GetHashCode()
    {
        HashSet<Vector>.CreateSetComparer();
        return Comparer.GetHashCode(Cells);
    }

    public override bool Equals(object? obj)
    {
        if(obj is not Region otherRegion)
            return false;

        return Cells.SetEquals(otherRegion.Cells);
    }
}

public class Garden(char[][] input)
{
    private readonly HashSet<Vector> _visitedPlaces = [];

    public int CalculateFencePrice()
    {
        var allPoints = input.SelectMany((r, ir) => r.Select((c, ic) => new Vector(ir, ic))).ToArray();
        List<Region> regions = [];
        foreach(var point in allPoints.Where(t=> !_visitedPlaces.Contains(t)))
        {
            var region = GetRegion(point).ToHashSet();
            regions.Add(new Region(this[point],region));
        }
        
        var regionDictionary = regions.SelectMany(region=> region.Cells.Select(cell=> (cell,region)))
                                      .ToDictionary(t=> t.cell, t=> t.region);


        for(int row = 0; row < input.Length; row++)
        {
            Region oldRegion = regions[0];
            char previous = this[new(0, 0)];
            
            for(int col = 0; col < input[row].Length; col++)
            {
                var position = new Vector(row,col);
                var currentRegion = regionDictionary[position];

                if(row == 0)
                {
                    if(!Equals(currentRegion, oldRegion))
                    {
                        // above
                        currentRegion.Sides++;
                        // below
                        // if
                        currentRegion.Sides++;
                        oldRegion = currentRegion;
                    }
                    
                }
                
                
            }
        }
        

        return 0;
    }

    private IEnumerable<Vector> GetRegion(Vector startPoint)
    {
        if(!_visitedPlaces.Add(startPoint))
            return [];

        Vector[] nextPositions =
        [
            startPoint + Vector.Down,
            startPoint + Vector.Up,
            startPoint + Vector.Right,
            startPoint + Vector.Left,
        ];
        
        return nextPositions
                .Where(CanMove)
                .Where(t=> this[t] == this[startPoint])
                .SelectMany(GetRegion)
                .Prepend(startPoint);


    }

    private int GetSides(HashSet<Vector> border)
    {
        // get vertical sides
        List<List<Vector>> verticalSides = [];
        var verticalLines = border.GroupBy(t => t.Col).Select(t=> t.ToList()).ToList();

        foreach(var line in verticalLines)
        {
            if(line is [var pos])
            {
                var left = pos + Vector.Left;
                var right = pos + Vector.Right;
                
                if (!border.Contains(left) || !border.Contains(right))
                    verticalSides.Add(line);
                continue;
            }

            var newLine = new List<Vector>();
            Vector? previous = null;
            foreach(var item in line.OrderBy(t => t.Row))
            {
                if(previous == null)
                {
                    previous = item;
                    continue;
                }

                if(previous.Row == item.Row + 1)
                {
                    newLine.Add(item);
                }
                else
                {
                    verticalSides.Add(newLine);
                    newLine = [];
                    previous = null;
                }
            }
            
            if (newLine.Count > 0)
                verticalSides.Add(newLine);
        }
        
        List<List<Vector>> horizontalSides = [];

        var horizontalLines = border.GroupBy(t => t.Row).Select(t=> t.ToList()).ToList();

        foreach(var line in horizontalLines)
        {
            if(line is [var pos])
            {
                var up = pos + Vector.Up;
                var down = pos + Vector.Down;
                
                if (!border.Contains(up) || !border.Contains(down))
                    horizontalSides.Add(line);
                continue;
            }

            var newLine = new List<Vector>();
            Vector? previous = null;
            foreach(var item in line.OrderBy(t => t.Col))
            {
                if(previous == null)
                {
                    previous = item;
                    continue;
                }

                if(previous.Col == item.Col + 1)
                {
                    newLine.Add(item);
                }
                else
                {
                    horizontalSides.Add(newLine);
                    newLine = [];
                    previous = null;
                }
            }
            
            if (newLine.Count > 0)
                horizontalSides.Add(newLine);
        }

        var comparer = HashSet<Vector>.CreateSetComparer();
        return verticalSides.Select(t=> t.ToHashSet()).DistinctBy(t=> comparer.GetHashCode(t)).Count()
            + horizontalSides.Select(t=> t.ToHashSet()).DistinctBy(t=> comparer.GetHashCode(t)).Count();

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