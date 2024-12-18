using Day18.Challenge2;

var input = File.ReadAllLines("input.txt");


var i = Map.FindBlocker(71, 71, input);
Console.WriteLine(input[i]);