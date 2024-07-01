namespace Solver.Domain.Cell;

public interface ICell 
{
    public CellValue? Value { get; }
    
    public IReadOnlySet<CellValue> RemainingCellValues { get; }
    
    public const string MonoSpacedBlank = "_";
    public const string MonoSpacedUnsolved = "+";

    public string MonoSpacedString=>Value.HasValue
            ? (Value.ToString() ?? string.Empty)
            : RemainingCellValues.Any()
            ? MonoSpacedUnsolved
            : MonoSpacedBlank;
}