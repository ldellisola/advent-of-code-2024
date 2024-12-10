namespace Day09.Challenge1;

public class DiskMap
{

    private readonly List<int> _files = [];
    
    public DiskMap(string input)
    {
        int id = 0;
        for(int i = 0; i < input.Length; i++)
        {
            var bytes = input[i] - '0';
            var isEmpty = i % 2 == 1;
            if(isEmpty)
            {
                _files.AddRange(Enumerable.Range(0, bytes).Select(_ => -1));
            }
            else
            {
                _files.AddRange(Enumerable.Range(0, bytes).Select(_ => id));
                id++;
            }
        }
    }


    public void DefragDisk()
    {
        for(int i = _files.Count - 1; i >= 0; i--)
        {
            if (_files[i] == -1)
                continue;
            
            var firstEmpty = _files.IndexOf(-1);
            if (firstEmpty == -1 || firstEmpty >= i)
                return;
            _files[firstEmpty] = _files[i];
            _files[i] = -1;
        }
    }


    public double CalculateChecksum()
    {
        double result = 0;
        foreach(var (i, id) in _files.TakeWhile(t=> t != -1).Index())
        {
            result += i * id;
        }

        return result;
    }
}

