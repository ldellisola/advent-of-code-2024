namespace Day11.Challenge2;

public record Rock(double Value)
{
    public IEnumerable<Rock> Transform()
    {
        if(Value is 0)
            return [new Rock(1)];


        var digits = Value == 0 ? 1 : Math.Floor(Math.Log10(Value) + 1);
        
        if(digits % 2 == 0)
        {
            var middle = Math.Pow(10, digits / 2);
            return
            [
                new Rock(Math.Floor(Value / middle)),
                new Rock(Math.Floor(Value % middle)),
            ];
        }

        return [new Rock(Value * 2024)];
    }
}