namespace Day24.Challenge1;

record XorInstruction(string left, string right) : Instruction(left, right)
{
    public override bool Execute(Dictionary<string, bool> variables)
        => variables[left] != variables[right];
}

record OrInstruction(string left, string right) : Instruction(left, right)
{
    public override bool Execute(Dictionary<string, bool> variables)
        => variables[left] || variables[right];
}

record AndInstruction(string left, string right) : Instruction(left, right)
{
    public override bool Execute(Dictionary<string, bool> variables)
        => variables[left] && variables[right];
}

abstract record Instruction(string left, string right)
{
    public abstract bool Execute(Dictionary<string, bool> variables);

    public  bool TryExecute(Dictionary<string, bool> variables, out bool  result)
    {
        if(variables.ContainsKey(left) && variables.ContainsKey(right))
        {
            result = Execute(variables);
            return true;
        }
        
        result = false;
        return false;
    }
}
