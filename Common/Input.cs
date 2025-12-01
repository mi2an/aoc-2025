namespace Common;

public class Input(string FileName)
{
    public IEnumerable<string?> Lines()
    {
        using var reader = new StreamReader(FileName);
        while (!reader.EndOfStream)
        {
            yield return reader.ReadLine();
        }
    }
}
