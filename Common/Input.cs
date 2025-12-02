using System.Text;

namespace Common;

public class Input(string FileName)
{
    public IEnumerable<string?> Lines()
    {
        using var reader = new StreamReader(FileName, Encoding.UTF8);
        while (!reader.EndOfStream)
        {
            yield return reader.ReadLine();
        }
    }
    public IEnumerable<string?> Next(ISet<string> separators)
    {
        using var reader = new StreamReader(FileName, Encoding.UTF8);
        StringBuilder data = new();
        while (!reader.EndOfStream)
        {
            var value = reader.Read();
            if (value == -1)
            {
                yield break;
            }
            var c = ((char)value).ToString();
            if (!separators.Contains(c))
            {
                data.Append((char)value);
                continue;
            }
            if (data.Length <= 0)
            {
                continue;
            }
            yield return data.ToString();
            data.Clear();
        }
        if (data.Length > 0)
        {
            yield return data.ToString();
        }
    }
}
