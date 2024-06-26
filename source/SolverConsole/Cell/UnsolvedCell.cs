namespace SolverConsole.Cell;

public abstract class UnsolvedCell 
{
    public static readonly ICell Instance = new UnsolvedCell_Internal();
    private class UnsolvedCell_Internal : ICell
    {
        public CellValue? Value { get; } = null;
        public SolveState State => SolveState.Unsolved;
    }
}