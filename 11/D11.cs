using Common;

namespace _11;

public class D11 : Solver
{
    public override int Day => 11;

    public override string Solve1(Input input)
    {
        return Solve(input, "you", "out").ToString();
    }

    public override string Solve2(Input input)
    {
        return Solve(input, "svr", "out", ["dac", "fft"]).ToString();
    }


    private static ulong Solve(Input input, string start, string end, string[]? mustVisitNodes = default)
    {
        ulong res = 0;
        var forwardGraph = input.Lines()
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l!.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(s => s[0][..^1], s => s[1..]);
        res = Visit(start, end, forwardGraph, mustVisitNodes ?? [], [], []);
        return res;
    }

    private static ulong Visit(
        string start, string end
        , Dictionary<string, string[]> graph, string[] mustVisit
        , HashSet<string> visiting, Dictionary<string, ulong> explored)
    {
        if (start == end)
        {
            if (mustVisit.All(visiting.Contains))
            {
                return 1;
            }
            return 0;
        }

        if (!graph.TryGetValue(start, out var next))
        {
            return 0;
        }

        ulong res = 0u;

        visiting.Add(start);
        foreach (var n in next)
        {
            if (visiting.Contains(n))
            {
                throw new ApplicationException("Loop detected");
            }

            var key = $"{n}-{string.Join("-", mustVisit.Select(visiting.Contains))}";
            if (!explored.TryGetValue(key, out var cache))
            {
                cache = Visit(n, end, graph, mustVisit, visiting, explored);
                explored[key] = cache;
            }

            res += cache;
        }
        visiting.Remove(start);

        return res;
    }
}
