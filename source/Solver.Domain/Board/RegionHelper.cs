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
    
    public static (int rowIndex, int columnIndex) GetRegionCoordinates(int rowIndex, int columnIndex)
    {
        return (rowIndex: rowIndex / 3, columnIndex: columnIndex / 3);
    }

    public static (int rowIndex, int columnIndex) GetRegionCoordinatesFromRowMajorOrder(int index)
    {
        return (rowIndex: index / 3, columnIndex: index % 3);
    }
}