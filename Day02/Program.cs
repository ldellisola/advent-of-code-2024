
var reports = File.ReadLines("input.txt").ToArray();

Console.WriteLine("Challenge 1:");

var unsafeReports = reports
    .AsParallel()
    .Count(IsSafe);

Console.WriteLine(unsafeReports);

static bool IsSafe(string line)
{
    var data = line.Split(" ").Select(int.Parse).ToArray();
    var data2 = data.Skip(1);
    var zippedData = data.Zip(data2).ToArray();
   
    return (zippedData.All(t=> t.First < t.Second) || zippedData.All(t=> t.Second < t.First))
        && zippedData.All(t=> Math.Abs(t.First - t.Second) <= 3);
}



Console.WriteLine("--------------");
Console.WriteLine("Challenge 2:");

var unsafeReports2 = reports
    .AsParallel()
    .Count(IsSafe2);

Console.WriteLine(unsafeReports2);

static bool IsSafe2(string line)
{
    var data = line.Split(" ").Select(int.Parse).ToArray();
    var data2 = data.Skip(1);
    var zippedData = data.Zip(data2).ToArray();
   
    var safe = (zippedData.All(t=> t.First < t.Second) || zippedData.All(t=> t.Second < t.First))
           && zippedData.All(t=> Math.Abs(t.First - t.Second) <= 3);
    
    if (safe) return true;
    
    // Handle the dampener
    for (int i = 0; i < data.Length; i++)
    {
        var newdata = data.Where((_,index) => index != i).ToArray();
        data2 = newdata.Skip(1);
        zippedData = newdata.Zip(data2).ToArray();
   
        safe = (zippedData.All(t=> t.First < t.Second) || zippedData.All(t=> t.Second < t.First))
                   && zippedData.All(t=> Math.Abs(t.First - t.Second) <= 3);
        if (safe) return true;
    }

    return false;
}

