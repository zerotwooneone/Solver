namespace Solver.Domain.Board;

internal class MutableRegionCollection : IRegionCollection
{
    public MutableNineCell NW { get; }
    public MutableNineCell N { get; }
    public MutableNineCell NE { get; }
    public MutableNineCell W { get; }
    public MutableNineCell C { get; }
    public MutableNineCell E { get; }
    public MutableNineCell SW { get; }
    public MutableNineCell S { get; }
    public MutableNineCell SE { get; }
    
    IRegion IRegionCollection.NW => NW;
    IRegion IRegionCollection.N => N;
    IRegion IRegionCollection.NE => NE;
    IRegion IRegionCollection.W => W;
    IRegion IRegionCollection.C => C;
    IRegion IRegionCollection.E => E;
    IRegion IRegionCollection.SW => SW;
    IRegion IRegionCollection.S => S;
    IRegion IRegionCollection.SE => SE;

    public MutableRegionCollection(IEnumerable<MutableNineCell>  cells): this(cells as MutableNineCell[]?? cells.ToArray())
    {
    }

    public MutableRegionCollection(params MutableNineCell[] cells)
    {
        if (cells.Length != 9)
        {
            throw new ArgumentException("NineCell must have exactly 9 cells", nameof(cells));
        }

        NW = cells[0];
        N = cells[1];
        NE = cells[2];
        W = cells[3];
        C = cells[4];
        E = cells[5];
        SW = cells[6];
        S = cells[7];
        SE = cells[8];
    }
    
    /// <summary>
    /// Index starts at the upper left in row major order
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columnIndex"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public MutableNineCell this[int rowIndex, int columnIndex]
    {
        get
        {
            if (rowIndex < 0  || rowIndex > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }
            if (columnIndex < 0 || columnIndex > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            var cell = (rowIndex, columnIndex) switch
            {
                (0, 0) => NW,
                (0, 1) => N,
                (0, 2) => NE,
                (1, 0) => W,
                (1, 1) => C,
                (1, 2) => E,
                (2, 0) => SW,
                (2, 1) => S,
                (2, 2) => SE,
                _ => throw new ArgumentOutOfRangeException(nameof(rowIndex))
            };
            return cell;
        }
    }
}