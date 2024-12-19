using Day19.Challenge2;

var input = File.ReadAllText("input.txt");

var tc = new TowelCounter(input);


Console.WriteLine(tc.CountPossibleTowels());