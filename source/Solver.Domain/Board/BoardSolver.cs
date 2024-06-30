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

            void UpdateTriple((MutableCell one, MutableCell two, MutableCell three, CellValue value1, CellValue value2, CellValue value3) triple)
            {
                triple.one.RemainingCellValues.Clear();
                triple.one.RemainingCellValues.Add(triple.value1);
                triple.one.RemainingCellValues.Add(triple.value2);
                triple.one.RemainingCellValues.Add(triple.value3);
                triple.two.RemainingCellValues.Clear();
                triple.two.RemainingCellValues.Add(triple.value1);
                triple.two.RemainingCellValues.Add(triple.value2);
                triple.two.RemainingCellValues.Add(triple.value3);
                triple.three.RemainingCellValues.Clear();
                triple.three.RemainingCellValues.Add(triple.value1);
                triple.three.RemainingCellValues.Add(triple.value2);
                triple.three.RemainingCellValues.Add(triple.value3);
            }
            if (row.TryGetHidden(out var rowPair))
            {
                if (rowPair!.Value.pair != null) UpdatePair(rowPair.Value.pair.Value);
                if (rowPair!.Value.triple != null) UpdateTriple(rowPair.Value.triple.Value);
                return true;
            }

            var tempColumn = columns[rowIndex];
            if (tempColumn.TryGetHidden(out var columnPair))
            {
                if (columnPair!.Value.pair != null) UpdatePair(columnPair.Value.pair.Value);
                if (columnPair!.Value.triple != null) UpdateTriple(columnPair.Value.triple.Value);
                return true;
            }

            var tempRegionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(rowIndex);
            var tempRegion = regions[tempRegionCoordinates.rowIndex, tempRegionCoordinates.columnIndex];
            if (tempRegion.TryGetHidden(out var regionPair))
            {
                if (regionPair!.Value.pair != null) UpdatePair(regionPair.Value.pair.Value);
                if (regionPair!.Value.triple != null) UpdateTriple(regionPair.Value.triple.Value);
                return true;
            }

            void UpdatePair((MutableCell one, MutableCell two, CellValue value1, CellValue value2) pair)
            {
                pair.one.RemainingCellValues.Clear();
                pair.one.RemainingCellValues.Add(pair.value1);
                pair.one.RemainingCellValues.Add(pair.value2);
                pair.two.RemainingCellValues.Clear();
                pair.two.RemainingCellValues.Add(pair.value1);
                pair.two.RemainingCellValues.Add(pair.value2);
                
            }
        }

        return false;
    }

}