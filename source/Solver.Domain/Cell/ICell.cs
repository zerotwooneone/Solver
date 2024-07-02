namespace Solver.Domain.Cell;

public interface ICell 
{
    CellValue? Value { get; }
    
    IReadOnlySet<CellValue> RemainingCellValues { get; }
    
    
    public const string MonoSpacedBlank = "_";
    public const string MonoSpacedUnsolved = "+";

    string MonoSpacedString=>Value.HasValue
            ? (Value.ToString() ?? string.Empty)
            : RemainingCellValues.Any()
            ? MonoSpacedUnsolved
            : MonoSpacedBlank;
}