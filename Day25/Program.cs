var content = File.ReadAllText("input.txt");

List<int[]> Keys = [];
List<int[]> Locks = [];

HashSet<(int[] Key, int[] Lock)> validCombinations = [];

Parse(content);

foreach(var key in Keys)
{
    foreach(var @lock in Locks)
    {
        bool foundMatch = true;
        for(int i = 0; i < 5; i++)
        {
            if(key[i] + @lock[i] > 5)
            {
                foundMatch = false;
                break;
            }
        }
        
        if (foundMatch)
            validCombinations.Add((key,  @lock));
            
    }
}


Console.WriteLine(validCombinations.Count);






void Parse(string raw)
{
    foreach(var a in raw.Split($"{Environment.NewLine}{Environment.NewLine}",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    {
        if (a[0] == '#')
            Locks.Add(ParseLock(a));
        else
            Keys.Add(ParseKey(a));
    }
    
}

int[] ParseLock(string raw)
{
    int[] @lock = [0, 0, 0, 0, 0];
    var map = raw.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                 .Select(t=> t.ToCharArray())
                 .ToArray();

    for(int col = 0; col < @lock.Length; col++)
    {
        for(int row = 1; row < 6; row++)
        {
            if(map[row][col] == '#')
                @lock[col] += 1;
            else
                break;
            
        }
        
    }
    return @lock;
}

int[] ParseKey(string raw)
{
    int[] key = [0, 0, 0, 0, 0];
    var map = raw.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                 .Select(t=> t.ToCharArray())
                 .ToArray();

    for(int col = 0; col < key.Length; col++)
    {
        for(int row = 5; row >= 1; row--)
        {
            if(map[row][col] == '#')
                key[col] += 1;
            else
                break;
            
        }
        
    }
    return key;
}