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
        var res = 0ul;
        List<(ulong Min, ulong Max)> inventory = [];
        foreach (var line in input.Lines())
        {
            if (line is null) continue;
            if (string.IsNullOrWhiteSpace(line)) break;
            var ns = line.Split('-');
            inventory.Add((ulong.Parse(ns[0]), ulong.Parse(ns[1])));
        }

        var ic = inventory.Count;
        bool merged;
        do
        {
            merged = false;
            for (int i = 0; i < ic; ++i)
            {
                for (int j = i + 1; j < ic; ++j)
                {
                    var (rm, rM) = inventory[i];
                    var (bm, bM) = inventory[j];
                    if (rm <= bM && bm <= rM)
                    {
                        inventory[i] = (Math.Min(rm, bm), Math.Max(rM, bM));
                        inventory[j] = inventory[ic - 1];
                        --ic;
                        --j;
                        merged = true;
                    }
                }
            }
        } while (merged);

        for (int i = 0; i < ic; ++i)
        {
            var (rm, rM) = inventory[i];
            res += (rM - rm + 1);
        }
        return res.ToString();
    }
}
