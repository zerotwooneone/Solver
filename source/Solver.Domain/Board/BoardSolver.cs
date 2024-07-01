using System.Collections.Immutable;
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
        var doRecheck = false;
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
        var columns = Enumerable.Range(0,9).Select(_=> new MutableNineCell()).ToArray();
        var regions = new MutableRegionCollection(Enumerable.Range(0,9).Select(_=> new MutableNineCell()));
        var rows = board.Rows.Select((r,rowIndex)=>
        {
            var row = new MutableNineCell();
            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
            {
                var column = columns[columnIndex];
                var regionCoordinates = RegionHelper.GetRegionCoordinates(rowIndex, columnIndex);
                var region = regions[regionCoordinates.rowIndex, regionCoordinates.columnIndex];
                
                var cell = new MutableCell(
                    r[columnIndex].Value, 
                    r[columnIndex].State.RemainingValues,
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

                if (cell.Value.HasValue)
                {
                    if (cell.SetCellAsSolved(cell.Value.Value))
                    {
                        return true;
                    }
                    continue;
                }

                if (cell.RemainingCellValues.Count == 1)
                {
                    var value = cell.RemainingCellValues.First();
                    if (cell.SetCellAsSolved(value))
                    {
                        return true;
                    }
                }

                var remaining = new HashSet<CellValue>( row.Remaining
                    .Intersect(column.Remaining)
                    .Intersect(region.Remaining));

                if (remaining.Count == 0)
                {
                    throw new NotImplementedException("no solution found");
                }

                if (remaining.Count == 1 && cell.SetCellAsSolved(remaining.First()))
                {
                    return true;
                }

                //todo: this should be cell.TryReduce(remaining)
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

            bool UpdateTriple((MutableCell one, MutableCell two, MutableCell three, CellValue value1, CellValue value2, CellValue value3) triple)
            {
                var changed = triple.one.TryReduceRemaining(triple.value1, triple.value2, triple.value3);
                changed = triple.two.TryReduceRemaining(triple.value1, triple.value2, triple.value3) || changed;
                changed = triple.three.TryReduceRemaining(triple.value1, triple.value2, triple.value3) || changed;
                return changed;
            }
            if (row.TryGetHidden(out var rowPair))
            {
                //todo: only return if SetCellAsSolved is true
                if (rowPair!.Value.single != null && rowPair.Value.single.Value.one.SetCellAsSolved(rowPair.Value.single.Value.value1.Value))
                {
                    return true;
                }
                if (rowPair!.Value.pair != null && UpdatePair(rowPair.Value.pair.Value)) return true;
                if (rowPair!.Value.triple != null && UpdateTriple(rowPair.Value.triple.Value)) return true;
            }

            var tempColumn = columns[rowIndex];
            if (tempColumn.TryGetHidden(out var columnPair))
            {
                if (columnPair!.Value.single != null && columnPair.Value.single.Value.one.SetCellAsSolved(columnPair.Value.single.Value.value1.Value))
                {
                    return true;
                }
                if (columnPair!.Value.pair != null && UpdatePair(columnPair.Value.pair.Value)) return true;
                if (columnPair!.Value.triple != null && UpdateTriple(columnPair.Value.triple.Value)) return true;
            }

            var tempRegionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(rowIndex);
            var tempRegion = regions[tempRegionCoordinates.rowIndex, tempRegionCoordinates.columnIndex];
            if (tempRegion.TryGetHidden(out var regionPair))
            {
                if (regionPair!.Value.single != null && regionPair.Value.single.Value.one.SetCellAsSolved(regionPair.Value.single.Value.value1.Value))
                {
                    return true;
                }
                if (regionPair!.Value.pair != null && UpdatePair(regionPair.Value.pair.Value)) return true;
                if (regionPair!.Value.triple != null && UpdateTriple(regionPair.Value.triple.Value)) return true;
                return true;
            }

            bool UpdatePair((MutableCell one, MutableCell two, CellValue value1, CellValue value2) pair)
            {
                var changed = pair.one.TryReduceRemaining(pair.value1, pair.value2);
                changed = pair.two.TryReduceRemaining(pair.value1, pair.value2) || changed;
                return changed;
            }
        }

        return false;
    }

}