using Common;

namespace _12;

public class D12 : Solver
{
    public override int Day => 12;
    public override bool Test => false;

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

        public long Area { get; }
        public Shape(string[] input)
        {
            Area = 0;
            Data = new bool[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Data[i, j] = input[i][j] == '#';
                    Area++;
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

    class Board
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

        public long Area => R * C;

        internal bool Accepts(IList<Shape> shapes)
        {
            return Area >= shapes.Select((s, i) => ShapeCounts[i] * s.Area).Sum();
        }
    }
}
