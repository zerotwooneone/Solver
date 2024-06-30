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

    private (IReadOnlyList<MutableNineCell> rows, IReadOnlyList<MutableNineCell> columns, MutableRegionCollection regions) GetMutable(GameBoard board)
    {
        var columns = Enumerable.Range(0,9).Select(_=> new MutableNineCell()).ToArray();
        var regions = new MutableRegionCollection(Enumerable.Range(0,9).Select(_=> new MutableNineCell()));
        var rows = board.Rows.Select((r,rowIndex)=>
        {
            var row = r.GetMutableNineCell();
            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
            {
                var cell = row[columnIndex];
                CellValue? cellValue = cell.Value;
                var column = columns[columnIndex];
                column[rowIndex] = cell;
                var regionCoordinates = RegionHelper.GetRegionCoordinates(rowIndex, columnIndex);

                var indexWithinRegion = RegionHelper.GetIndexWithinRegion(rowIndex, columnIndex);
                var region = regions[regionCoordinates.row, regionCoordinates.column];
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
        MutableRegionCollection regions)
    {
        const int boardSize = 9;
        for (int rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            var row = rows[rowIndex];
            for (int columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                var cell = rows[rowIndex][columnIndex];
                var column = columns[columnIndex];
                
                var regionCoordinates = RegionHelper.GetRegionCoordinates(rowIndex, columnIndex);
                var region = regions[regionCoordinates.row, regionCoordinates.column];
                
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

                if (column.TryGetHiddenPair(out var pair))
                {
                    pair.Value.one.RemainingCellValues.Clear();
                    pair.Value.one.RemainingCellValues.Add(pair.Value.value1);
                    pair.Value.one.RemainingCellValues.Add(pair.Value.value2);
                    pair.Value.two.RemainingCellValues.Clear();
                    pair.Value.two.RemainingCellValues.Add(pair.Value.value1);
                    pair.Value.two.RemainingCellValues.Add(pair.Value.value2);
                    return true;
                }
            }
        }

        return false;
    }

}