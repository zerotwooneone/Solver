using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class BoardSolver
{
    public GameBoard GetSolvedBoard(GameBoard board)
    {
        (GameBoard GameBoard, bool doRecheck) output = (board, false);
        do
        {
            output = GetSolvedBoard_Internal(output.GameBoard);
        } while (output.doRecheck);

        return output.GameBoard;
    }

    private static (GameBoard gameBoard, bool doRecheck) GetSolvedBoard_Internal(GameBoard board)
    {
        var doRecheck = false;
        Dictionary<int, HashSet<int>> exisingByRowIndex = board.Rows
            .Select((r,i) => (i,r:new HashSet<int>(r.Where(c => c.Value.HasValue).Select(c=>(int)c.Value))))
            .ToDictionary(c=>c.i,c=>c.r);
        Dictionary<int, HashSet<int>> existingByColumnIndex = board.Columns
            .Select((r,i) => (i,r:new HashSet<int>(r.Where(c => c.Value.HasValue).Select(c=>(int)c.Value))))
            .ToDictionary(c=>c.i,c=>c.r);
        Dictionary<int, HashSet<int>> existingByRegionIndex = board.Regions
            .Select((r,i) => (i,r:new HashSet<int>(r.Where(c => c.Value.HasValue).Select(c=>(int)c.Value))))
            .ToDictionary(c=>c.i,c=>c.r);
        
        const int boardSize = 9;
        var cells = new List<ICell>();
        for (int rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            var rowStats = board.Rows[rowIndex].GetStats();
            for (int columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                var columnStats = board.Columns[columnIndex].GetStats();
                
                var cell = board[rowIndex, columnIndex];
                if (cell.Value.HasValue ||  !cell.State.RemainingValues.Any())
                {
                    cells.Add(cell);
                    continue;
                }
                
                var regionIndex = ((rowIndex / 3) * 3) + (columnIndex / 3);
                var regionStats = board.Regions[regionIndex].GetStats();

                if (cell.State.RemainingValues.Count == 1)
                {
                    var v = cell.State.RemainingValues.First();
                    rowStats.existing.Add(v);
                    rowStats.remaining.Remove(v);
                    columnStats.existing.Add(v);
                    columnStats.remaining.Remove(v);
                    regionStats.existing.Add(v);
                    regionStats.remaining.Remove(v);
                    
                    cells.Add(new SolvedCell(v));
                    continue;
                }
                
                var remaining = new RemainingCellValues(cell.State.RemainingValues.Where(r =>
                    !rowStats.existing.Contains(r) && 
                    !columnStats.existing.Contains(r) &&
                    !regionStats.existing.Contains(r)));

                if(remaining.Count == cell.State.RemainingValues.Count)
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
                    rowStats.existing.Add(foundSolution);
                    rowStats.remaining.Remove(foundSolution);
                    columnStats.existing.Add(foundSolution);
                    columnStats.remaining.Remove(foundSolution);
                    regionStats.existing.Add(foundSolution);
                    regionStats.remaining.Remove(foundSolution);
                    
                    cells.Add(new SolvedCell(foundSolution));
                }
                else 
                {
                    doRecheck = true;
                    cells.Add(new PartialCell(null, remaining));
                }
            }
        }
        return (gameBoard: new GameBoard(cells), doRecheck);
    }

    
}