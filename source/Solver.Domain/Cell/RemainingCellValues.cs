using System.Collections;

namespace Solver.Domain.Cell;

public struct RemainingCellValues : IEnumerable<CellValue>
{
    public static readonly RemainingCellValues Empty = new(Array.Empty<CellValue>());
    public static readonly RemainingCellValues All = new(CellValue.AllValues);
    public IReadOnlyList<CellValue> Values { get; } = CellValue.AllValues;

    public RemainingCellValues(IEnumerable<CellValue> values): this(values as CellValue[] ?? values.ToArray())
    {
    }
    public RemainingCellValues(params CellValue[] values)
    {
        if (values.Length > CellValue.AllValues.Count)
        {
            throw new ArgumentException("invalid number of values", nameof(values));
        }
        Array.Sort(values);
        var distinct = values.Distinct();
        if (distinct.Count() != values.Length)
        {
            throw new ArgumentException("cannot contain duplicate values", nameof(values));
        }

        if (!values.All(v => CellValue.AllValues.Contains(v)))
        {
            throw new ArgumentException("invalid value", nameof(values));
        }
        Values = values;
    }

    public IEnumerator<CellValue> GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public override string ToString()
    {
        return string.Join(",", Values);
    }
}