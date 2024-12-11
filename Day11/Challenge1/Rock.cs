namespace Day11.Challenge1;

public record Rock(string Value)
{
    public Rock[] Transform()
    {
        return Value switch
        {
            "0" => [new Rock("1")],
            { Length: var length } when length % 2 == 0 =>
            [
                new Rock(Parse(Value, 0, length / 2)),
                new Rock(Parse(Value, length / 2, length / 2)),
            ],
            var other => [new Rock((long.Parse(other)* 2024).ToString())]
        };
    }

    private static string Parse(string input, int start, int length)
    {
        var a= input.Substring(start, length ).TrimStart('0');

        return a is "" ? "0" : a;
    }

}