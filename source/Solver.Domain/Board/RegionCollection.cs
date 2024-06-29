namespace Solver.Domain.Board;

public class RegionCollection : IRegionCollection
{
    public IRegion NW { get; }
    public IRegion N { get; }
    public IRegion NE { get; }
    public IRegion W { get; }
    public IRegion C { get; }
    public IRegion E { get; }
    public IRegion SW { get; }
    public IRegion S { get; }
    public IRegion SE { get; }

    public RegionCollection(IEnumerable<IRegion>  cells): this(cells as IRegion[]?? cells.ToArray())
    {
    }

    public RegionCollection(params IRegion[] cells)
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
}