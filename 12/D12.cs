using Common;

namespace _12;

public class D12 : Solver
{
    public override int Day => 12;

    public override string Solve1(Input input)
    {
        var (shapes, boards) = ParseInput(input);

        return boards.Count(b => b.Accepts(shapes)).ToString();
    }

    public override string Solve2(Input input)
    {
        throw new NotImplementedException();
    }


    private static (IList<Shape> Shapes, IEnumerable<Board> Board) ParseInput(Input input)
    {
        string[] data = input.Lines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()!;

        int i;
        List<Shape> shapes = [];
        for (i = 0; i < data.Length; i += 4)
        {
            if (data[i].Contains('x'))
            {
                break;
            }
            shapes.Add(new Shape(data[(i + 1)..(i + 4)]));
        }
        IEnumerable<Board> boardData = data[i..].Select(line => new Board(line));
        return (shapes, boardData);
    }


    class Shape
    {
        public bool this[int i, int j] => Data[i, j];

        private bool[,] Data { get; }
        public Shape(string[] input)
        {
            Data = new bool[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Data[i, j] = input[i][j] == '#';
                }
            }
        }
        private Shape(bool[,] data)
        {
            Data = data;
        }

        public override string ToString()
        {
            var lines = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                var line = "";
                for (int j = 0; j < 3; j++)
                {
                    line += Data[i, j] ? '#' : '.';
                }
                lines.Add(line);
            }
            return string.Join(Environment.NewLine, lines);
        }

        public ISet<Shape> Transform()
        {
            var tr = Enumerable.Range(0, 6).Select(_ => new bool[3, 3]).ToArray();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var data = Data[i, j];
                    tr[0][i, j] = data; //itself
                    tr[1][j, 2 - i] = data; //+90
                    tr[2][2 - i, 2 - j] = data; //+180
                    tr[3][2 - j, i] = data; //+270

                    tr[4][i, 2 - j] = data; //Horizontal flip
                    tr[5][2 - i, j] = data; //Vertical flip
                }
            }
            return tr.Select(d => new Shape(d)).ToHashSet();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is not Shape other) return false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Data[i, j] != other.Data[i, j]) return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            int hash = 1;
            foreach (var v in Data)
            {
                hash = (hash << 1) | (v ? 1 : 0);
            }
            return hash;
        }
        public static bool operator ==(Shape left, Shape right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Shape left, Shape right)
        {
            return !(left == right);
        }
    }

    readonly struct Board
    {
        public int R { get; }
        public int C { get; }
        public int[,] Data { get; }
        public int[] ShapeCounts { get; }
        public Board(string input)
        {
            string[] info = input.Split(':');
            var dim = info[0].Split('x').Select(int.Parse).ToArray();

            Data = new int[dim[1], dim[0]];
            ShapeCounts = [.. info[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)];

            R = Data.GetLength(0);
            C = Data.GetLength(1);
        }

        public bool Accepts(IList<Shape> shapes)
        {
            var list = ShapeCounts.SelectMany((c, i) => Enumerable.Range(1, c).Select(_ => shapes[i])).ToArray();
            var res = CanBePlaced(0, list, list.Length);
            Console.WriteLine(ToString());
            return res;
        }

        private bool CanBePlaced(int si, Shape[] list, int count)
        {
            if (si >= count)
            {
                return true;
            }

            var current = list[si];

            var transforms = current.Transform();
            foreach (var tr in transforms)
            {
                for (int i = 0; i < R; ++i)
                {
                    for (int j = 0; j < C; ++j)
                    {
                        if (Data[i, j] != 0) continue;

                        if (!TryEmplace(tr, i, j))
                        {
                            Revert(tr, i, j);
                            continue;
                        }

                        if (CanBePlaced(si + 1, list, count))
                        {
                            return true;
                        }

                        Revert(tr, i, j);
                    }
                }
            }

            return false;
        }

        private void Revert(Shape tr, int x, int y)
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (!tr[i, j]) continue;
                    var tx = i + x;
                    var ty = j + y;

                    if (tx >= R || ty >= C)
                    {
                        continue;
                    }
                    Data[tx, ty] -= 1;
                }
            }
        }

        private bool TryEmplace(Shape tr, int x, int y)
        {
            bool res = true;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (!tr[i, j]) continue;
                    var tx = i + x;
                    var ty = j + y;

                    if (tx >= R || ty >= C)
                    {
                        res = false;
                    }
                    else
                    {
                        if (Data[tx, ty] != 0)
                        {
                            res = false;
                        }
                        Data[tx, ty] += 1;
                    }
                }
            }

            return res;
        }

        public override readonly string ToString()
        {
            var lines = new List<string>();
            int r = Data.GetLength(0);
            int c = Data.GetLength(1);
            for (int i = 0; i < r; i++)
            {
                var line = "";
                for (int j = 0; j < c; j++)
                {
                    line += Data[i, j].ToString("0");
                }
                lines.Add(line);
            }
            return string.Join(Environment.NewLine, [$"({r}x{c})", .. lines]);
        }
    }
}
