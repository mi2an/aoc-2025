using Common;

namespace _08;

public class D08 : Solver
{
    public override int Day => 8;

    public override string Solve1(Input input)
    {
        long res = 1L;
        var junctionBoxes = input.Lines().Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => Pos.New(x!)).ToArray();
        PriorityQueue<(int, int), double> pq = new();

        for (int i = 0; i < junctionBoxes.Length; i++)
        {
            for (int j = i + 1; j < junctionBoxes.Length; j++)
            {
                pq.Enqueue((i, j), junctionBoxes[i].Distance(junctionBoxes[j]));
            }
        }

        Dictionary<int, HashSet<int>> groups = [];
        for (int i = 0; i < junctionBoxes.Length; i++)
        {
            groups[i] = [i];
        }

        var count = Test ? 10 : 1000;
        while (count-- > 0)
        {
            if (pq.TryDequeue(out var groupPair, out var distance) == false)
            {
                break;
            }
            var (i, j) = groupPair;

            var fg = groups[i];
            var sg = groups[j];
            fg.UnionWith(sg);

            foreach (var k in fg)
            {
                groups[k] = fg;
            }
        }
        pq.Clear();

        res = groups.Values.Distinct().OrderByDescending(k => k.Count).Take(3).Select(x => x.Count).Aggregate(1L, (acc, x) => acc * x);
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        throw new NotImplementedException();
    }


    private record Pos(long X, long Y, long Z)
    {
        public static Pos New(string s)
        {
            var parts = s.Split(',').Select(long.Parse).ToArray();
            return new Pos(parts[0], parts[1], parts[2]);
        }

        public double Distance(Pos other)
        {
            //We don't have to take the square root for distance comparisons, it will faster this way.
            return Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2);
        }
    }
}
