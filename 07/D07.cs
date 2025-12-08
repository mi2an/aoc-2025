using Common;

namespace _07;

public class D07 : Solver
{
    public override int Day => 7;

    public override string Solve1(Input input)
    {
        int res = 0;
        HashSet<int> tachyons = [];
        int m = -1;
        int M = -1;
        foreach (var line in input.Lines())
        {
            if (line is null) continue;

            if (m == -1)
            {
                m = 0;
                M = line.Length;
                tachyons.Add(line.IndexOf('S'));
                continue;
            }
            for (int i = 0; i < M; ++i)
            {
                if (line[i] != '^' || !tachyons.Contains(i))
                {
                    continue;
                }

                ++res;
                tachyons.Remove(i);
                tachyons.Add(Math.Max(i - 1, 0));
                tachyons.Add(Math.Min(i + 1, M - 1));
            }
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        long res = 0;
        Dictionary<int, long> tachyons = [];
        int m = -1;
        int M = -1;
        foreach (var line in input.Lines())
        {
            if (line is null) continue;

            if (m == -1)
            {
                m = 0;
                M = line.Length;
                tachyons.Add(line.IndexOf('S'), 1);
                continue;
            }
            for (int i = 0; i < M; ++i)
            {
                if (line[i] != '^' || !tachyons.ContainsKey(i))
                {
                    continue;
                }

                ++res;
                long count = tachyons[i];
                tachyons.Remove(i);
                int t = Math.Max(i - 1, 0);
                if (!tachyons.TryAdd(t, count))
                {
                    tachyons[t] += count;
                }
                t = Math.Min(i + 1, M - 1);

                if (!tachyons.TryAdd(t, count))
                {
                    tachyons[t] += count;
                }
            }
        }
        res = tachyons.Sum(kvp => kvp.Value);
        return res.ToString();
    }
}
