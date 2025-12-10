using Common;

namespace _10;

public class D10 : Solver
{
    public override int Day => 10;

    public override string Solve1(Input input)
    {
        uint res = 0;
        foreach (var line in input.Lines())
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var (desiredState, buttons, _) = ParseLine(line);

            int initialState = 0;

            var statesForwardGraph = CreateLightStatesForwardGraph(buttons, initialState);
            uint minCount = Dijkstra(statesForwardGraph, initialState)[desiredState];
            res += minCount;
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        long res = 0;
        foreach (var line in input.Lines())
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var (_, buttons, joltages) = ParseLine(line);


            using var ctx = new Microsoft.Z3.Context();
            var solver = ctx.MkOptimize();

            var counts = buttons.Select((b, i) => ctx.MkIntConst($"x{i}")).ToArray();
            foreach (var c in counts)
            {
                solver.Add(ctx.MkGe(c, ctx.MkInt(0)));
            }

            for (int i = 0, mask = 1; i < joltages.Length; i++, mask <<= 1)
            {
                var sum = ctx.MkAdd(counts.Where((v, j) => (buttons[j] & mask) != 0));

                solver.Add(ctx.MkEq(sum, ctx.MkInt(joltages[i])));
            }

            solver.MkMinimize(ctx.MkAdd(counts));

            var chk = solver.Check();

            if (chk != Microsoft.Z3.Status.SATISFIABLE)
            {
                Console.WriteLine("Line problem unsatisfiable: '{0}'", line);
                continue;
            }

            var m = solver.Model;
            res += counts.Select(c => (solver.Model.Evaluate(c, true) as Microsoft.Z3.IntNum)!.Int64).Sum();
        }
        return res.ToString();
    }


    private static (int DesiredState, int[] Buttons, int[] Joltages) ParseLine(string line)
    {
        int li = line.LastIndexOf(']');
        int bi = line.LastIndexOf(')');

        int DesiredState = 0;
        foreach (var c in line[1..li].Reverse())
        {
            DesiredState = (DesiredState << 1) | (c == '.' ? 0 : 1);
        }

        var Buttons = line[(li + 2)..(bi + 1)]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(b => b[1..(b.Length - 1)]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(n => (1 << int.Parse(n)))
            .Aggregate(0, (a, v) => a | v)
        ).ToArray();

        var Joltages = line[(bi + 3)..(line.Length - 1)]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
        return (DesiredState, Buttons, Joltages);
    }


    private static Dictionary<int, IEnumerable<int>> CreateLightStatesForwardGraph(int[] buttons, int fromState = 0)
    {
        Dictionary<int, IEnumerable<int>> res = [];
        _fillResult(fromState);
        return res;

        void _fillResult(int state)
        {
            if (res.ContainsKey(state)) return;
            res[state] = [.. buttons.Select(b => state ^ b).Distinct()];
            foreach (var s in res[state].Where(s => !res.ContainsKey(s)))
            {
                _fillResult(s);
            }
        }
    }


    private static Dictionary<Key, uint> Dijkstra<Key>(Dictionary<Key, IEnumerable<Key>> graph, Key src) where Key : notnull
        => Dijkstra(graph, src, v => v);

    private static Dictionary<Key, uint> Dijkstra<Key, Value>(Dictionary<Key, IEnumerable<Value>> graph, Value src, Func<Value, Key> getKey)
        where Key : notnull
        where Value : notnull
    {
        PriorityQueue<Key, uint> closestVertices = new();

        Dictionary<Key, uint> res = graph.Keys.ToDictionary(v => v, v => uint.MaxValue);

        var key = getKey(src);
        res[key] = 0;
        closestVertices.Enqueue(key, 0);

        while (closestVertices.TryDequeue(out var v, out uint d))
        {
            if (d > res[v])
            {
                continue;
            }

            foreach (var nv in graph[v])
            {
                var nd = res[v] + 1;
                var nk = getKey(nv);
                if (nd < res[nk])
                {
                    res[nk] = nd;
                    closestVertices.Enqueue(nk, nd);
                }
            }
        }
        return res;
    }
}

//16043 too low     (Google.OR, Math.Round)
//16290 is wrong... (Google.OR, Math.Ceiling)

//16063 ... GOOD!   (Microsoft.Z3)