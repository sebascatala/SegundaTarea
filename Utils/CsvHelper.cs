public class CSVHelper
{
    public static List<string[]> ReadCSV(string filePath)
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            return lines.Select(line => line.Split(',')).ToList();
        }
        else
        {
            throw new FileNotFoundException($"El archivo {filePath} no se encontró.");
        }
    }

    public static void WriteCSV(string filePath, List<string[]> data)
    {
        var lines = data.Select(fields => string.Join(",", fields)).ToArray();
        File.WriteAllLines(filePath, lines);
    }
}