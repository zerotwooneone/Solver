using Solver.Domain.Board;

namespace Solver.Domain.Cell;

public class PartialCell(
    CellValue? initialValue, 
    IEnumerable<CellValue> remainingValues,
    IRow row,
    IColumn column,
    IRegion region) : ICell
{
    public CellValue? Value { get; } = initialValue;
    public IRow Row { get; } = row;
    public IColumn Column { get; } = column;
    public IRegion Region { get; } = region;
    public IReadOnlySet<CellValue> RemainingCellValues { get; } = new HashSet<CellValue>(remainingValues);

    public override string ToString()
    {
        return ((ICell)this).MonoSpacedString;
    }
    
    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if(!(obj is ICell cell)) return false;
        return Value.Equals(cell.Value);
    }
}