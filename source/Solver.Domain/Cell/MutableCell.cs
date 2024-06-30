using Solver.Domain.Board;

namespace Solver.Domain.Cell;

public class MutableCell(CellValue? value, IEnumerable<CellValue> remainingCellValues, MutableNineCell row, MutableNineCell column, MutableNineCell region) : ICell
{
    public CellValue? Value { get; set; } = value;
    public MutableNineCell Row { get; } = row;
    public MutableNineCell Column { get; } = column;
    public MutableNineCell Region { get; } = region;

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
        var changed = RemainingCellValues.Count > 0;
        RemainingCellValues.Clear();
        changed = Value != value || changed;
        Value = value;
        
        //debug: changed && value == 9
        changed = Row.SetSolved(value) || changed;
        changed = Column.SetSolved(value) || changed;
        changed = Region.SetSolved(value) || changed;
        
        return changed;
    }

    public bool TryReduceRemaining(params CellValue[] values)
    {
        if (values.Length == CellValue.AllValues.Count)
        {
            return false;
        }

        bool changed = !RemainingCellValues.SetEquals(values);
        if (changed)
        {
            RemainingCellValues.Clear();
            foreach (var cellValue in values)
            {
                RemainingCellValues.Add(cellValue);
            }
        }

        return changed;
    }
}