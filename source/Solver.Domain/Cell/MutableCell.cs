﻿namespace Solver.Domain.Cell;

public class MutableCell(int index, CellValue? value, IEnumerable<CellValue> remainingCellValues) : ICell
{
    public int Index { get; } = index;
    public CellValue? Value { get; set; } = value;

    SolveState ICell.State => RemainingCellValues.Count == 0
        ? SolveState.Solved
        : SolveState.CreatePartialState(new RemainingCellValues(RemainingCellValues));

    public HashSet<CellValue> RemainingCellValues { get;  } = [..remainingCellValues];
    public bool HasChanged { get; set; } = false;
        
    public int CompareTo(ICell? other)
    {
        if (other == null) return 1;
        if(other is not MutableCell mutableOther)
        {
            return Value != null && other.Value != null
                ? Value.Value - other.Value.Value
                : 1;
        }

        return Index - mutableOther.Index;
    }
    public override string ToString()
    {
        return Value.HasValue 
            ? (Value.ToString()?.ToLower() ?? string.Empty)
            : "+";
    }
}