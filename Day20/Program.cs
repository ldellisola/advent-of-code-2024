using Day20.Challenge2;

var input = File.ReadAllLines("input.txt").Select(t=> t.ToCharArray()).ToArray();

var raceTrack = new RaceTrack(input);

Console.WriteLine(raceTrack.CountCheats());