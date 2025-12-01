using Common;

namespace _01;

public class D01 : Solver
{
    public override int Day => 1;

    public override string Solve1(Input input)
    {
        int res = 0;
        int dial = 50;
        foreach (var line in input.Lines())
        {
            if (line is null) continue;

            char action = line[0];
            if (action == '-')
            {
                continue;
            }

            int number = int.Parse(line[1..]);

            dial += action switch
            {
                'R' => +(number % 100),
                'L' => -(number % 100),
                _ => throw new InvalidOperationException($"Unknown action '{action}'"),
            };

            dial = (dial + 100) % 100;
            if (dial == 0) res++;
        }
        return res.ToString();
    }

    public override string Solve2(Input input)
    {
        int res = 0;
        int dial = 50;
        foreach (var line in input.Lines())
        {
            if (line is null) continue;

            char action = line[0];
            if (action == '-')
            {
                continue;
            }

            var number = int.Parse(line[1..]);

            res += number / 100;

            int d = action switch
            {
                'R' => +(number % 100),
                'L' => -(number % 100),
                _ => throw new InvalidOperationException($"Unknown action '{action}'"),
            };

            var t = dial + d;
            res += t switch
            {
                0 or >= 100 => 1,
                < 0 => dial == 0 ? 0 : 1,
                _ => 0,
            };
            dial = (t + 100) % 100;
        }

        return res.ToString();
    }
}
