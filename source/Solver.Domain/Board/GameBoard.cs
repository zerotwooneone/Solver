using System.Collections.Immutable;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class GameBoard
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cells">cells must be in row major order</param>
    /// <exception cref="ArgumentException"></exception>
    public GameBoard(IEnumerable<ICell> cells): this(cells as ICell[]?? cells.ToArray())
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cells">cells must be in row major order</param>
    /// <exception cref="ArgumentException"></exception>
    public GameBoard(params ICell[] cells)
    {
        if (cells.Length != 81)
        {
            throw new ArgumentException("must be 81 cells");
        }

        var column0 = new List<ICell>();
        var column1 = new List<ICell>();
        var column2 = new List<ICell>();
        var column3 = new List<ICell>();
        var column4 = new List<ICell>();
        var column5 = new List<ICell>();
        var column6 = new List<ICell>();
        var column7 = new List<ICell>();
        var column8 = new List<ICell>();

        var row0 = new List<ICell>();
        var row1 = new List<ICell>();
        var row2 = new List<ICell>();
        var row3 = new List<ICell>();
        var row4 = new List<ICell>();
        var row5 = new List<ICell>();
        var row6 = new List<ICell>();
        var row7 = new List<ICell>();
        var row8 = new List<ICell>();

        const int rowSide = 9;
        var region = new List<List<ICell>>(rowSide);
        for (int rangneRowIndex = 0; rangneRowIndex < rowSide; rangneRowIndex++)
        {
            region.Add(new List<ICell>(rowSide));
        }


        for (int positionIndex = 0; positionIndex < rowSide; positionIndex++)
        {
            row0.Add(cells[positionIndex]);
            row1.Add(cells[positionIndex + 9]);
            row2.Add(cells[positionIndex + 18]);
            row3.Add(cells[positionIndex + 27]);
            row4.Add(cells[positionIndex + 36]);
            row5.Add(cells[positionIndex + 45]);
            row6.Add(cells[positionIndex + 54]);
            row7.Add(cells[positionIndex + 63]);
            row8.Add(cells[positionIndex + 72]);

            var column = positionIndex switch
            {
                0 => column0,
                1 => column1,
                2 => column2,
                3 => column3,
                4 => column4,
                5 => column5,
                6 => column6,
                7 => column7,
                8 => column8,
                _ => throw new ArgumentOutOfRangeException()
            };

            var outerColumnIndex = positionIndex;
            for (int outerRowIndex = 0; outerRowIndex < rowSide; outerRowIndex++)
            {
                var cellIndex = outerColumnIndex + outerRowIndex * rowSide;
                var cell = cells[cellIndex];
                column.Add(cell);

                var regionIndex = ((outerRowIndex / 3) * 3) + (outerColumnIndex / 3);
                region[regionIndex].Add(cell);
            }

        }

        Rows = [new NineCell(row0), new NineCell(row1), new NineCell(row2), new NineCell(row3), new NineCell(row4), 
            new NineCell(row5), new NineCell(row6), new NineCell(row7), new NineCell(row8)];
        Columns =
        [
            new NineCell(column0), new NineCell(column1), new NineCell(column2), new NineCell(column3), 
            new NineCell(column4), new NineCell(column5), new NineCell(column6), new NineCell(column7), 
            new NineCell(column8)
        ];
        Regions = new RegionCollection(region.Select(l => (IRegion)new NineCell(l)));
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

    public bool IsSolved => Rows.All(r => AllTrue(r.Check())) && Columns.All(c => AllTrue(c.Check())) &&
                            Regions.All(r => AllTrue(r.Check()));

    public bool IsValid => Rows.All(r => r.Check().IsValid) && Columns.All(c => c.Check().IsValid) &&
                           Regions.All(r => r.Check().IsValid);

    private static bool AllTrue((bool, bool) b)
    {
        return b.Item1 && b.Item2;
    }
    
    public ICell this[int rowIndex, int columnIndex] => Rows[rowIndex][columnIndex];
}