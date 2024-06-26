using System.Collections.Immutable;
using SolverConsole.Cell;

namespace SolverConsole.Board;

public class GameBoard
{
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
        var ranges = new List<List<ICell>>(rowSide);
        for (int rangneRowIndex = 0; rangneRowIndex < rowSide; rangneRowIndex++)
        {
            ranges.Add(new List<ICell>(rowSide)); 
        }

        
        for (int positionIndex = 0; positionIndex < rowSide; positionIndex++)
        {
            row0.Add(cells[positionIndex]);
            row1.Add(cells[positionIndex+9]);
            row2.Add(cells[positionIndex+18]);
            row3.Add(cells[positionIndex+27]);
            row4.Add(cells[positionIndex+36]);
            row5.Add(cells[positionIndex+45]);
            row6.Add(cells[positionIndex+54]);
            row7.Add(cells[positionIndex+63]);
            row8.Add(cells[positionIndex+72]);

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
                _=> throw new ArgumentOutOfRangeException()
            };

            var outerColumnIndex = positionIndex;
            for (int outerRowIndex = 0; outerRowIndex < rowSide; outerRowIndex++)
            {
                var cellIndex = outerColumnIndex+outerRowIndex*rowSide;
                var cell = cells[cellIndex];
                column.Add(cell);
                
                var rangeIndex = ((outerRowIndex / 3)*3) + (outerColumnIndex/3);
                ranges[rangeIndex].Add(cell);
            }
            
        }

        Rows = [new Row(row0), new(row1), new(row2), new(row3), new(row4), new(row5), new(row6), new(row7), new(row8)];
        Columns = [new Column(column0), new(column1), new(column2), new(column3), new(column4), new(column5), new(column6), new(column7), new(column8)];
        Regions = [..ranges.Select(l=>new ThreeByThree(l)).ToArray()];
    }
    /// <summary>
    /// indexed from top to bottom
    /// </summary>
    public ImmutableArray<Row> Rows { get; }
    /// <summary>
    /// indexed from left to right
    /// </summary>
    public ImmutableArray<Column> Columns { get; }
    /// <summary>
    /// indexed from left to right and then top to bottom
    /// </summary>
    public ImmutableArray<ThreeByThree> Regions { get; }
}