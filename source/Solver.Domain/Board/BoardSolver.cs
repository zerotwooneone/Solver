using System.Collections.Immutable;
using System.Diagnostics;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class BoardSolver
{
    public GameBoard GetSolvedBoard(GameBoard board)
    {
        if (!board.GetIsValid())
        {
            throw new ArgumentException("the board is not valid");
        }
        if (board.GetIsSolved())
        {
            return board;
        }

        const int sanityMaximum = 10000;
        int sanityCount = 0;
        var (rows, columns, regions) = GetMutable(board);
        bool doRecheck;
        do
        {
            doRecheck = TryReduce(rows, columns, regions);
            if (sanityCount++ > sanityMaximum)
            {
                throw new Exception("sanity check exceeded");
            }
        } while (doRecheck);

        return new GameBoard(rows, columns, regions);
    }

    private (IReadOnlyList<MutableNineCell> rows, IReadOnlyList<MutableNineCell> columns, MutableRegionCollection regions) GetMutable(GameBoard board)
    {
        var columns = Enumerable.Range(0,9).Select(i=> new MutableNineCell(i)).ToArray();
        var regions = new MutableRegionCollection(Enumerable.Range(0,9).Select(i=> new MutableNineCell(i)));
        var rows = board.Rows.Select((r,rowIndex)=>
        {
            var row = new MutableNineCell(rowIndex);
            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
            {
                var column = columns[columnIndex];
                var regionCoordinates = RegionHelper.GetRegionCoordinates(rowIndex, columnIndex);
                var region = regions[regionCoordinates.rowIndex, regionCoordinates.columnIndex];
                
                var cell = new MutableCell(
                    r[columnIndex].Value, 
                    r[columnIndex].RemainingCellValues,
                    row,
                    column,
                    region);
                row[columnIndex] = cell;
                CellValue? cellValue = cell.Value;
                
                column[rowIndex] = cell;
                

                var indexWithinRegion = RegionHelper.GetIndexWithinRegion(rowIndex, columnIndex);
                
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
        bool hasChanged = false;
        const int boardSize = 9;
        for (var rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            var row = rows[rowIndex];
            
            for (var columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                var cell = rows[rowIndex][columnIndex];
                hasChanged = cell.TryReduce() || hasChanged;
            }

            bool HandleHiddenRemaining(NineCellExtensions.HiddenRemaining hiddenRemaining)
            {
                switch (hiddenRemaining.Cells.Count)
                {
                    case 1:
                        return hiddenRemaining.Cells.First().SetCellAsSolved(hiddenRemaining.ToRemoveFromEach.First()) || hasChanged;
                        break;
                    default:
                        bool didChange = false;
                        var cells = hiddenRemaining.Cells;
                        foreach (var cell in cells)
                        {
                            didChange = cell.TryReduceRemaining(hiddenRemaining.ToRemoveFromEach) || didChange;
                        }

                        return didChange;
                        break;
                }
            }

            if (row.TryGetHidden(out var rowPair))
            {
                hasChanged = HandleHiddenRemaining(rowPair.Value) || hasChanged;
            }

            var tempColumn = columns[rowIndex];
            if (tempColumn.TryGetHidden(out var columnPair))
            {
                hasChanged = HandleHiddenRemaining(columnPair.Value) || hasChanged;
            }

            var tempRegionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(rowIndex);
            var tempRegion = regions[tempRegionCoordinates.rowIndex, tempRegionCoordinates.columnIndex];
            if (tempRegion.TryGetHidden(out var regionPair))
            {
                hasChanged = HandleHiddenRemaining(regionPair.Value) || hasChanged;
            }
        }

        return hasChanged;
    }

    
}