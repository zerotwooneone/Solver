namespace Solver.Domain.Cell;

public class PartialCell(CellValue? value, RemainingCellValues remainingValues) : ICell
{
    public CellValue? Value { get; } = value;
    public SolveState State { get; } = SolveState.CreatePartialState(remainingValues);

    public override string ToString()
    {
        return Value.HasValue 
            ? (Value.ToString()?.ToLower() ?? string.Empty)
            : "+";
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