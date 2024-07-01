using System.Diagnostics;
using Solver.Domain.Board;

namespace Solver.Domain.Cell;

public class MutableCell(CellValue? value, IEnumerable<CellValue> remainingCellValues, MutableNineCell row, MutableNineCell column, MutableNineCell region) : ICell
{
    public CellValue? Value { get; set; } = value;
    public MutableNineCell Row { get; } = row;
    public MutableNineCell Column { get; } = column;
    public MutableNineCell Region { get; } = region;
    public int RowIndex => Row.Index;
    public int ColumnIndex => Column.Index;
    public int RegionIndex => Region.Index;

    SolveState ICell.State => RemainingCellValues.Count == 0
        ? SolveState.Solved
        : SolveState.CreatePartialState(new RemainingCellValues(RemainingCellValues));

    public HashSet<CellValue> RemainingCellValues { get;  } = [..remainingCellValues];
    public bool HasChanged { get; set; } = false;
    public override string ToString()
    {
        return Value.HasValue 
            ? (Value.ToString()?.ToLower() ?? string.Empty)
            : "+";
    }
    
    public bool SetCellAsSolved(CellValue value)
    {
        if (Value.HasValue && Value != value)
        {
            throw new InvalidOperationException("Cell has already been solved");
        }
        var changed = RemainingCellValues.Count > 0;
        RemainingCellValues.Clear();
        changed = Value != value || changed;
        if (changed)
        {
            Debug.WriteLine($"Cell changed from {Value} to {value}");
        }
        Value = value;
        
        //debug: changed && value == 9
        changed = Row.SetSolved(value) || changed;
        changed = Column.SetSolved(value) || changed;
        changed = Region.SetSolved(value) || changed;
        
        return changed;
    }

    public bool TryReduceRemaining(params CellValue[] remaining)
    {
        if (remaining.Length == CellValue.AllValues.Count)
        {
            return false;
        }

        return RemainingCellValues.Aggregate(false,(hc,currentCell)=> (!remaining.Contains(currentCell) &&
                                                                       RemainingCellValues.Remove(currentCell)) || hc);
    }
    
    public bool TryReduce()
    {
        bool hasChanged=false;
        
        if (Value.HasValue)
        {
            if (SetCellAsSolved(Value.Value))
            {
                hasChanged = true;
            }
        }

        if (RemainingCellValues.Count == 1)
        {
            var value = RemainingCellValues.First();
            if (SetCellAsSolved(value))
            {
                hasChanged = true;
            }
        }

        var remaining = Row.Remaining
            .Intersect(Column.Remaining)
            .Intersect(Region.Remaining)
            .ToArray();

        if (!Value.HasValue && 
            remaining.Length == 1 && 
            SetCellAsSolved(remaining.First()))
        {
            hasChanged = true;
        }

        if (TryReduceRemaining(remaining) && RemainingCellValues.Count == 1)
        {
            var value = RemainingCellValues.First();
            SetCellAsSolved(value);
            hasChanged = true;
        }

        return hasChanged;
    }
}