using Common;

namespace _04
{
    public class D04 : Solver
    {
        public override int Day => 4;

        public override string Solve1(Input input)
        {
            var res = 0;
            var f = "";
            var c = "";
            foreach (var line in input.Lines())
            {
                if (line is null) continue;

                if (c == "")
                {
                    f = new('.', line.Length + 2);
                    c = '.' + line + '.';
                    continue;
                }

                var n = '.' + line + '.';
                res += Process(f, c, n);

                f = c;
                c = n;
            }
            res += Process(f, c, new('.', c.Length + 2));
            return res.ToString();
        }

        public override string Solve2(Input input)
        {
            throw new NotImplementedException();
        }

        private static int Process(string f, string c, string n)
        {
            var res = 0;
            for (int i = 1; i < c.Length; ++i)
            {
                if (c[i] != '@') continue;

                var t = 0;

                if (f[i - 1] == '@') t++;
                if (f[i] == '@') t++;
                if (f[i + 1] == '@') t++;

                if (c[i - 1] == '@') t++;
                //
                if (c[i + 1] == '@') t++;

                if (n[i - 1] == '@') t++;
                if (n[i] == '@') t++;
                if (n[i + 1] == '@') t++;

                if (t < 4) ++res;
            }
            return res;
        }
    }
}
