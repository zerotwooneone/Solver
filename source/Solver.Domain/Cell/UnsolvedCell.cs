namespace Solver.Domain.Cell;

public abstract class UnsolvedCell 
{
    public static readonly ICell Instance = new UnsolvedCell_Internal();
    private class UnsolvedCell_Internal : ICell
    {
        public CellValue? Value => null;
        private static readonly HashSet<CellValue> All = new(CellValue.AllValues);
        public IReadOnlySet<CellValue> RemainingCellValues => All;
        public override string ToString()
        {
            return ICell.MonoSpacedBlank;
        }
    }
}