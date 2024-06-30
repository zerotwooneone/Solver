namespace Solver.Domain.Cell;

public interface ICell 
{
    public CellValue? Value { get; }
    public const string Blank = "_";
    public SolveState State { get; }
}