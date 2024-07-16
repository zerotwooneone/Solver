using Solver.Domain.Board;

namespace Solver.Domain.Cell;

public interface ICell 
{
    CellValue? Value { get; }
    
    IReadOnlySet<CellValue> RemainingCellValues { get; }
    
    IRow Row { get; }
    IColumn Column { get; }
    IRegion Region { get; }
    
    public const string MonoSpacedBlank = "_";
    public const string MonoSpacedUnsolved = "+";

    string MonoSpacedString=>Value.HasValue
            ? (Value.ToString() ?? string.Empty)
            : RemainingCellValues.Any()
            ? MonoSpacedUnsolved
            : MonoSpacedBlank;
}