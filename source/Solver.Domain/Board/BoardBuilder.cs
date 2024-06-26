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

        var board = new GameBoard(cells);
        return GetSolvedBoard(board);
    }

    private GameBoard GetSolvedBoard(GameBoard board)
    {
        bool doRecheck;
        

        do
        {
            doRecheck = false;
            board = GetSolvedBoard(board, ref doRecheck);
        } while (doRecheck);

        return board;
    }

    private static GameBoard GetSolvedBoard(GameBoard board, ref bool doRecheck)
    {
        var exisingByRowIndex = board.Rows
            .Select((r,i) => (i,r:new HashSet<int>(r.Where(c => c.Value.HasValue).Select(c=>(int)c.Value))))
            .ToDictionary(c=>c.i,c=>c.r);
        var existingByColumnIndex = board.Columns
            .Select((r,i) => (i,r:new HashSet<int>(r.Where(c => c.Value.HasValue).Select(c=>(int)c.Value))))
            .ToDictionary(c=>c.i,c=>c.r);
        var existingByRegionIndex = board.Regions
            .Select((r,i) => (i,r:new HashSet<int>(r.Where(c => c.Value.HasValue).Select(c=>(int)c.Value))))
            .ToDictionary(c=>c.i,c=>c.r);
        
        const int boardSize = 9;
        var cells = new List<ICell>();
        for (int rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            var existingOnRow = exisingByRowIndex[rowIndex];
            for (int columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                var cell = board[rowIndex, columnIndex];
                if (cell.Value.HasValue ||  !cell.State.RemainingValues.Any())
                {
                    cells.Add(cell);
                    continue;
                }

                if (cell.State.RemainingValues.Count() == 1)
                {
                    cells.Add(new SolvedCell(cell.State.RemainingValues.First()));
                    continue;
                }
                var existingOnColumn = existingByColumnIndex[columnIndex];
                
                var regionIndex = ((rowIndex / 3) * 3) + (columnIndex / 3);
                var existingOnRegion = existingByRegionIndex[regionIndex];
                
                var remaining = cell.State.RemainingValues.Where(r =>
                    !existingOnRow.Contains(r) && 
                    !existingOnColumn.Contains(r) &&
                    !existingOnRegion.Contains(r)).ToList();

                if(remaining.Count == cell.State.RemainingValues.Count())
                {
                    cells.Add(cell);
                }else if (remaining.Count == 0)
                {
                    cells.Add(UnsolvedCell.Instance);
                } else if (remaining.Count == 1)
                {
                    doRecheck = true;
                    var foundSolution = remaining[0];
                    
                    //update for the recheck
                    existingOnRow.Add(foundSolution);
                    existingOnColumn.Add(foundSolution);
                    existingOnRegion.Add(foundSolution);
                    
                    cells.Add(new SolvedCell(foundSolution));
                }
                else 
                {
                    doRecheck = true;
                    cells.Add(new PartialCell(null, new RemainingCellValues(remaining)));
                }
            }
        }
        return new GameBoard(cells);
    }
}