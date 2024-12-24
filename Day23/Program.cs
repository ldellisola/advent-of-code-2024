using Day23;

var input = File.ReadAllLines("input.txt");

var computers = Load(input);

var result = FindCoputerGroups(computers, 3);

foreach(var r in result)
{
    Console.WriteLine(string.Join(',',r.Select(t=> t.Id)));
}


return;


HashSet<Computer>[] FindCoputerGroups(HashSet<Computer> computers, int groupSize)
{
    List<HashSet<Computer>> groups = [];

    foreach(var source in computers)
    {
        if (source.Connections.Count < groupSize)
            continue;
        
        if (!source.Id.Contains('t'))
            continue;
        
        groups.AddRange(GetGroups(source, groupSize));
    }
    
    return groups.ToArray();
}


HashSet<Computer>[] GetGroups(Computer computer, int groupSize)
{
    List<HashSet<Computer>> groups = [];

    foreach(var source in computer.Connections)
    {
        var combinations = source.Connections.GetCombinationsList(groupSize-1)
                                 .Where(t=> t.Length == groupSize-1)
                                               .Select(t=> new HashSet<Computer>([..t, source]))
                                               ;
        foreach(var combination in combinations)
        {
            bool isValid = true;
            foreach(var c in combination)
            {
                if(!c.Connections.IsSupersetOf(combination.Except([c])))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
                groups.Add(combination);
        }
    }
    
    
    return groups.ToArray();
}


HashSet<Computer> Load(string[] input)
{
    var computers = new Dictionary<string, Computer>();
    foreach(var connection in input)
    {
        var linkedComputers = connection.Split("-");
        var computer1Name = linkedComputers[0];
        var computer2Name = linkedComputers[1];

        if(!computers.TryGetValue(computer1Name, out var computer1))
        {
            computer1 = new Computer(computer1Name, []);
            computers.Add(computer1Name, computer1);
        }

        if(!computers.TryGetValue(computer2Name, out var computer2))
        {
            computer2 = new Computer(computer2Name, []);
            computers.Add(computer2Name, computer2);
        }

        computer1.Connections.Add(computer2);
        computer2.Connections.Add(computer1);
    }

    return computers.Values.ToHashSet();
}

public record Computer(string Id, HashSet<Computer> Connections);