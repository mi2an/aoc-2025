using Common;

namespace _05;

public class D05 : Solver
{
    public override int Day => 5;

    public override string Solve1(Input input)
    {
        var res = 0;
        bool fresh = true;
        List<(ulong Min, ulong Max)> inventory = [];
        foreach (var line in input.Lines())
        {
            if (line is null) continue;
            if (string.IsNullOrWhiteSpace(line))
            {
                fresh = false;
                continue;
            }
            if (fresh)
            {
                var ns = line.Split('-');
                inventory.Add((ulong.Parse(ns[0]), ulong.Parse(ns[1])));
                continue;
            }

            //We use a brute force approach here (for now?), so let's just use LINQ to simplify things
            var v = ulong.Parse(line);
            if (inventory.Any(mm => mm.Min <= v && v <= mm.Max))
            {
                ++res;
            }
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        throw new NotImplementedException();
    }
}
