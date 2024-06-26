namespace SolverConsole.Cell;

public class PartialCell(CellValue? value, RemainingCellValues remainingValues) : ICell
{
    public CellValue? Value { get; } = value;
    public SolveState State { get; } = SolveState.CreatePartialState(remainingValues);

    public override string ToString()
    {
        return Value.HasValue 
            ? (Value.ToString()?.ToLower() ?? string.Empty)
            : State.RemainingValues.Values.Count> 9
            ? "+"
            : State.RemainingValues.Values.Count.ToString();
    }
}