using Common;

namespace _03;

public class D03 : Solver
{
    public override int Day => 3;
    public override bool Test => false;

    public override string Solve1(Input input)
    {
        var res = 0;
        foreach (var line in input.Lines())
        {
            if (line is null) continue;

            int it = line.Length - 2;
            int v = line[it] - '0';
            for (int i = it; i >= 0; --i)
            {
                if ((line[i] - '0') >= v)
                {
                    it = i;
                    v = line[i] - '0';
                }
            }

            var lv = v*10;
            ++it;
            v = line[it] - '0';
            for (int i = it; i < line.Length; ++i)
            {
                if ((line[i] - '0') > v)
                {
                    it = i;
                    v = line[it] - '0';
                }
            }
            res += lv + v;
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        var res = 0;

        return res.ToString();
    }
}
