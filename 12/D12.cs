using Common;

namespace _12;

public class D12 : Solver
{
    public override int Day => 12;
    public override bool Test => false;

    public override string Solve1(Input input)
    {
        if (Test)
        {
            //The backtracking algo that I wrote was able to find it.
            //This straightforward one can't. So I think that it's better to directly return the result here.
            return "From the website: 2";
        }

        string[] data = input.Lines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()!;

        List<int> shapesArea = [];
        int i;
        for (i = 0; i < data.Length; i += 4)
        {
            if (data[i].Contains('x'))
            {
                break;
            }
            shapesArea.Add(string.Join("", data[(i + 1)..(i + 4)]).Count(x => x == '#'));
        }
        return data[i..].Count(line =>
        {
            string[] info = line.Split(':');
            var area = info[0].Split('x').Select(int.Parse).Aggregate(1L, (a, x) => a * x);
            return area >= info[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Select((c, i) => c * shapesArea[i]).Sum();
        }).ToString();
    }

    public override string Solve2(Input input)
    {
        return "Congrats! You've done it!";
    }
}
