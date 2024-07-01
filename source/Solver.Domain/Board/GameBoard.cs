using System.Collections.Immutable;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class GameBoard
{
    public GameBoard(IEnumerable<IRow> rows, IEnumerable<IColumn> columns, IEnumerable<IRegion> regions)
    {
        if (rows == null)
        {
            throw new ArgumentNullException(nameof(rows));
        }

        if (columns == null)
        {
            throw new ArgumentNullException(nameof(columns));
        }

        if (regions == null)
        {
            throw new ArgumentNullException(nameof(regions));
        }

        var rowArray = rows as IRow[] ?? rows.ToArray();
        if (rowArray.Length != 9)
        {
            throw new ArgumentException("must be 9 rows");
        }
        var columnArray = columns as IColumn[] ?? columns.ToArray();
        if (columnArray.Length != 9)
        {
            throw new ArgumentException("must be 9 columns");
        }
        var regionArray = regions as IRegion[] ?? regions.ToArray();
        if (regionArray.Length != 9)
        {
            throw new ArgumentException("must be 9 regions");
        }

        Rows = rowArray.ToImmutableArray();
        Columns = columnArray.ToImmutableArray();
        Regions = new RegionCollection(regionArray);
    }

    /// <summary>
    /// indexed from top to bottom
    /// </summary>
    public ImmutableArray<IRow> Rows { get; }

    /// <summary>
    /// indexed from left to right
    /// </summary>
    public ImmutableArray<IColumn> Columns { get; }

    /// <summary>
    /// indexed from left to right and then top to bottom
    /// </summary>
    public IRegionCollection Regions { get; }

    public bool GetIsSolved() =>
        Rows.All(r => AllTrue(r.Check())) && Columns.All(c => AllTrue(c.Check())) &&
        Regions.All(r => AllTrue(r.Check()));

    public bool GetIsValid()
    {
        var rowsValid = Rows.All(r => r.Check().IsValid);
        var columnsValid = Columns.All(c => c.Check().IsValid);
        var regionsValid = Regions.All(r => r.Check().IsValid);
        return rowsValid && columnsValid &&
               regionsValid;
    }

    private static bool AllTrue((bool, bool) b)
    {
        return b.Item1 && b.Item2;
    }
    
    public ICell this[int rowIndex, int columnIndex] => Rows[rowIndex][columnIndex];
}