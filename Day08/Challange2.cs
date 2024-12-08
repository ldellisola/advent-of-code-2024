using System.Text;

namespace Day08.Challange2;

public class Map(char[][] layout, char frequency)
{
    
    public IEnumerable<(int row, int col)> FindAntiNodes()
    {
        var antennas = FindAntennas().ToArray();
        var antennaPair = antennas.SelectMany(a1 => antennas.Select(a2 => (a1, a2)))
            .Where(t => t.Item1 != t.Item2)
            .ToArray();

        foreach (var (antenna1, antenna2) in antennaPair)
        {
            var harmonics = GetResonantHarmonics(antenna1, antenna2);
            
            // look forward
            var position = antenna1;
            while (InMap(position))
            {
                position.col += harmonics.col;
                position.row += harmonics.row;
                if (InMap(position) && (layout[position.row][position.col] != frequency || (position == antenna1 || position == antenna2) ))
                {
                    yield return position;
                }
            }
            // look backwards
            position = antenna1;
            while (InMap(position))
            {
                position.col -= harmonics.col;
                position.row -= harmonics.row;
                if (InMap(position) && (layout[position.row][position.col] != frequency || (position == antenna1 || position == antenna2) ))
                {
                    yield return position;
                }
            }
        }
    }

    private bool InMap((int row, int col) position)
    {
        return position.row >= 0 && position.row < layout.Length && position.col >= 0 && position.col < layout[0].Length;
    }

    private static (int row, int col) GetResonantHarmonics((int row, int col) antenna1, (int row, int col) antenna2)
    {
        var horizontal = antenna1.col - antenna2.col;
        var vertical = antenna1.row - antenna2.row;
        return (vertical, horizontal);
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
    

    public override string ToString()
    {
       var antiNodes = FindAntiNodes().Distinct().ToArray();
       var bld = new StringBuilder();

       bld.AppendLine($"Frequency: {frequency}");
       bld.AppendLine($"AntiNodes: {antiNodes.Length}");
       
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