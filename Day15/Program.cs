using Day15.Challenge2;

var input = File.ReadAllText("input.txt");

var wareHouse = new WareHouse(input);

wareHouse.RunRobotToEnd();

Console.WriteLine(wareHouse.ToString());
Console.WriteLine(wareHouse.CalculateBoxesScore());
