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

        const int boardSize = 9;
        var cells = new List<ICell>();
        var regionStatsByIndex = new Dictionary<int, (HashSet<CellValue> remaining, HashSet<CellValue> solved)>();
        for (int rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            var row = board.Rows[rowIndex];
            var rowStats = row.GetStats();
            for (int columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                var column = board.Columns[columnIndex];
                var columnStats = column.GetStats();

                var cell = board[rowIndex, columnIndex];
                if (cell.Value.HasValue || !cell.State.RemainingValues.Any())
                {
                    cells.Add(cell);
                    continue;
                }

                var regionIndex = ((rowIndex / 3) * 3) + (columnIndex / 3);
                var region = board.Regions[regionIndex];
                if (!regionStatsByIndex.ContainsKey(regionIndex))
                {
                    regionStatsByIndex.Add(regionIndex, region.GetStats());
                }
                var regionStats = regionStatsByIndex[regionIndex];

                if (cell.State.RemainingValues.Count == 1)
                {
                    var v = cell.State.RemainingValues.First();
                    rowStats.solved.Add(v);
                    rowStats.remaining.Remove(v);
                    columnStats.solved.Add(v);
                    columnStats.remaining.Remove(v);
                    regionStats.solved.Add(v);
                    regionStats.remaining.Remove(v);

                    cells.Add(new SolvedCell(v));
                    continue;
                }

                var remaining = new RemainingCellValues(cell.State.RemainingValues.Where(r =>
                    !rowStats.solved.Contains(r) &&
                    !columnStats.solved.Contains(r) &&
                    !regionStats.solved.Contains(r)));

                /*var indexWithinRow = columnIndex;
                var indexWithinColumn = rowIndex;
                var indexWithinRegion = (columnIndex % 3) + (rowIndex % 3) * 3;
                var rowHs = row.TryGetHiddenSingle(indexWithinRow, out var value);
                var columnHs = column.TryGetHiddenSingle(indexWithinColumn, out value);
                var reguinHs = region.TryGetHiddenSingle(indexWithinRegion, out value);
                if (rowHs ||
                    columnHs ||
                    reguinHs)
                {
                    var x = 0;
                }*/

                if (remaining.Count == cell.State.RemainingValues.Count)
                {
                    cells.Add(cell);
                }
                else if (remaining.Count == 0)
                {
                    cells.Add(UnsolvedCell.Instance);
                }
                else if (remaining.Count == 1)
                {
                    doRecheck = true;
                    var foundSolution = remaining[0];

                    rowStats.solved.Add(foundSolution);
                    rowStats.remaining.Remove(foundSolution);
                    columnStats.solved.Add(foundSolution);
                    columnStats.remaining.Remove(foundSolution);
                    regionStats.solved.Add(foundSolution);
                    regionStats.remaining.Remove(foundSolution);

                    cells.Add(new PartialCell(null, new RemainingCellValues(foundSolution)));
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

    /// <summary>
    /// Attempts to reduce the number of remaining values in each unsolved cell.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="outputCells"></param>
    /// <returns>True when any cell's remaining values changed</returns>
    public bool TryReduceRemaining(IReadOnlyList<ICell> cells, out IReadOnlyList<ICell> outputCells)
    {
        var unsolvedCells = new List<MutableCell>(cells.Count);
        var solvedIndexes = new List<int>(cells.Count);
        var commonRemainingByValue = new Dictionary<CellValue, List<MutableCell>>();
        var resultCells = new ICell[cells.Count];
        for (int index = 0; index < cells.Count; index++)
        {
            var cell = cells[index];
            if (cell.Value != null)
            {
                solvedIndexes.Add(index);
                resultCells[index]=cell;
                continue;
            }

            // if (cell.State.RemainingValues.Count == 1)
            // {
            //     var newSolvedValue = cell.State.RemainingValues.First();
            //     resultCells[index]=new SolvedCell(newSolvedValue);
            //     foreach (var unsolvedCell in unsolvedCells)
            //     {
            //         unsolvedCell.RemainingCellValues.Remove(newSolvedValue);
            //     }
            //     continue;
            // }

            var mutableCell = (cell as MutableCell) ?? new MutableCell
            (
                index,
                cell.Value,
                cell.State.RemainingValues
            );
            unsolvedCells.Add(mutableCell);
            resultCells[index]=mutableCell;

            foreach (var remainingValue in mutableCell.RemainingCellValues)
            {
                if (!commonRemainingByValue.TryGetValue(remainingValue, out var commonRemainingCells))
                {
                    commonRemainingCells = new List<MutableCell>();
                    commonRemainingByValue.Add(remainingValue, commonRemainingCells);
                }

                commonRemainingCells.Add(mutableCell);
            }
        }

        if (solvedIndexes.Count == CellValue.AllValues.Count)
        {
            //already solved
            outputCells = Array.Empty<ICell>();
            return false;
        }

        //happens when there is only one remaining value in all unknown cells
        if (unsolvedCells.Count == 0)
        {
            outputCells = resultCells;
            return true;
        }

        // var hiddenSingles = commonRemainingByValue
        //     .Where(kvp => kvp.Value.Count == 1)
        //     .Select(kvp => (cell: kvp.Value.First(), value: kvp.Key));
        // foreach (var hiddenSingle in hiddenSingles)
        // {
        //     commonRemainingByValue.Remove(hiddenSingle.value);
        //     newSolvedCells.Add(hiddenSingle.cell);
        // }

        do
        {
            int x = 0;
        } while (TryReduceRemaining(resultCells, commonRemainingByValue, unsolvedCells));

        outputCells = resultCells;

        return true;
    }

    private bool TryReduceRemaining(
        ICell[] cells, 
        Dictionary<CellValue, List<MutableCell>> commonRemainingByValue,
        List<MutableCell> unsolvedCells)
    {
        var commonRemainingPairs = commonRemainingByValue.ToArray();
        
        for (var index = 0; index < commonRemainingPairs.Length; index++)
        {
            var kvp = commonRemainingPairs[index];
            if (kvp.Value.Count == 1)
            {
                var cell = kvp.Value.First();
                var value = kvp.Key;
                cells[cell.Index] = new SolvedCell(value);
                commonRemainingByValue.Remove(value);
                return true;
            }
        }

        foreach (var unsolvedCell in unsolvedCells)
        {
            foreach (var currentValue in unsolvedCell.RemainingCellValues)
            {
                var othersWithValue = commonRemainingByValue[currentValue].Where(c => c.Index != unsolvedCell.Index).ToArray();
            }
        }
        return false;
    }
}