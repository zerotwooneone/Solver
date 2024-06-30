namespace Solver.Domain.Cell;

public class MutableCell(CellValue? value, IEnumerable<CellValue> remainingCellValues) : ICell
{
    public CellValue? Value { get; set; } = value;

    SolveState ICell.State => RemainingCellValues.Count == 0
        ? SolveState.Solved
        : SolveState.CreatePartialState(new RemainingCellValues(RemainingCellValues));

    public HashSet<CellValue> RemainingCellValues { get;  } = [..remainingCellValues];
    public bool HasChanged { get; set; } = false;
    public override string ToString()
    {
        return Value.HasValue 
            ? (Value.ToString()?.ToLower() ?? string.Empty)
            : "+";
    }
}