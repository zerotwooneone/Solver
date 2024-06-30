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
                var region = regions[regionCoordinates.rowIndex, regionCoordinates.columnIndex];
                region[indexWithinRegion.rowIndex, indexWithinRegion.columnIndex] = cell;

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
                var region = regions[regionCoordinates.rowIndex, regionCoordinates.columnIndex];

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

                if (cell.RemainingCellValues.Count == 1)
                {
                    var value = cell.RemainingCellValues.First();
                    SetCellAsSolved(value, cell);
                    return true;
                }

                var remaining = new HashSet<CellValue>( row.Remaining
                    .Intersect(column.Remaining)
                    .Intersect(region.Remaining));

                if (remaining.Count == 0)
                {
                    throw new NotImplementedException("no solution found");
                }

                if (remaining.Count == 1)
                {
                    SetCellAsSolved(remaining.First(), cell);
                    return true;
                }

                if (remaining.Count < 9 && 
                    cell.RemainingCellValues.Aggregate(false,(hasChanged,currentCell)=>
                    {
                        var didChange = (!remaining.Contains(currentCell) &&
                                      cell.RemainingCellValues.Remove(currentCell));
                        return hasChanged || didChange;
                    }))
                {
                    return true;
                }
            }
            
            if (row.TryGetHiddenPair(out var rowPair))
            {
                rowPair.Value.one.RemainingCellValues.Clear();
                rowPair.Value.one.RemainingCellValues.Add(rowPair.Value.value1);
                rowPair.Value.one.RemainingCellValues.Add(rowPair.Value.value2);
                rowPair.Value.two.RemainingCellValues.Clear();
                rowPair.Value.two.RemainingCellValues.Add(rowPair.Value.value1);
                rowPair.Value.two.RemainingCellValues.Add(rowPair.Value.value2);
                return true;
            }

            var tempColumn = columns[rowIndex];
            if (tempColumn.TryGetHiddenPair(out var columnPair))
            {
                columnPair.Value.one.RemainingCellValues.Clear();
                columnPair.Value.one.RemainingCellValues.Add(columnPair.Value.value1);
                columnPair.Value.one.RemainingCellValues.Add(columnPair.Value.value2);
                columnPair.Value.two.RemainingCellValues.Clear();
                columnPair.Value.two.RemainingCellValues.Add(columnPair.Value.value1);
                columnPair.Value.two.RemainingCellValues.Add(columnPair.Value.value2);
                return true;
            }

            var tempRegionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(rowIndex);
            var tempRegion = regions[tempRegionCoordinates.rowIndex, tempRegionCoordinates.columnIndex];
            if (tempRegion.TryGetHiddenPair(out var regionPair))
            {
                regionPair.Value.one.RemainingCellValues.Clear();
                regionPair.Value.one.RemainingCellValues.Add(regionPair.Value.value1);
                regionPair.Value.one.RemainingCellValues.Add(regionPair.Value.value2);
                regionPair.Value.two.RemainingCellValues.Clear();
                regionPair.Value.two.RemainingCellValues.Add(regionPair.Value.value1);
                regionPair.Value.two.RemainingCellValues.Add(regionPair.Value.value2);
                return true;
            }
        }

        return false;
    }

}