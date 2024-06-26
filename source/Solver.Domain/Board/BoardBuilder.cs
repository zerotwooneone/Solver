using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class BoardBuilder
{
    public GameBoard CreateFrom9x9Csv(string path, string separator = ",")
    {
        var file = new FileInfo(path);
        if (!file.Exists)
        {
            throw new FileNotFoundException();
        }

        using var stream = file.OpenRead();
        using var streamReader = new StreamReader(stream);
        string? line = string.Empty;
        
        var cells = new List<ICell>();
        while ((line = streamReader.ReadLine()) != null)
        {
            var split = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in split)
            {
                if (int.TryParse(s, out var intValue))
                {
                    cells.Add(new SolvedCell(intValue));
                }
                else
                {
                    cells.Add(UnsolvedCell.Instance);
                }
            }
        }

        return new GameBoard(cells);
    } 
}