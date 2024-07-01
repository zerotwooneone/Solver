using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class BoardBuilder
{
    public GameBoard CreateFrom9x9Csv(string path, string separator = ",")
    {
        var file = new FileInfo(path);
        if (!file.Exists)
        {
            throw new FileNotFoundException();
        }

        using var stream = file.OpenRead();
        using var streamReader = new StreamReader(stream);
        string? line = string.Empty;
        
        var cells = new List<ICell>();
        while ((line = streamReader.ReadLine()) != null)
        {
            var split = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in split)
            {
                if (int.TryParse(s, out var intValue))
                {
                    cells.Add(new SolvedCell(intValue));
                }
                else
                {
                    cells.Add(UnsolvedCell.Instance);
                }
            }
        }

        return CreateFromRowMajorOrder(cells);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cells">cells must be in row major order</param>
    /// <exception cref="ArgumentException"></exception>
    public GameBoard CreateFromRowMajorOrder(IEnumerable<ICell> cells)
    {
        return CreateFromRowMajorOrder(cells as ICell[] ?? cells.ToArray());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cells">cells must be in row major order</param>
    /// <exception cref="ArgumentException"></exception>
    public GameBoard CreateFromRowMajorOrder(params ICell[] cells)
    {
        var groups = GetGroups(cells);
        return new GameBoard(groups.rows, groups.columns, groups.regions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cells">cells must be in row major order</param>
    /// <exception cref="ArgumentException"></exception>
    public (IReadOnlyList<IRow> rows, IReadOnlyList<IColumn> columns, IRegionCollection regions) GetGroups(params ICell[] cells)
    {
        const int boardSize = 9;
        if (cells.Length != (boardSize*boardSize))
        {
            throw new ArgumentException("must be 81 cells");
        }

        var rowArray = new NineCell[boardSize];
        var columnArray = new NineCell[boardSize];
        
        var regionArray = new NineCell[boardSize];

        for (int positionIndex = 0; positionIndex < boardSize; positionIndex++)
        {
            rowArray[positionIndex] = new NineCell(cells[(boardSize * positionIndex)],
                cells[(boardSize * positionIndex) + 1],
                cells[(boardSize * positionIndex) + 2],
                cells[(boardSize * positionIndex) + 3],
                cells[(boardSize * positionIndex) + 4],
                cells[(boardSize * positionIndex) + 5],
                cells[(boardSize * positionIndex) + 6],
                cells[(boardSize * positionIndex) + 7],
                cells[(boardSize * positionIndex) + 8]);

            columnArray[positionIndex] = new NineCell(cells[positionIndex],
                cells[positionIndex + boardSize],
                cells[positionIndex + 2 * boardSize],
                cells[positionIndex + 3 * boardSize],
                cells[positionIndex + 4 * boardSize],
                cells[positionIndex + 5 * boardSize],
                cells[positionIndex + 6 * boardSize],
                cells[positionIndex + 7 * boardSize],
                cells[positionIndex + 8 * boardSize]);

            var regionCoordinates = RegionHelper.GetRegionCoordinatesFromRowMajorOrder(positionIndex);
            var row1Start = (regionCoordinates.rowIndex *27) + (regionCoordinates.columnIndex * 3);
            var row2Start = row1Start+9;
            var row3Start = row2Start+9;
            
            regionArray[positionIndex]= new NineCell(cells[row1Start],
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