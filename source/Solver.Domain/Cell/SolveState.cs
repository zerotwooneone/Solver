namespace Solver.Domain.Cell;

public abstract class SolveState
{
    public static readonly SolveState Unsolved = new UnsolvedState();
    public static readonly SolveState Solved = new SolvedState();
    
    public abstract bool IsSolved { get; }
    public abstract RemainingCellValues RemainingValues { get; }
    
    private class UnsolvedState : SolveState
    {
        public override bool IsSolved { get; } = false;
        public override RemainingCellValues RemainingValues { get; } = RemainingCellValues.All;
    }

    private class PartialState : SolveState
    {
        public override bool IsSolved { get; } = false;
        public override RemainingCellValues RemainingValues { get; }

        public PartialState(RemainingCellValues remainingValues)
        {
            RemainingValues = remainingValues;
        }
    }

    private class SolvedState : SolveState
    {
        public override bool IsSolved { get; } = true;
        public override RemainingCellValues RemainingValues { get; } = RemainingCellValues.Empty;
    }

    public static SolveState CreatePartialState(RemainingCellValues remainingValues)
    {
        return new PartialState(remainingValues);
    }
}