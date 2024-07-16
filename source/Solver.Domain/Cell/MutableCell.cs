using Solver.Domain.Board;

namespace Solver.Domain.Cell;

public class MutableCell(CellValue? initialValue, IEnumerable<CellValue> remainingCellValues, MutableNineCell row, MutableNineCell column, MutableNineCell region) : ICell
{
    public CellValue? Value { get; set; } = initialValue;
    public MutableNineCell Row { get; } = row;
    IRow ICell.Row => Row;
    public MutableNineCell Column { get; } = column;
    IColumn ICell.Column => Column;
    public MutableNineCell Region { get; } = region;
    IRegion ICell.Region => Region;
    
    public static HashSet<CellValue> GetAllCellValues() => new(CellValue.AllValues);
    public static readonly HashSet<CellValue> EmptyRemainingValues = new(0);

    public HashSet<CellValue> RemainingCellValues { get;  } = [..remainingCellValues];
    IReadOnlySet<CellValue> ICell.RemainingCellValues => RemainingCellValues;
    
    public override string ToString()
    {
        return ((ICell)this).MonoSpacedString;
    }
    
    public bool TrySetCellAsSolved(CellValue value)
    {
        /*if (Value == null)
        {
            if (!RemainingCellValues.Contains(value))
            {
                int x = 0;
            }

            var intersect = Row.Remaining.Intersect(column.Remaining).Intersect(region.Remaining);
            if (!intersect.Contains(value))
            {
                int x = 0;
            }
            
            var union = Row.Solved.Union(column.Solved).Union(region.Solved);
            if (union.Contains(value))
            {
                int x = 0;
            }
        }*/
        
        if (Value.HasValue && Value != value)
        {
            throw new InvalidOperationException("Cell has already been solved");
        }
        var changed = RemainingCellValues.Count > 0;
        RemainingCellValues.Clear();

        changed = Value != value || changed;
        Value = value;
        
        changed = Row.SetSolved(value) || changed;
        changed = Column.SetSolved(value) || changed;
        changed = Region.SetSolved(value) || changed;
        
        return changed;
    }

    /// <summary>
    /// Reduce the remaining values to a subset of those provided. This can only remove remaining, this will not add
    /// </summary>
    public bool TryReduceRemaining(IEnumerable<CellValue> remaining)
    {
        return TryReduceRemaining(remaining as CellValue[] ?? remaining.ToArray());
    }

    /// <summary>
    /// Reduce the remaining values to a subset of those provided. This can only remove remaining, this will not add
    /// </summary>
    /// <param name="remaining"></param>
    /// <returns></returns>
    public bool TryReduceRemaining(params CellValue[] remaining)
    {
        // if (remaining.Length == CellValue.AllValues.Count)
        // {
        //     return false;
        // }

        var allSolved = row.Solved.Union(column.Solved).Union(region.Solved).ToHashSet();
        var hasChanged = RemainingCellValues.Aggregate(false,(hasChanged,value)=>
        {
            hasChanged = (!remaining.Contains(value) &&
                          TryRemoveRemaining(value)) || hasChanged;
            
            hasChanged = (allSolved.Contains(value) && TryRemoveRemaining(value)) || hasChanged;
            return hasChanged;
        });
        
        return hasChanged;
    }
    
    public bool TryReduceRemaining()
    {
        bool hasChanged=false;
        
        var remaining = Row.Remaining
            .Intersect(Column.Remaining)
            .Intersect(Region.Remaining)
            .ToArray();

        hasChanged = TryReduceRemaining(remaining) || hasChanged;

        return hasChanged;
    }

    public bool UpdateSolveState()
    {
        if (!Value.HasValue) return false;
        
        return TrySetCellAsSolved(Value.Value);
    }

    public bool TrySolveFromRemaining()
    {
        if (RemainingCellValues.Count != 1) return false;
        
        var value = RemainingCellValues.First();
        return TrySetCellAsSolved(value);
    }

    public bool TryRemoveRemaining(CellValue value)
    {
        return RemainingCellValues.Remove(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if(obj is not ICell cell) return false;

        var coordsMatch = Row.Index == cell.Row.Index && 
                Column.Index == cell.Column.Index
            /*&& Region.Index == cell.Region.Index*/;
        return Value.HasValue
            ? coordsMatch && Value == cell.Value
            : coordsMatch;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Row.Index, Column.Index, Value);
    }
}