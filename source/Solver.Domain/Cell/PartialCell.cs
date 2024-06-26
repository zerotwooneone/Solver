namespace Solver.Domain.Cell;

public class PartialCell(CellValue? value, RemainingCellValues remainingValues) : ICell, IComparable
{
    public CellValue? Value { get; } = value;
    public SolveState State { get; } = SolveState.CreatePartialState(remainingValues);

    public override string ToString()
    {
        return Value.HasValue 
            ? (Value.ToString()?.ToLower() ?? string.Empty)
            : "+";
    }
    
    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? base.GetHashCode();
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
        return obj is PartialCell other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(PartialCell)}");
    }
}