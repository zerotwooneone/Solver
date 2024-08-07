﻿using System.Collections.Immutable;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class BoardSolver
{
    const int boardSize = 9;
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
        TrySolve(rows);
        do
        {
            doRecheck = TryFindHidden(rows, columns, regions);
            doRecheck = TrySolve(rows) || doRecheck;
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

    private static bool TrySolve(
        IReadOnlyList<MutableNineCell> rows)
    {
        bool hasChanged = false;
        
        for (var rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                var cell = rows[rowIndex][columnIndex];
                
                hasChanged = cell.UpdateSolveState();
                hasChanged = cell.TryReduceRemaining() || hasChanged;
                hasChanged = cell.TrySolveFromRemaining() || hasChanged;
            }
        }

        return hasChanged;
    }

    private static bool TryFindHidden(IReadOnlyList<MutableNineCell> rows, IReadOnlyList<MutableNineCell> columns, MutableRegionCollection regions)
    {
        bool hasChanged = false;
        for (int positionIndex = 0; positionIndex < boardSize; positionIndex++)
        {
            var row = rows[positionIndex];
            var rowCorners = (new[]
                {
                    RegionHelper.GetRegionCoordinates(positionIndex, 0), 
                    RegionHelper.GetRegionCoordinates(positionIndex, 3),
                    RegionHelper.GetRegionCoordinates(positionIndex, 8)
                })
                .Select(t => regions[t.rowIndex, t.columnIndex]);
            foreach (var rowCorner in rowCorners)
            {
                var otherRegionCells =
                    ((IReadOnlyList<MutableCell>) rowCorner).Where(c => !row.Contains(c)).ToArray();
                var regionCells = ((IReadOnlyList<MutableCell>) rowCorner).Where(c => !otherRegionCells.Contains(c))
                    .ToArray();
                var otherCells = ((IReadOnlyList<MutableCell>) row).Where(c => !regionCells.Contains(c)).ToArray();
                if (NineCellExtensions.TryGetPointing(regionCells, otherRegionCells, otherCells,
                        out var rowHiddenRemaining))
                {
                    hasChanged = rowHiddenRemaining.Aggregate(hasChanged, (current, hiddenRemaining) => HandleCellsToUpdate(hiddenRemaining) || current);
                }
                if (NineCellExtensions.TryGetPointing(regionCells,  otherCells,otherRegionCells,
                        out var rowHiddenRemaining2))
                {
                    hasChanged = rowHiddenRemaining2.Aggregate(hasChanged, (current, hiddenRemaining) => HandleCellsToUpdate(hiddenRemaining) || current);
                }
            }

            if (row.TryGetHidden(out var rowPair))
            {
                hasChanged = HandleCellsToUpdate(rowPair.Value) || hasChanged;
            }
            
            if (row.TryGetNaked(out var rowNaked))
            {
                foreach (var hiddenRemaining in rowNaked)
                {
                    hasChanged = HandleCellsToUpdate(hiddenRemaining) || hasChanged;
                }
            }

            var tempColumn = columns[positionIndex];
            var columnCorners = (new[]
                {
                    RegionHelper.GetRegionCoordinates(0, positionIndex), 
                    RegionHelper.GetRegionCoordinates(3, positionIndex),
                    RegionHelper.GetRegionCoordinates(8, positionIndex)
                })
                .Select(t => regions[t.rowIndex, t.columnIndex]);
            foreach (var columnCornerCorner in columnCorners)
            {
                var otherRegionCells = ((IReadOnlyList<MutableCell>) columnCornerCorner)
                    .Where(c => !tempColumn.Contains(c)).ToArray();
                var regionCells = ((IReadOnlyList<MutableCell>) columnCornerCorner)
                    .Where(c => !otherRegionCells.Contains(c)).ToArray();
                var otherCells = ((IReadOnlyList<MutableCell>) tempColumn).Where(c => !regionCells.Contains(c))
                    .ToArray();
                if (NineCellExtensions.TryGetPointing(regionCells, otherRegionCells, otherCells,
                        out var columnHiddenRemaining))
                {
                    hasChanged = columnHiddenRemaining.Aggregate(hasChanged, (current, hiddenRemaining) => HandleCellsToUpdate(hiddenRemaining) || current);
                }
                if (NineCellExtensions.TryGetPointing(regionCells,  otherCells,otherRegionCells,
                        out var columnHiddenRemaining2))
                {
                    hasChanged = columnHiddenRemaining2.Aggregate(hasChanged, (current, hiddenRemaining) => HandleCellsToUpdate(hiddenRemaining) || current);
                }
            }

            if (tempColumn.TryGetHidden(out var columnPair))
            {
                hasChanged = HandleCellsToUpdate(columnPair.Value) || hasChanged;
            }
            
            if (tempColumn.TryGetNaked(out var columnNaked))
            {
                foreach (var hiddenRemaining in columnNaked)
                {
                    hasChanged = HandleCellsToUpdate(hiddenRemaining) || hasChanged;
                }
            }

            var tempRegionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(positionIndex);
            var tempRegion = regions[tempRegionCoordinates.rowIndex, tempRegionCoordinates.columnIndex];
            if (tempRegion.TryGetHidden(out var regionPair))
            {
                hasChanged = HandleCellsToUpdate(regionPair.Value) || hasChanged;
            }
            
            if (tempRegion.TryGetNaked(out var regionNaked))
            {
                foreach (var hiddenRemaining in regionNaked)
                {
                    hasChanged = HandleCellsToUpdate(hiddenRemaining) || hasChanged;
                }
            }
        }

        return hasChanged;
    }
    
    private static bool HandleCellsToUpdate(NineCellExtensions.CellsToUpdate cellsToUpdate)
    {
        if (cellsToUpdate.Cells.Count == 1 &&
            cellsToUpdate.NewRemainingValues.Count == 1)
        {
            return ((MutableCell)cellsToUpdate.Cells.First())
                .TrySetCellAsSolved(cellsToUpdate.NewRemainingValues.First());
        }
        bool didChange = false;
        var cells = cellsToUpdate.Cells;
        foreach (var cell in cells.Cast<MutableCell>())
        {
            didChange = cell.TryReduceRemaining(cellsToUpdate.NewRemainingValues) || didChange;
            didChange = cell.TrySolveFromRemaining() || didChange;
        }

        return didChange;
    }
}