using Day09.Challenge2;

var input = File.ReadAllText("input.txt");

var disk = new DiskMap(input);
disk.DefragDisk();
Console.WriteLine("Challenge 2:");
Console.WriteLine(disk.CalculateChecksum());