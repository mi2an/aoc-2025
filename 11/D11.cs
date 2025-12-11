using Common;

namespace _11;

public class D11 : Solver
{
    public override int Day => 11;

    public override string Solve1(Input input)
    {
        ulong res = 0;
        var forwardGraph = input.Lines().Where(l => !string.IsNullOrWhiteSpace(l)).Select(l =>
        {
            var s = l!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (Key: s[0].Replace(":", ""), Next: s[1..]);
        }).ToDictionary(k => k.Key, k => k.Next);

        //We assume that there is no loop.
        Queue<string> track = new();
        track.Enqueue("you");

        while (track.TryDequeue(out var c))
        {
            if (c == "out")
            {
                ++res;
                continue;
            }

            if (!forwardGraph.TryGetValue(c, out var connections))
            {
                continue;
            }
            foreach (var next in connections)
            {
                track.Enqueue(next);
            }
        }

        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        ulong res = 0;
        var forwardGraph = input.Lines().Where(l => !string.IsNullOrWhiteSpace(l)).Select(l =>
        {
            var s = l!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (Key: s[0].Replace(":", ""), Next: s[1..]);
        }).ToDictionary(k => k.Key, k => k.Next);

        res = Visit("svr", "out", forwardGraph, ["fft", "dac"], []);

        return res.ToString();
    }

    private static ulong Visit(string start, string end, Dictionary<string, string[]> graph, HashSet<string> mustVisit, HashSet<string> visiting)
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
        foreach (var n in next.Where(n => !visiting.Contains(n)))
        {
            res += Visit(n, end, graph, mustVisit, visiting);
        }
        visiting.Remove(start);

        return res;
    }
}
