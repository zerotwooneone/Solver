namespace Solver.Domain.Cell;

public class PartialCell(CellValue? value, IEnumerable<CellValue> remainingValues) : ICell
{
    public CellValue? Value { get; } = value;
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