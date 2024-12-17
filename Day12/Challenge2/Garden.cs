// using Common;
//
// namespace Day12.Challenge2;
//
// public class Garden(char[][] input)
// {
//     private readonly HashSet<Vector> _visitedPlaces = [];
//
//     public int CalculateFencePrice()
//     {
//
//         var allPoints = input.SelectMany((r, ir) => r.Select((c, ic) => new Vector(ir, ic))).ToArray();
//
//         var result = 0;
//         foreach(var point in allPoints.Where(t=> !_visitedPlaces.Contains(t)))
//         {
//             var area = 0;
//             var sides = 0;
//             HashSet<Vector> seenSides = [];
//             AnalizeRegion(point,seenSides, ref area, ref sides);
//             sides = GetSides(seenSides);
//             Console.WriteLine($"Plant {this[point]}. Area: {area}, Sides: {sides}. Total Fence Price: {area*sides}");
//             result += area * sides;
//         }
//
//         return result;
//     }
//
//     private int GetSides(HashSet<Vector> border)
//     {
//         // get vertical sides
//         List<List<Vector>> verticalSides = [];
//         var verticalLines = border.GroupBy(t => t.Col).Select(t=> t.ToList()).ToList();
//
//         foreach(var line in verticalLines)
//         {
//             if(line is [var pos])
//             {
//                 var left = pos + Vector.Left;
//                 var right = pos + Vector.Right;
//                 
//                 if (!border.Contains(left) || !border.Contains(right))
//                     verticalSides.Add(line);
//                 continue;
//             }
//
//             var newLine = new List<Vector>();
//             Vector? previous = null;
//             foreach(var item in line.OrderBy(t => t.Row))
//             {
//                 if(previous == null)
//                 {
//                     previous = item;
//                     continue;
//                 }
//
//                 if(previous.Row == item.Row + 1)
//                 {
//                     newLine.Add(item);
//                 }
//                 else
//                 {
//                     verticalSides.Add(newLine);
//                     newLine = [];
//                     previous = null;
//                 }
//             }
//             
//             if (newLine.Count > 0)
//                 verticalSides.Add(newLine);
//         }
//         
//         List<List<Vector>> horizontalSides = [];
//
//         var horizontalLines = border.GroupBy(t => t.Row).Select(t=> t.ToList()).ToList();
//
//         foreach(var line in horizontalLines)
//         {
//             if(line is [var pos])
//             {
//                 var up = pos + Vector.Up;
//                 var down = pos + Vector.Down;
//                 
//                 if (!border.Contains(up) || !border.Contains(down))
//                     horizontalSides.Add(line);
//                 continue;
//             }
//
//             var newLine = new List<Vector>();
//             Vector? previous = null;
//             foreach(var item in line.OrderBy(t => t.Col))
//             {
//                 if(previous == null)
//                 {
//                     previous = item;
//                     continue;
//                 }
//
//                 if(previous.Col == item.Col + 1)
//                 {
//                     newLine.Add(item);
//                 }
//                 else
//                 {
//                     horizontalSides.Add(newLine);
//                     newLine = [];
//                     previous = null;
//                 }
//             }
//             
//             if (newLine.Count > 0)
//                 horizontalSides.Add(newLine);
//         }
//
//         var comparer = HashSet<Vector>.CreateSetComparer();
//         return verticalSides.Select(t=> t.ToHashSet()).DistinctBy(t=> comparer.GetHashCode(t)).Count()
//             + horizontalSides.Select(t=> t.ToHashSet()).DistinctBy(t=> comparer.GetHashCode(t)).Count();
//
//     }
//     
//     private (int area, int sides) AnalizeRegion(Vector point, HashSet<Vector> seenSides, ref int area, ref int sides)
//     {
//         if (!_visitedPlaces.Add(point))
//             return (area: 0, sides: 0);
//         
//         area += 1;
//         var a = Calculate(point,Vector.Left,seenSides,ref area,ref sides);
//         var b = Calculate(point,Vector.Up,seenSides,ref area,ref sides);
//         var c = Calculate(point,Vector.Right,seenSides,ref area,ref sides);
//         var d =Calculate(point,Vector.Down,seenSides,ref area,ref sides);
//         
//         Vector[] pointsToExplore = new List<Vector?>([a,b,c,d]).OfType<Vector>().Reverse().ToArray();
//         foreach(Vector v in pointsToExplore)
//         {
//             AnalizeRegion(v, seenSides, ref area, ref sides);
//         }
//      
//         return (area: area, sides: sides);
//     }
//
//     private Vector? Calculate(Vector point, Vector direction, HashSet<Vector> seenSides, ref int area, ref int sides)
//     {
//         var nextPoint = point + direction;
//
//         if(CanMove(nextPoint) && this[nextPoint] == this[point])
//         {
//             return nextPoint;
//             // AnalizeRegion(nextPoint, seenSides, ref area,ref sides);
//         }
//         else
//         {
//             if(Equals(direction, Vector.Up) || Equals(direction, Vector.Down))
//             {
//                 var left = nextPoint + Vector.Left;
//                 var visitedLeft = seenSides.Contains(left);
//                 var right = nextPoint + Vector.Right;
//                 var visitedRight = seenSides.Contains(right);
//
//                 if(!visitedLeft && !visitedRight)
//                 {
//                     sides += 1;
//                     seenSides.Add(nextPoint);
//                 } else if((!CanMove(left) || this[left] != this[point]) && (!CanMove(right) || this[right] != this[point]))
//                 {
//                     seenSides.Add(nextPoint);
//                 }
//             }
//             if(Equals(direction, Vector.Right) || Equals(direction, Vector.Left))
//             {
//                 var up = nextPoint + Vector.Up;
//                 var visitedUp = seenSides.Contains(up);
//                 var down = nextPoint + Vector.Down;
//                 var visitedDown = seenSides.Contains(down);
//                 
//                 if (!visitedUp && !visitedDown)
//                 {
//                     sides += 1;
//                     seenSides.Add(nextPoint);
//                 }
//                 else if ((!CanMove(up) || this[up] != this[point]) && (!CanMove(down) || this[down] != this[point]))
//                 {
//                     seenSides.Add(nextPoint);
//                 }
//             }
//         }
//
//         return null;
//     }
//     
//     private bool CanMove(Vector position)
//     {
//         return position.Row >= 0 && position.Row <= input.Length - 1 &&
//                position.Col >= 0 && position.Col <= input[0].Length - 1;
//     }
//     
//     public char this[Vector v]
//     {
//         get
//         {
//             var (row, col) = v;
//             return input[row][col];
//         }
//         set
//         {
//             var (row, col) = v;
//             input[row][col] = value;
//         }
//     }
// }