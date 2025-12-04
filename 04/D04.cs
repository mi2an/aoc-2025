using Common;

namespace _04;

public class D04 : Solver
{
    public override int Day => 4;

    public override string Solve1(Input input)
    {
        var rolls = input.Lines()
                .Where(l => l is not null)!
                .Select<string, char[]>(l => ['.', .. l.ToCharArray(), '.']).ToArray();
        string t = new('.', rolls[0].Length);
        return Process([
            t.ToCharArray()
            , ..rolls
            , t.ToCharArray()
        ]).ToString();
    }

    public override string Solve2(Input input)
    {
        var res = 0;
        var rolls = input.Lines()
            .Where(l => l is not null)!
            .Select<string, char[]>(l => ['.', .. l.ToCharArray(), '.'])
            .ToArray();
        string t = new('.', rolls[0].Length);
        rolls = [t.ToCharArray(), .. rolls, t.ToCharArray()];

        int cc;
        do
        {
            cc = 0;
            cc = Process(rolls);
            res += cc;
        } while (cc != 0);
        return res.ToString();
    }

    private static int Process(char[][] rolls)
    {
        var r = rolls.Length;
        var c = rolls[0].Length;

        var res = 0;
        for (int i = r - 2; i > 0; --i)
        {
            for (int j = c - 2; j > 0; --j)
            {
                if (rolls[i][j] != '@') continue;

                var t = 0;

                if (rolls[i - 1][j - 1] != '.') t++;
                if (rolls[i - 1][j] != '.') t++;
                if (rolls[i - 1][j + 1] != '.') t++;

                if (rolls[i][j - 1] != '.') t++;
                //
                if (rolls[i][j + 1] != '.') t++;

                if (rolls[i + 1][j - 1] != '.') t++;
                if (rolls[i + 1][j] != '.') t++;
                if (rolls[i + 1][j + 1] != '.') t++;

                if (t < 4)
                {
                    rolls[i][j] = 'X';
                    ++res;
                }
            }
        }
        for (int i = r - 2; i > 0; --i)
        {
            for (int j = c - 2; j > 0; --j)
            {
                if (rolls[i][j] == 'X') rolls[i][j] = '.';
            }
        }
        return res;
    }
}
