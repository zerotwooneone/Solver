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
        return (gameBoard: new GameBoard(cells), doRecheck);
    }
}