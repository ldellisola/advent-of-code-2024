using System.Text;

namespace Day09.Challenge2;

public class DiskMap(string input)
{
    private readonly List<int[]> _files = ReadDisk(input).ToList();

    public void DefragDisk()
    {
        var count = 0;
        while(count <= _files.Count && count != -1)
        {
            count = foo(count);
        }
    }

    private int foo(int startFrom)
    {
        var files = _files.Index().SkipLast(startFrom).ToList();
        var (i, file) = files.LastOrDefault(t => t.Item[0] != -1);

        if(file is null)
            return -1;

        int firstEmpty = -1;
        do
        {
            
            firstEmpty = _files.FindIndex(firstEmpty + 1, t => t[0] == -1);
            if(firstEmpty == -1 || firstEmpty >= i)
                break;

            if(_files[firstEmpty].Length == _files[i].Length)
            {
                (_files[firstEmpty], _files[i]) = (_files[i], _files[firstEmpty]);
                return startFrom + 1;
            }

            if(_files[firstEmpty].Length > _files[i].Length)
            {
                var totalEmpty = _files[firstEmpty].Length;
                var difference = _files[firstEmpty].Length - _files[i].Length;
                _files[firstEmpty] = _files[i];
                _files[i] = Enumerable.Range(0, totalEmpty - difference).Select(t => -1).ToArray();
                _files.Insert(firstEmpty + 1, Enumerable.Range(0, difference).Select(t => -1).ToArray());
                return startFrom +1;
            }
        }
        while(firstEmpty < i);

        return startFrom + 1;
    }

    private static IEnumerable<int[]> ReadDisk(string input)
    {
        int id = 0;
        for(int i = 0; i < input.Length; i++)
        {
            var bytes = input[i] - '0';
            if(bytes == 0)
                continue;
            var isEmpty = i % 2 == 1;
            if(isEmpty)
            {
                yield return Enumerable.Range(0, bytes).Select(_ => -1).ToArray();
            }
            else
            {
                yield return Enumerable.Range(0, bytes).Select(_ => id).ToArray();
                id++;
            }
        }
    }

    public double CalculateChecksum()
    {
        double result = 0;
        int index = 0;
        foreach(var file in _files)
        {
            foreach(var @byte in file)
            {
                if (@byte != -1)
                    result += @byte * index;
                index++;
            }
        }

        return result;
    }

    public override string ToString()
    {
        var bld = new StringBuilder();

        foreach(var file in _files)
        {
            foreach(var @byte in file)
            {
                if (@byte != -1)
                    bld.Append($"{@byte}");
                else 
                    bld.Append('.'); }
        }
        
        return bld.ToString();
    }
}