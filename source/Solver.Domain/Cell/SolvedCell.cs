namespace Solver.Domain.Cell;

public class SolvedCell(CellValue initialValue) : ICell
{
    public CellValue? Value { get; } = initialValue;
    private static readonly HashSet<CellValue> Empty = new();
    public IReadOnlySet<CellValue> RemainingCellValues => Empty;

    public override string ToString()
    {
        return initialValue.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if(!(obj is ICell cell)) return false;
        return Value.Equals(cell.Value);
    }
}