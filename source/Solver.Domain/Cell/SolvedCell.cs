namespace Solver.Domain.Cell;

public class SolvedCell(CellValue value) : ICell
{
    public CellValue? Value { get; } = value;
    private static readonly HashSet<CellValue> Empty = new();
    public IReadOnlySet<CellValue> RemainingCellValues => Empty;

    public override string ToString()
    {
        return value.ToString();
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