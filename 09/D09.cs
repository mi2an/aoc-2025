using System.ComponentModel.Design;
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

                if (HasCrossingLine(points, p1, p2))
                {
                    continue;
                }

                if (!PnPoly_ForHorizontalOrVerticalLines(points, p1[0], p2[1]) || !PnPoly_ForHorizontalOrVerticalLines(points, p2[0], p1[1]))
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

    private static bool HasCrossingLine(long[][] points, long[] p1, long[] p2)
    {
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
                return true;
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
                    return true;
                }
            }
            if (mtY == MtY && mtY > mY && mtY < MY)
            {
                if (mtX <= mX && MtX > mX || mtX < MX && MtX >= MX)
                {
                    //There is a horizontal line crossing the rectangle
                    return true;
                }
            }
        }

        return false;
    }

    //https://www.eecs.umich.edu/courses/eecs380/HANDOUTS/PROJ2/InsidePoly.html
    //https://wrfranklin.org/Research/Short_Notes/pnpoly.html
    //This algorithm has been specifically modified to only address horizontal and vertical lines.
    private static bool PnPoly_ForHorizontalOrVerticalLines(long[][] polygon, long x, long y)
    {
        bool res = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i, ++i)
        {
            var xi = polygon[i][0];
            var yi = polygon[i][1];
            var yj = polygon[j][1];

            var is_y_between_ij = (yi <= y) && (y < yj) || (yj <= y) && (y < yi);
            var is_horizontal_line = yj == yi;
            var is_to_the_left_of_ij = x < xi; //Verifying "xi" is enough since this operation applies only when xi == xj, indicating a vertical line.

            var is_x_correctly_placed = is_horizontal_line || is_to_the_left_of_ij;

            if (is_y_between_ij && is_x_correctly_placed)
            {
                res = !res;
            }
        }
        return res;
    }
}