using Solver.Domain.Board;

namespace Solver.Domain.Cell;

public class UnsolvedCell(
    IRow row,
    IColumn column,
    IRegion region)  : ICell
{
    public IRow Row { get; } = row;
    public IColumn Column { get; } = column;
    public IRegion Region { get; } = region;
    public CellValue? Value => null;
    private static readonly HashSet<CellValue> All = new(CellValue.AllValues);
    public IReadOnlySet<CellValue> RemainingCellValues => All;
    public override string ToString()
    {
        return ICell.MonoSpacedBlank;
    }
    
}