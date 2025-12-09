using Common;

namespace _09;

public class D09 : Solver
{
    public override int Day => 9;

    public override string Solve1(Input input)
    {
        long res = 0L;
        var points = input.Lines().Where(l => l is not null).Select(x => x!)
            .Select(l => l.Split(',').Select(e => long.Parse(e)).ToArray())
            .ToArray();
        for(int i = 0; i < points.Length; i++)
        {
            var p1 = points[i];
            for(int j = i + 1; j < points.Length; j++)
            {
                var p2 = points[j];
                var dx = Math.Abs(p1[0] - p2[0]) + 1;
                var dy = Math.Abs(p1[1] - p2[1]) + 1;
                res = Math.Max(res, dx * dy);
            }
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        throw new NotImplementedException();
    }
}
