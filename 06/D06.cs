using System.Text;
using Common;

namespace _06;

public class D06 : Solver
{
    public override int Day => 6;

    public override string Solve1(Input input)
    {
        ulong res = 0;

        var lines = input.Lines().Where(l => l is not null).Select(l => l!.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();

        var ops = lines.Last().Select(s => s[0]).ToList();
        for (int j = lines[0].Length - 1; j >= 0; --j)
        {
            var isAdd = ops[j] == '+';
            var com = isAdd ? 0ul : 1ul;
            for (int i = lines.Count - 2; i >= 0; --i)
            {
                if (isAdd)
                {
                    com += ulong.Parse(lines[i][j]);
                }
                else
                {
                    com *= ulong.Parse(lines[i][j]);
                }
            }
            res += com;
        }

        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        ulong res = 0;

        var lines = input.Lines().Where(l => l is not null).Select(l => l!).ToList();
        var ops = lines.Last();
        StringBuilder n = new();

        bool isAdd;
        var t = lines[0].Length - 1;
        while (ops[t] == ' ') --t;
        isAdd = ops[t] == '+';
        ulong co = isAdd ? 0ul : 1ul;
        for (int j = lines[0].Length - 1; j >= 0; --j)
        {
            if (j < t)
            {
                t = j;
                while (ops[t] == ' ') --t;
                isAdd = ops[t] == '+';
                res += co;
                co = isAdd ? 0ul : 1ul;
            }

            n.Clear();
            bool hasNumber = false;
            for (int i = 0; i < lines.Count - 1; ++i)
            {
                var c = lines[i][j];
                if (c != ' ') hasNumber = true;

                n.Append(c);
            }

            if (!hasNumber) continue;

            if (isAdd)
            {
                co += ulong.Parse(n.ToString());
            }
            else
            {
                co *= ulong.Parse(n.ToString());
            }
        }

        res += co;
        return res.ToString();
    }
}
