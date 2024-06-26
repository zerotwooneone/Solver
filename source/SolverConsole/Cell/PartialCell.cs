namespace SolverConsole.Cell;

public class PartialCell : ICell
{
    public PartialCell(CellValue value, RemainingCellValues remainingValues)
    {
        Value = value;
        State = SolveState.CreatePartialState(remainingValues);
    }

    public CellValue? Value { get; }
    public SolveState State { get; }
}