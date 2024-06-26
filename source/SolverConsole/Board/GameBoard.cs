using System.Diagnostics;
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
        var ranges = new List<ICell[]>(rowSide);
        for (int rangneRowIndex = 0; rangneRowIndex < rowSide; rangneRowIndex++)
        {
            ranges.Add(new ICell[rowSide]); 
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
            column.AddRange(cells.Skip(positionIndex*rowSide).Take(rowSide));

            var outerColumnIndex = positionIndex;
            var rangeColumnIndex = outerColumnIndex % 3;
            for (int outerRowIndex = 0; outerRowIndex < rowSide; outerRowIndex++)
            {
                var rangeIndex = ((outerRowIndex / 3)*3) + (outerColumnIndex/3);

                var innerRangeIndex = (outerRowIndex % 3)*3 +(outerColumnIndex % 3);

                var cellIndex = outerColumnIndex+outerRowIndex*rowSide;
                ranges[rangeIndex][innerRangeIndex]=cells[cellIndex];
            }
            
        }

        Rows = new Row[]{new Row(row0), new Row(row1), new Row(row2), new Row(row3), new Row(row4), new Row(row5), new Row(row6), new Row(row7), new Row(row8)};
        Columns = new Column[]{new Column(column0), new Column(column1), new Column(column2), new Column(column3), new Column(column4), new Column(column5), new Column(column6), new Column(column7), new Column(column8)};
        Regions = ranges.Select(l=>new ThreeByThree(l)).ToArray();
        
    }
    /// <summary>
    /// indexed from top to bottom
    /// </summary>
    public IReadOnlyCollection<Row> Rows { get; }
    /// <summary>
    /// indexed from left to right
    /// </summary>
    public IReadOnlyCollection<Column> Columns { get; }
    /// <summary>
    /// indexed from left to right and then top to bottom
    /// </summary>
    public IReadOnlyCollection<ThreeByThree> Regions { get; }
}