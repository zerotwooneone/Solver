namespace SolverConsole.Cell;

public readonly struct CellValue: IComparable<CellValue>, IComparable
{
    public static readonly IReadOnlyCollection<CellValue> AllValues = new CellValue[]{1,2,3,4,5,6,7,8,9};
    public CellValue(int value)
    {
        if (value < 1 || value > 9)
        {
            throw new ArgumentException("Value must be between 1 and 9", nameof(value));
        }
        Value = value;
    }

    public int Value { get; } = 1;
    public static implicit operator int(CellValue value) => value.Value;
    public static implicit operator CellValue(int value) => new(value);

    public int CompareTo(CellValue other)
    {
        return Value.CompareTo(other.Value);
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is CellValue other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(CellValue)}");
    }
    
    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if(!(obj is CellValue cv)) return false;
        return Value.Equals(cv.Value);
    }
}