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

            var statesForwardGraph = CreateForwardGraph(buttons, initialState);
            uint minCount = Dijkstra(statesForwardGraph, initialState)[desiredState];
            res += minCount;
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        throw new NotImplementedException();
    }



    private (int DesiredState, int[] Buttons, int[] Joltages) ParseLine(string line)
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

    private static Dictionary<int, IEnumerable<int>> CreateForwardGraph(int[] buttons, int fromState = 0)
    {
        Dictionary<int, IEnumerable<int>> states = [];
        _fillResult(fromState);
        return states;

        void _fillResult(int state)
        {
            if (states.ContainsKey(state)) return;
            states[state] = buttons.Select(b => state ^ b).Distinct();
            foreach (var s in states[state].Where(s => !states.ContainsKey(s)))
            {
                _fillResult(s);
            }
        }
    }

    private static Dictionary<int, uint> Dijkstra(Dictionary<int, IEnumerable<int>> graph, int src)
    {
        PriorityQueue<int, uint> closestVertices = new();

        Dictionary<int, uint> res = graph.Keys.ToDictionary(v => v, v => uint.MaxValue);

        res[src] = 0;
        closestVertices.Enqueue(src, 0);

        while (closestVertices.TryDequeue(out int v, out uint d))
        {
            if (d > res[v])
            {
                continue;
            }

            foreach (var nv in graph[v])
            {
                var nd = res[v] + 1;
                if (nd < res[nv])
                {
                    res[nv] = nd;
                    closestVertices.Enqueue(nv, nd);
                }
            }
        }
        return res;
    }
}
