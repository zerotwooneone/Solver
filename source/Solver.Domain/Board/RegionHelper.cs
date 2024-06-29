namespace Solver.Domain.Board;

public static class RegionHelper
{
    public static int GetRegionIndex(int rowIndex, int columnIndex)
    {
        return ((rowIndex / 3) * 3) + (columnIndex / 3);
    }

    public static (int row, int column) GetIndexWithinRegion(int rowIndex, int columnIndex)
    {
        return ( row:rowIndex % 3, column: columnIndex % 3);
    }
}