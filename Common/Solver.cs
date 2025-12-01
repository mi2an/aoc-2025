namespace Common;

public abstract class Solver
{
    public void Run(string inputFile)
    {
        Console.WriteLine($"--- Day {Day} ---");
        if (Test)
        {
            inputFile = inputFile.Replace(".txt", ".test.txt");
        }
        var input = new Input(inputFile);
        Console.WriteLine(Solve1(input));
        Console.WriteLine(Solve2(input));
    }

    public virtual bool Test { get; } = false;
    public abstract int Day { get; }
    public abstract string Solve1(Input input);
    public abstract string Solve2(Input input);
}
