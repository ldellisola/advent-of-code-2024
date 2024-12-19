using Day16.Challenge2;

var input = File.ReadAllLines("input.txt").Select(t=> t.ToCharArray()).ToArray();

var maze = new Maze(input,0);

var shortestPath = maze.CalculateRun();

Console.WriteLine(shortestPath);