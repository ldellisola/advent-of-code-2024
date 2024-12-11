using System.Collections.Concurrent;

namespace Day11.Challenge2;

public class Map(string[] input, int maxGeneration)
{
    private readonly (int Generation, ulong value)[] _rockBag = input.Select(t => (0, ulong.Parse(t))).ToArray();
    public ulong TotalRocks = 0;
    

    public void Run()
    {
        foreach(var rock in _rockBag)
        {
            TotalRocks += Foo(rock);
        }
    }



    private readonly Dictionary<(int Generation, ulong value), ulong> _rockBagToGeneration = new();
    private ulong Foo((int generation, ulong value) rock)
    {
        if(_rockBagToGeneration.TryGetValue(rock, out ulong count))
            return count;
        
        if(rock.generation == maxGeneration)
        {
            return _rockBagToGeneration[rock] = 1;
        }
        

        if(rock.value is 0)
        {
            return _rockBagToGeneration[rock] = Foo((rock.generation + 1, 1ul));;
        }
        
        
        var digits = rock.value == 0 ? 1 : Math.Floor(Math.Log10(rock.value) + 1);
        if(digits % 2 == 0)
        {
            var middle = Math.Pow(10, digits / 2);
            var firstRock = (rock.generation + 1, (ulong)Math.Floor(rock.value % middle));
            var secondRock = (rock.generation + 1, (ulong)Math.Floor(rock.value / middle));
            return _rockBagToGeneration[rock] = Foo(firstRock) + Foo(secondRock);
        }
            
        return _rockBagToGeneration[rock] = Foo((rock.generation+1, rock.value * 2024));
    }
}