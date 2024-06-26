namespace SolverConsole.Cell;

public class SolvedCell(CellValue value) : ICell
{
    public CellValue? Value { get; } = value;
    public SolveState State => SolveState.Solved;

    public override string ToString()
    {
        return Value.ToString() ?? "?";
    }
}