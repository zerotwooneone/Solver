namespace SolverConsole.Cell;

public abstract class UnsolvedCell 
{
    public static readonly ICell Instance = new UnsolvedCell_Internal();
    private class UnsolvedCell_Internal : ICell, IComparable
    {
        public CellValue? Value => null;
        public SolveState State => SolveState.Unsolved;
        public override string ToString()
        {
            return ICell.Blank;
        }

        public int CompareTo(ICell? other)
        {
            return 1;
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is UnsolvedCell_Internal other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(UnsolvedCell_Internal)}");
        }
    }
}