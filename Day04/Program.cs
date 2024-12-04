char[][] puzzle = File.ReadLines("input.txt").Select(t=> t.ToArray()).ToArray();
Console.WriteLine("Challenge 1");
int found = 0;
for(int row = 0; row < puzzle.Length; row++)
{
    for(int col = 0; col < puzzle[row].Length; col++)
    {
        bool isStart = puzzle[row][col] == 'X';
        if (!isStart)
            continue;

        if(col >= 3)
        {
            // check backwards
            found += puzzle[row][col-1] == 'M' && puzzle[row][col-2] == 'A' && puzzle[row][col-3] == 'S' ? 1 : 0;
        }

        if(col < puzzle[row].Length - 3)
        {
            // check forward
            found += puzzle[row][col+1] == 'M' && puzzle[row][col+2] == 'A' && puzzle[row][col+3] == 'S' ? 1 : 0;
        }

        if(row >= 3)
        {
            // check up
            found += puzzle[row-1][col] == 'M' && puzzle[row-2][col] == 'A' && puzzle[row-3][col] == 'S' ? 1 : 0;
        }

        if(row < puzzle.Length - 3)
        {
            // check down
            found += puzzle[row+1][col] == 'M' && puzzle[row+2][col] == 'A' && puzzle[row+3][col] == 'S' ? 1 : 0;
        }

        // diagonals
        if(col >= 3 && row >= 3)
        {
            found += puzzle[row-1][col-1] == 'M' && puzzle[row-2][col-2] == 'A' && puzzle[row-3][col-3] == 'S' ? 1 : 0;
        }

        if(col >= 3 && row < puzzle.Length - 3)
        {
            found += puzzle[row+1][col-1] == 'M' && puzzle[row+2][col-2] == 'A' && puzzle[row+3][col-3] == 'S' ? 1 : 0;
        }

        if(col < puzzle[row].Length - 3 && row < puzzle.Length - 3)
        {
            found += puzzle[row+1][col+1] == 'M' && puzzle[row+2][col+2] == 'A' && puzzle[row+3][col+3] == 'S' ? 1 : 0;
        }

        if(col < puzzle[row].Length - 3 && row >= 3)
        {
            found += puzzle[row-1][col+1] == 'M' && puzzle[row-2][col+2] == 'A' && puzzle[row-3][col+3] == 'S' ? 1 : 0;

        }
    }
}
Console.WriteLine(found);

Console.WriteLine("Challenge 2");
found = 0;
for(int row = 0; row < puzzle.Length; row++)
{
    if(row == 0 || row == puzzle.Length - 1)
        continue;
    
    for(int col = 0; col < puzzle[row].Length; col++)
    {
        if (col == 0 || col == puzzle[row].Length - 1)
            continue;
        
        bool isStart = puzzle[row][col] == 'A';
        if (!isStart)
            continue;

        if(puzzle[row - 1][col - 1] == 'M' && puzzle[row + 1][col + 1] == 'S' && puzzle[row - 1][col + 1] == 'M' && puzzle[row + 1][col - 1] == 'S')
            found += 1;
        
        if(puzzle[row - 1][col - 1] == 'M' && puzzle[row + 1][col + 1] == 'S' && puzzle[row - 1][col + 1] == 'S' && puzzle[row + 1][col - 1] == 'M')
            found += 1;
        
        if(puzzle[row - 1][col - 1] == 'S' && puzzle[row + 1][col + 1] == 'M' && puzzle[row - 1][col + 1] == 'S' && puzzle[row + 1][col - 1] == 'M')
            found += 1;
        
        if(puzzle[row - 1][col - 1] == 'S' && puzzle[row + 1][col + 1] == 'M' && puzzle[row - 1][col + 1] == 'M' && puzzle[row + 1][col - 1] == 'S')
            found += 1;

    }
}
Console.WriteLine(found);