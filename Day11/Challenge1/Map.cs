namespace Day11.Challenge1;

public class Map(string[] input)
{
    private Queue<Rock> _rocks = new(input.Select(t=> new Rock(t)));

    public void Blink()
    {
        var newQueue = new Queue<Rock>();
        while(_rocks.TryDequeue(out var rock))
        {
            var newRocks = rock.Transform();

            foreach(var newRock in newRocks)
            {
                newQueue.Enqueue(newRock);
            }
        }
        
        _rocks = newQueue;
    }
    
    
    public Rock[] GetRocks()
    {
        return _rocks.ToArray();
    }
}