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
        for (int i = 0; i < points.Length; i++)
        {
            var p1 = points[i];
            for (int j = i + 1; j < points.Length; j++)
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
        long res = 0L;
        var points = input.Lines().Where(l => l is not null).Select(x => x!)
            .Select(l => l.Split(',').Select(e => long.Parse(e)).ToArray())
            .ToArray();

        for (int i = 0; i < points.Length; i++)
        {
            var p1 = points[i];
            for (int j = i + 1; j < points.Length; j++)
            {
                var p2 = points[j];

                long[][] corners = [[p1[0], p2[1]], [p2[0], p1[1]]];
                bool[] ok = [false, false];
                double[] angles = [0, 0];

                var mX = Math.Min(p1[0], p2[0]);
                var mY = Math.Min(p1[1], p2[1]);
                var MX = Math.Max(p1[0], p2[0]);
                var MY = Math.Max(p1[1], p2[1]);

                for (int k = 0; k < points.Length; ++k)
                {
                    var pk = points[k];
                    var pl = points[k == 0 ? points.Length - 1 : k - 1];

                    if (pk[0] > mX && pk[0] < MX && pk[1] > mY && pk[1] < MY || pl[0] > mX && pl[0] < MX && pl[1] > mY && pl[1] < MY)
                    {
                        //a point inside the rectangle means there is a line cutting through
                        ok[0] = false;
                        angles[0] = 0;
                        break;
                    }

                    //Being here means that pk and pl are outside the candidate rectangle

                    var mtX = Math.Min(pk[0], pl[0]);
                    var MtX = Math.Max(pk[0], pl[0]);
                    var mtY = Math.Min(pk[1], pl[1]);
                    var MtY = Math.Max(pk[1], pl[1]);

                    if (mtX == MtX && mtX > mX && mtX < MX)
                    {
                        if (mtY <= mY && MtY > mY || mtY < MY && MtY >= MY)
                        {
                            //There is a vertical line crossing the rectangle
                            ok[0] = false;
                            angles[0] = 0;
                            break;
                        }
                    }
                    if (mtY == MtY && mtY > mY && mtY < MY)
                    {
                        if (mtX <= mX && MtX > mX || mtX < MX && MtX >= MX)
                        {
                            //There is a horizontal line crossing the rectangle
                            ok[0] = false;
                            angles[0] = 0;
                            break;
                        }
                    }

                    for (int t = 0; t < 2; ++t)
                    {
                        var c = corners[t];
                        if (pk[0] == c[0] && pk[1] == c[1] || pl[0] == c[0] && pl[1] == c[1])
                        {
                            ok[t] = true;
                        }

                        if (!ok[t])
                        {
                            var a1 = Math.Atan2(c[1] - pk[1], c[0] - pk[0]);
                            var a2 = Math.Atan2(c[1] - pl[1], c[0] - pl[0]);

                            var da = a2 - a1;
                            while (da >= Math.PI) da -= 2 * Math.PI;
                            while (da < -Math.PI) da += 2 * Math.PI;

                            angles[t] += da;
                        }
                    }

                    if (ok.All(o => o))
                    {
                        break;
                    }
                }

                ok[0] = ok[0] || Math.Abs(Math.Abs(angles[0]) - 2 * Math.PI) < 0.01;
                ok[1] = ok[1] || Math.Abs(Math.Abs(angles[1]) - 2 * Math.PI) < 0.01;

                var (x, y) = (p1[0], p1[1]);
                var (x1, y1) = (p2[0], p2[1]);
                if (!ok.All(o => o))
                {
                    continue;
                }
                var dx = Math.Abs(p1[0] - p2[0]) + 1;
                var dy = Math.Abs(p1[1] - p2[1]) + 1;
                res = Math.Max(res, dx * dy);
            }
        }
        return res.ToString();
    }
}