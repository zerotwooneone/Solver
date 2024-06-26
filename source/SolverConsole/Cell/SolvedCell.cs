namespace SolverConsole.Cell;

public class SolvedCell : ICell
{
    public SolvedCell(CellValue value)
    {
        Value = value;
    }

    public CellValue? Value { get; }
    public SolveState State => SolveState.Solved;
}