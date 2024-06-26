namespace SolverConsole.Cell;

public interface ICell : IComparable<ICell>
{
    public CellValue? Value { get; }
    public const string Blank = "_";
    public SolveState State { get; }
}