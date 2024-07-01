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
        bool hasChanged = false;
        const int boardSize = 9;
        for (int rowIndex = 0; rowIndex < boardSize; rowIndex++)
        {
            var row = rows[rowIndex];
            
            for (int columnIndex = 0; columnIndex < boardSize; columnIndex++)
            {
                Debug.WriteLine($"Starting cell r{rowIndex},c{columnIndex}");
                var cell = rows[rowIndex][columnIndex];
                hasChanged = cell.TryReduce() || hasChanged;
            }

            Debug.WriteLine($"Starting to look for hidden");
            Debug.WriteLine("");
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
                    hasChanged = true;
                }
                if (rowPair.Value.pair != null && UpdatePair(rowPair.Value.pair.Value))
                {
                    hasChanged = true;
                }

                if (rowPair.Value.triple != null && UpdateTriple(rowPair.Value.triple.Value))
                {
                    hasChanged = true;
                }
            }

            var tempColumn = columns[rowIndex];
            if (tempColumn.TryGetHidden(out var columnPair))
            {
                if (columnPair!.Value.single != null && columnPair.Value.single.Value.one.SetCellAsSolved(columnPair.Value.single.Value.value1.Value))
                {
                    hasChanged = true;
                }
                if (columnPair.Value.pair != null && UpdatePair(columnPair.Value.pair.Value))
                {
                    hasChanged = true;
                }

                if (columnPair.Value.triple != null && UpdateTriple(columnPair.Value.triple.Value))
                {
                    hasChanged = true;
                }
            }

            var tempRegionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(rowIndex);
            var tempRegion = regions[tempRegionCoordinates.rowIndex, tempRegionCoordinates.columnIndex];
            if (tempRegion.TryGetHidden(out var regionPair))
            {
                if (regionPair!.Value.single != null && regionPair.Value.single.Value.one.SetCellAsSolved(regionPair.Value.single.Value.value1.Value))
                {
                    hasChanged = true;
                }
                if (regionPair.Value.pair != null && UpdatePair(regionPair.Value.pair.Value))
                {
                    hasChanged = true;
                }

                if (regionPair.Value.triple != null && UpdateTriple(regionPair.Value.triple.Value))
                {
                    hasChanged = true;
                }
            }

            bool UpdatePair((MutableCell one, MutableCell two, CellValue value1, CellValue value2) pair)
            {
                var changed = pair.one.TryReduceRemaining(pair.value1, pair.value2);
                changed = pair.two.TryReduceRemaining(pair.value1, pair.value2) || changed;
                return changed;
            }
            
            Debug.WriteLine("end of hidden check");
            Debug.WriteLine("");
        }

        return hasChanged;
    }

    
}