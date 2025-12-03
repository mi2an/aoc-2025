using System.Text;
using Common;

namespace _03;

public class D03 : Solver
{
    public override int Day => 3;
    public override bool Test => false;

    public override string Solve1(Input input)
    {
        var res = 0ul;
        foreach (var line in input.Lines())
        {
            if (line is null) continue;

            res += Solve(line, 2);
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        var res = 0ul;
        foreach (var line in input.Lines())
        {
            if (line is null) continue;

            res += Solve(line, 12);
        }
        return res.ToString();
    }

    private static ulong Solve(string line, int count)
    {
        if (line.Length < count)
        {
            return 0ul;
        }
        if (line.Length == count)
        {
            return ulong.Parse(line);
        }
        StringBuilder res = new();

        var li = 0;
        while (count > 0)
        {
            var ci = line.Length - count;
            var cv = line[ci] - '0';
            for (int i = ci; i >= li; --i)
            {
                if ((line[i] - '0') >= cv)
                {
                    ci = i;
                    cv = line[i] - '0';
                }
            }
            res.Append(line[ci]);
            li = ci + 1;
            --count;
        }

        return ulong.Parse(res.ToString());
    }
}
