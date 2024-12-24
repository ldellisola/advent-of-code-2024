namespace Day22.Challenge1;
using number = ulong;

public static class Monkey
{
    

    public static number CalculateSecret(number secret, int iterations)
    {
        while(iterations != 0)
        {
            iterations--;
            secret = CalculateSecret(secret);
        }
        return secret;
    }
    public static number CalculateSecret(number secret)
    {
        var a = MixAndPrune(secret * 64, secret);
        var b = MixAndPrune(a / 32, a);
        var c = MixAndPrune(b * 2048, b);
        return c;
    }

    static number MixAndPrune (number value, number secret) => (value ^ secret) % 16777216;
}