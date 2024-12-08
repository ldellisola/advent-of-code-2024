using System.Text;

namespace Day08.Challange1;

public class Map(char[][] layout, char frequency)
{
    
    public IEnumerable<(int row, int col)> FindAntiNodes()
    {
        var antennas = FindAntennas().ToArray();
        
        foreach (var (iRow, row) in layout.Index())
        {
            foreach (var (iCol, col) in row.Index())
            {
                if (col == frequency)
                    continue;
                if (antennas.Any(a1 => antennas.Any(a2 => CreatesInterferance((iRow,iCol), a1, a2))))
                    yield return (iRow, iCol);
            }
        }
    }

    private static bool CreatesInterferance((int row, int col) position, (int row, int col) antenna1, (int row, int col) antenna2)
    {
        if (antenna1 == antenna2 || antenna1 == position || antenna2 == position)
            return false;
        
        var distanceTo1 = Distance(position, antenna1);
        var distanceTo2 = Distance(position, antenna2);
        var distanceBetweenAntennas = Distance(antenna1, antenna2);

        var antenna1IsCloser = distanceTo2 == distanceTo1 * 2;
        var antenna2IsCloser = distanceTo1 == distanceTo2 * 2;


        if (antenna1IsCloser)
        {
            if (distanceBetweenAntennas != distanceTo1)
                return false;
            
            var horizontalDistanceTo1 = Math.Abs(position.col - antenna1.col);
            var verticalDistanceTo1 = Math.Abs(position.row - antenna1.row);
        
        
            var horizontalDistanceBetweenAntennas = Math.Abs(antenna1.col - antenna2.col);
            var verticalDistanceBetweenAntennas = Math.Abs(antenna1.row - antenna2.row);
            
            return horizontalDistanceTo1 == horizontalDistanceBetweenAntennas && verticalDistanceTo1 == verticalDistanceBetweenAntennas;
            
        }

        if (antenna2IsCloser)
        {
            if (distanceBetweenAntennas != distanceTo2)
                return false;
        
            var horizontalDistanceTo2 = Math.Abs(position.col - antenna2.col);
            var verticalDistanceTo2 = Math.Abs(position.row - antenna2.row);
        
            var horizontalDistanceBetweenAntennas = Math.Abs(antenna1.col - antenna2.col);
            var verticalDistanceBetweenAntennas = Math.Abs(antenna1.row - antenna2.row);
            
            return horizontalDistanceTo2 == horizontalDistanceBetweenAntennas && verticalDistanceTo2 == verticalDistanceBetweenAntennas;
        }


        return false;
    }
    
    private IEnumerable<(int row, int col)> FindAntennas()
    {
        foreach (var (iRow, row) in layout.Index())
        {
            foreach (var (iCol, col) in row.Index())
            {
                if (col == frequency)
                    yield return (iRow, iCol);
            }
        }
    }

    private static int Distance((int row, int col) a, (int row, int col) b)
    {
        return Math.Abs(a.row - b.row) + Math.Abs(a.col - b.col);
    }

    public override string ToString()
    {
       var antiNodes = FindAntiNodes().ToArray();
       var bld = new StringBuilder();
       
       foreach (var (iRow, row) in layout.Index())
       {
           foreach (var (iCol, col) in row.Index())
           {
               if (antiNodes.Contains((iRow, iCol)))
                   bld.Append('#');
               else
                   bld.Append(col);
           }
           bld.AppendLine();
       }
       
       return bld.ToString();
    }
}