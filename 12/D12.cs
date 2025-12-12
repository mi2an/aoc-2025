using Common;

namespace _12;

public class D12 : Solver
{
    public override int Day => 12;
    public override bool Test => true;

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
        for (i = 1; i < data.Length; i += 4)
        {
            if (data[i].Contains('x'))
            {
                --i;
                break;
            }

            shapes.Add(new Shape(data[i..(i + 3)]));
        }
        IEnumerable<Board> boardData = data[i..].Select(line => new Board(line));
        return (shapes, boardData);
    }


    class Shape
    {
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

        public bool TryEmplace(int x, int y, bool[,] board, out List<bool[,]>? result)
        {
            result = null;

            var rotations = Transformations(this).ToArray();

            var rb = board.GetLength(0);
            var cb = board.GetLength(1);

            result = rotations.Select(shape =>
            {
                var newBoard = (bool[,])board.Clone();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!shape.Data[i, j]) continue;

                        var tx = x + i;
                        var ty = y + j;
                        if (tx >= rb || ty >= cb || board[tx, ty])
                        {
                            return null;
                        }
                        newBoard[tx, ty] = true;
                    }
                }
                return newBoard;
            }).Where(r => r is not null).ToList()!;

            return result.Count > 0;
        }

        private static HashSet<Shape> Transformations(Shape shape)
        {
            var tr = Enumerable.Range(0, 6).Select(_ => new bool[3, 3]).ToArray();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var data = shape.Data[i, j];
                    tr[0][i, j] = data;
                    tr[1][j, 2 - i] = data;
                    tr[2][2 - i, 2 - j] = data;
                    tr[3][2 - j, i] = data;

                    tr[4][i, 2 - j] = data;
                    tr[5][2 - i, j] = data;
                }
            }
            return [.. tr.Select(d => new Shape(d))];
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
        public bool[,] Data { get; }
        public int[] ShapeCounts { get; }
        public Board(string input)
        {
            string[] info = input.Split(':');
            var dim = info[0].Split('x').Select(int.Parse).ToArray();

            Data = new bool[dim[0], dim[1]];
            ShapeCounts = [.. info[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)];
        }

        public bool Accepts(IList<Shape> shapes)
        {
            throw new NotImplementedException();
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
                    line += Data[i, j] ? '#' : '.';
                }
                lines.Add(line);
            }
            return string.Join(Environment.NewLine, [$"({r}x{c})", .. lines]);
        }
    }
}
