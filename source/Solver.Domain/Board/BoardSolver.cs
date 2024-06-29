using System.Collections.Immutable;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class BoardSolver
{
    public GameBoard GetSolvedBoard(GameBoard board)
    {
        if (!board.IsValid)
        {
            throw new ArgumentException("the board is not valid");
        }
        if (board.IsSolved)
        {
            return board;
        }

        var (rows, columns, regions) = GetMutable(board);
        var doRecheck = false;
        do
        {
            doRecheck = TryReduce(rows, columns, regions);
        } while (doRecheck);

        throw new NotImplementedException("return the game board here");
    }

    private (IReadOnlyList<MutableNineCell> rows, IReadOnlyList<MutableNineCell> columns, IReadOnlyList<MutableNineCell> regions) GetMutable(GameBoard board)
    {
        var columns = Enumerable.Range(0,9).Select(_=> new MutableNineCell()).ToArray();
        var regions = Enumerable.Range(0,9).Select(_=> new MutableNineCell()).ToArray();
        var rows = board.Rows.Select((r,rowIndex)=>
        {
            var row = r.GetMutableNineCell();
            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
            {
                var cell = row[columnIndex];
                CellValue? cellValue = cell.Value;
                var column = columns[columnIndex];
                column[rowIndex] = cell;
                var regionIndex = RegionHelper.GetRegionIndex(rowIndex, columnIndex);

                var indexWithinRegion = RegionHelper.GetIndexWithinRegion(rowIndex, columnIndex);
                var region = regions[regionIndex];
                region[indexWithinRegion.row, indexWithinRegion.column] = cell;

                if (cellValue.HasValue)
                {
                    column.Remaining.Remove(cellValue.Value);
                    column.Solved.Add(cellValue.Value);
                    region.Remaining.Remove(cellValue.Value);
                    region.Solved.Add(cellValue.Value);
                }
            }

            return row;
        }).ToImmutableArray();
        return (rows, 
            columns, 
            regions);
    }

    private static bool TryReduce(
        IReadOnlyList<MutableNineCell> rows, 
        IReadOnlyList<MutableNineCell> columns, 
        IReadOnlyList<MutableNineCell> regions)
    {
        var doRecheck = false;

        const int boardSize = 9;
        for (int rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            var row = rows[rowIndex];
            for (int columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                var cell = rows[rowIndex][columnIndex];
                var column = columns[columnIndex];
                
                var regionIndex = RegionHelper.GetRegionIndex(rowIndex, columnIndex);
                var region = regions[regionIndex];
                
                void SetCellAsSolved(CellValue value, MutableCell cell)
                {
                    row.SetSolved(value);
                    column.SetSolved(value);
                    region.SetSolved(value);
                    
                    cell.RemainingCellValues.Clear();
                    cell.Value = value;
                }
                
                if (cell.Value.HasValue && cell.RemainingCellValues.Any())
                {
                    SetCellAsSolved(cell.Value.Value, cell);
                    return true;
                }
                if (cell.Value.HasValue)
                {
                    row.SetSolved(cell.Value.Value);
                    column.SetSolved(cell.Value.Value);
                    region.SetSolved(cell.Value.Value);
                    continue;
                }

                if (cell.State.RemainingValues.Count ==1)
                {
                    var value = cell.State.RemainingValues[0];
                    SetCellAsSolved(value, cell);
                    return true;
                }

                var remaining = new RemainingCellValues(cell.RemainingCellValues.Where(r =>
                    !row.Solved.Contains(r) &&
                    !column.Solved.Contains(r) &&
                    !region.Solved.Contains(r)));

                if (remaining.Count == 0)
                {
                    throw new NotImplementedException("no solution found");
                }

                if (remaining.Count == 1)
                {
                    SetCellAsSolved(remaining.First(), cell);
                    return true;
                }
                if (remaining.SequenceEqual(cell.RemainingCellValues))
                {
                    continue;
                }
            }
        }

        return doRecheck;
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