using System.Diagnostics;

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
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine(Solve1(input));
        sw.Stop();
        Console.WriteLine($"(Took {sw.ElapsedMilliseconds} ms)");
        sw.Restart();
        Console.WriteLine(Solve2(input));
        sw.Stop();
        Console.WriteLine($"(Took {sw.ElapsedMilliseconds} ms)");
    }

    public virtual bool Test { get; } = false;
    public abstract int Day { get; }
    public abstract string Solve1(Input input);
    public abstract string Solve2(Input input);
}
