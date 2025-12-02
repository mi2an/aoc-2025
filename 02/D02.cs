using Common;

namespace _02
{
    public class D02 : Solver
    {
        public override int Day => 2;

        public override string Solve1(Input input)
        {
            return Solve(input, (min, max) =>
            {
                ulong res = 0;
                //That's a brute force solution... I'll get back to it later... Maybe :p
                for (var i = min; i <= max; i++)
                {
                    if (IsInvalid_1(i))
                    {
                        res += i;
                    }
                }
                return res;
            }).ToString();
        }

        public override string Solve2(Input input)
        {
            return Solve(input, (min, max) =>
            {
                ulong res = 0;
                //Again, that's a brute force solution, and I'll get back to it later... maybe :p
                for (var i = min; i <= max; i++)
                {
                    if (IsInvalid_2(i))
                    {
                        res += i;
                    }
                }
                return res;
            }).ToString();
        }

        private ulong Solve(Input input, Func<ulong, ulong, ulong> solve)
        {
            ulong res = 0;
            foreach (var data in input.Next(Separators))
            {
                if (data is null) continue;
                var s = data.Split('-');
                var min = ulong.Parse(s[0]);
                var max = ulong.Parse(s[1]);

                res += solve(min, max);
            }
            return res;
        }

        private static bool IsInvalid_1(ulong number)
        {
            var s = number.ToString();
            if (s.Length % 2 != 0)
            {
                return false; // Odd number of digits are not a duplicated sequence.
            }
            var half = s.Length / 2;

            for (var i = 0; i < half; ++i)
            {
                var j = i + half;
                if (s[i] != s[j])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsInvalid_2(ulong number)
        {
            var s = number.ToString();
            if (s.Length < 2)
            {
                return false;
            }

            var lenDivs = new List<int>(s.Length) { 1 };
            for (int r = s.Length / 2; r > 1; --r)
            {
                if (s.Length % r == 0)
                {
                    lenDivs.Add(r);
                }
            }

            return lenDivs.Any(n =>
            {
                for (var i = 0; i < n; ++i)
                {
                    char c = s[i];

                    for (int j = i + n; j < s.Length; j += n)
                    {
                        if (s[j] != c)
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
        }

        private readonly HashSet<string> Separators = [","];
    }
}
