using System.Text;

namespace Day22.Challenge2;
using number = ulong;

public class RoundList(int size)
{
    private int[] _array = new int[size];
    private int index = 0;

    public void Add(int number)
    {
        _array[index%size] = number;
        index++;
    }

    public int ReverseAndGet()
    {
        index = index == 0 ? size-1 : index - 1;
        return _array[index];
    }

    public int[] ToArray()
    {
        var data = new int[size];
        for(int i = 1; i <= size; i++)
        {
           data[i-1] = _array[(index + i) % size];
        }
        return data;
    }
    public override string ToString()
    {
        var bld = new StringBuilder();
        for(int i = 0; i < size; i++)
        {
            if (i !=0)
                bld.Append(',');
            bld.Append(Math.Sign(_array[(index + i) % size]) switch
               {
                   1 => "",
                   0 => "",
                   -1 => "-",
                   _ => throw new IndexOutOfRangeException()
               })
               .Append(Math.Abs(_array[(index + i) % size]));
        }
        return bld.ToString();
    }
}

public static class Monkey
{
    public static IEnumerable<(int bestPrice, string sequence)> CalculateSecret(number secret, int iterations)
    {
        var seen = new HashSet<string>();
        // int highestPrice = -1;

        var pastDiff = new RoundList(4);
        
        int totalIterations = iterations;
        var previousPrice = (int) secret %10;
        while(iterations != 0)
        {
            secret = CalculateSecret(secret);
            var price = BananaPrice(secret);
            
            pastDiff.Add(price-previousPrice);
            previousPrice = price;
            iterations--;
            
            if (iterations > totalIterations - 4)
                continue;

            var sequence = pastDiff.ToString();
            if (!seen.Add(sequence))
                yield return (price, pastDiff.ToString());
        }
    }    
    
    private static int BananaPrice(number secret) => (int) secret %10;
    public static number CalculateSecret(number secret)
    {
        var a = MixAndPrune(secret * 64, secret);
        var b = MixAndPrune(a / 32, a);
        var c = MixAndPrune(b * 2048, b);
        return c;
    }

    static number MixAndPrune (number value, number secret) => (value ^ secret) % 16777216;
}