namespace SolverConsole.Cell;

public abstract class UnsolvedCell 
{
    public static readonly ICell Instance = new UnsolvedCell_Internal();
    private class UnsolvedCell_Internal : ICell
    {
        public CellValue? Value => null;
        public SolveState State => SolveState.Unsolved;
        public override string ToString()
        {
            return ICell.Blank;
        }
    }
}