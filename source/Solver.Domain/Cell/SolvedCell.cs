namespace Solver.Domain.Cell;

public class SolvedCell(CellValue value) : ICell
{
    public CellValue? Value { get; } = value;
    public SolveState State => SolveState.Solved;

    public override string ToString()
    {
        return Value.ToString() ?? "?";
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