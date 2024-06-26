namespace Solver.Domain.Cell;

public class SolvedCell(CellValue value) : ICell, IComparable
{
    public CellValue? Value { get; } = value;
    public SolveState State => SolveState.Solved;

    public override string ToString()
    {
        return Value.ToString() ?? "?";
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if(!(obj is ICell cell)) return false;
        return Value.Equals(cell.Value);
    }

    public int CompareTo(ICell? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Nullable.Compare(Value, other.Value);
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is SolvedCell other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(SolvedCell)}");
    }
}