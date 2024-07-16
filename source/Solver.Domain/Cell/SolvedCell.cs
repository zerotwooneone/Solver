using Solver.Domain.Board;

namespace Solver.Domain.Cell;

public class SolvedCell(
    CellValue initialValue,
    IRow row,
    IColumn column,
    IRegion region) : ICell
{
    public CellValue? Value { get; } = initialValue;
    public IRow Row { get; } = row;
    public IColumn Column { get; } = column;
    public IRegion Region { get; } = region;
    public static readonly HashSet<CellValue> Empty = new(0);
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