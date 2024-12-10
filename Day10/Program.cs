using Day10.Challenge2;

var input = File.ReadLines("input.txt")
                .Select(t=> t.ToCharArray())
                .ToArray();
                
                
var map = new TopographicMap(input);

var result = map.FindTrailHeadScore();
Console.WriteLine(result);