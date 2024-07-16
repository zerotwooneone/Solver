using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class BoardBuilder
{
    const int BoardSize = 9;
    public GameBoard CreateFrom9x9Csv(string path, string separator = ",")
    {
        var file = new FileInfo(path);
        if (!file.Exists)
        {
            throw new FileNotFoundException();
        }

        using var stream = file.OpenRead();
        using var streamReader = new StreamReader(stream);
        string? line;
        
        var rows = Enumerable.Range(0,BoardSize).Select(i=> new MutableNineCell(i)).ToArray();
        var columns = Enumerable.Range(0,BoardSize).Select(i=> new MutableNineCell(i)).ToArray();
        var regions = new MutableRegionCollection(Enumerable.Range(0,BoardSize).Select(i=> new MutableNineCell(i)));

        var cellCount = 0;
        while ((line = streamReader.ReadLine()) != null)
        {
            var split = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in split)
            {
                var rowIndex = cellCount/BoardSize;
                var columnIndex = cellCount%BoardSize;
                cellCount++;
                
                var row = rows[rowIndex];
                var column = columns[columnIndex];
                var r = RegionHelper.GetRegionCoordinates(rowIndex, columnIndex);
                var region = regions[r.rowIndex, r.columnIndex];

                var canParse = int.TryParse(s, out var parsedValue);
                CellValue? initialValue = canParse ? parsedValue : null;
                var remainingValues = canParse
                    ? MutableCell.EmptyRemainingValues
                    : MutableCell.GetAllCellValues();
                var cell = new MutableCell(
                    initialValue,
                    remainingValues,
                    row,
                    column,
                    region);
                
                row[columnIndex] = cell;
                column[rowIndex] = cell;
                
                var innerRegionCoordinates = RegionHelper.GetIndexWithinRegion(rowIndex, columnIndex);
                (region)[innerRegionCoordinates.rowIndex, innerRegionCoordinates.columnIndex] = cell;
            }
        }

        return new GameBoard(rows, columns, regions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cells">cells must be in row major order</param>
    /// <exception cref="ArgumentException"></exception>
    public (IReadOnlyList<IRow> rows, IReadOnlyList<IColumn> columns, IRegionCollection regions) GetGroups(params ICell[] cells)
    {
        if (cells.Length != (BoardSize*BoardSize))
        {
            throw new ArgumentException("must be 81 cells");
        }

        var rowArray = new NineCell[BoardSize];
        var columnArray = new NineCell[BoardSize];
        
        var regionArray = new NineCell[BoardSize];

        for (int positionIndex = 0; positionIndex < BoardSize; positionIndex++)
        {
            rowArray[positionIndex] = new NineCell(positionIndex,cells[(BoardSize * positionIndex)],
                cells[(BoardSize * positionIndex) + 1],
                cells[(BoardSize * positionIndex) + 2],
                cells[(BoardSize * positionIndex) + 3],
                cells[(BoardSize * positionIndex) + 4],
                cells[(BoardSize * positionIndex) + 5],
                cells[(BoardSize * positionIndex) + 6],
                cells[(BoardSize * positionIndex) + 7],
                cells[(BoardSize * positionIndex) + 8]);

            columnArray[positionIndex] = new NineCell(positionIndex,cells[positionIndex],
                cells[positionIndex + BoardSize],
                cells[positionIndex + 2 * BoardSize],
                cells[positionIndex + 3 * BoardSize],
                cells[positionIndex + 4 * BoardSize],
                cells[positionIndex + 5 * BoardSize],
                cells[positionIndex + 6 * BoardSize],
                cells[positionIndex + 7 * BoardSize],
                cells[positionIndex + 8 * BoardSize]);

            var regionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(positionIndex);
            var row1Start = (regionCoordinates.rowIndex *27) + (regionCoordinates.columnIndex * 3);
            var row2Start = row1Start+BoardSize;
            var row3Start = row2Start+BoardSize;
            
            regionArray[positionIndex]= new NineCell(positionIndex,cells[row1Start],
                cells[row1Start + 1],
                cells[row1Start + 2],
                cells[row2Start],
                cells[row2Start + 1],
                cells[row2Start + 2],
                cells[row3Start],
                cells[row3Start + 1],
                cells[row3Start + 2]);
        }

        return (rows: rowArray,
            columns: columnArray,
            regions: new RegionCollection(regionArray));
    }
}