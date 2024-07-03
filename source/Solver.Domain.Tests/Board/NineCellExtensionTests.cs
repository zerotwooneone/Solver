using Solver.Domain.Board;
using Solver.Domain.Cell;

namespace Solver.Domain.Tests.Board;

public class NineCellExtensionTests
{
    [Test]
    public void GetHidden_PairExists_AndFound()
    {
        var row = new MutableNineCell(0);
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var expectedCell1 = new MutableCell( null, new CellValue[]{2,3,4,6,7,8}, row, column, region);
        var expectedCell2 = new MutableCell( null, new CellValue[]{2,3,4,6,8,9}, row, column, region);
        var cells = new []{
            new MutableCell( null, new CellValue[]{3,4,7,8,9}, row, column, region),
            new MutableCell( 1, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{3,4,7,8}, row, column, region),
            expectedCell1,
            new MutableCell( null, new CellValue[]{3,4,7,8}, row, column, region),
            new MutableCell( null, new CellValue[]{3,4,7,8}, row, column, region),
            expectedCell2,
            new MutableCell( 5, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{3,4,8}, row, column, region),
            };
        
        Assert.IsTrue(cells.Check().IsValid);
        Assert.IsFalse(cells.Check().IsSolved);

        Assert.IsTrue(cells.TryGetHidden(out var tuple));
        Assert.NotNull(tuple);
        Assert.AreEqual(2, tuple.Value.Cells.Count);
        Assert.AreEqual(2, tuple.Value.NewRemainingValues.Count);
        
        var expected = new CellValue[]{2,6};
        var expectedCells = new[] {expectedCell1, expectedCell2};

        CollectionAssert.AreEquivalent(expected, tuple.Value.NewRemainingValues);
        CollectionAssert.AreEquivalent(expectedCells, tuple.Value.Cells);
    }
    
    [Test]
    public void GetHidden_TripleExists_AndFound()
    {
        var row = new MutableNineCell(0);
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var expectedCell1 = new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, row, column, region);
        var expectedCell2 = new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, row, column, region);
        var expectedCell3 = new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, row, column, region);
        var cells = new []{
            new MutableCell( 1, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( 3, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{2}, row, column, region),
            expectedCell1,
            new MutableCell( 9, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{2,4,8}, row, column, region),
            expectedCell2,
            expectedCell3,
            new MutableCell( null, new CellValue[]{2,4,8}, row, column, region),
        };
        
        Assert.IsTrue(cells.Check().IsValid);
        Assert.IsFalse(cells.Check().IsSolved);

        Assert.IsTrue(cells.TryGetHidden(out var tuple));
        Assert.NotNull(tuple);
        Assert.AreEqual(3, tuple.Value.Cells.Count);
        Assert.AreEqual(3, tuple.Value.NewRemainingValues.Count);
        
        var expected = new CellValue[]{5,6,7};
        var expectedCells = new[] {expectedCell1, expectedCell2, expectedCell3};

        CollectionAssert.AreEquivalent(expected, tuple.Value.NewRemainingValues);
        CollectionAssert.AreEquivalent(expectedCells, tuple.Value.Cells);
    }
    
    [Test]
    public void GetHidden_SingleExists_AndFound()
    {
        var row = new MutableNineCell(0);
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var expectedCell1 = new MutableCell( null, new CellValue[]{1,7,8}, row, column, region);
        var cells = new []{
            new MutableCell( null, new CellValue[]{7,8,9}, row, column, region),
            new MutableCell( null, new CellValue[]{7,8,9}, row, column, region),
            expectedCell1,
            new MutableCell( null, new CellValue[]{3,4,7,8}, row, column, region),
            new MutableCell( 2, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( 6, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{3,4,9}, row, column, region),
            new MutableCell( null, new CellValue[]{3,4,9}, row, column, region),
            new MutableCell( 5, Array.Empty<CellValue>(), row, column, region),
        };
        
        Assert.IsTrue(cells.Check().IsValid);
        Assert.IsFalse(cells.Check().IsSolved);

        Assert.IsTrue(cells.TryGetHidden(out var tuple));
        Assert.NotNull(tuple);
        
        Assert.AreEqual(1, tuple.Value.Cells.Count);
        Assert.AreEqual(1, tuple.Value.NewRemainingValues.Count);
        
        var expected = new CellValue[]{1};
        var expectedCells = new[] {expectedCell1};

        CollectionAssert.AreEquivalent(expected, tuple.Value.NewRemainingValues);
        CollectionAssert.AreEquivalent(expectedCells, tuple.Value.Cells);
    }

    [Test]
    public void TryGetPointing_SingleExists_AndFound()
    {
        var row = new MutableNineCell(0);
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var regionCells = new[]
        {
            new MutableCell(null, new CellValue[] {2, 4, 5}, row, column, region),
            new MutableCell(null, new CellValue[] {2, 5, 7}, row, column, region),
            new MutableCell(null, new CellValue[] {4, 5, 7}, row, column, region)
        };
        var otherRegionCells = new[]
        {
            new MutableCell(null, new CellValue[] {5, 6}, row, column, region),
            new MutableCell(9, Array.Empty<CellValue>(), row, column, region),
            new MutableCell(8, Array.Empty<CellValue>(), row, column, region),
            new MutableCell(null, new CellValue[] {1, 2, 5, 6, 7}, row, column, region),
            new MutableCell(null, new CellValue[] {5, 6, 7}, row, column, region),
            new MutableCell(3, Array.Empty<CellValue>(), row, column, region),
        };
        var expectedCell = new MutableCell(null, new CellValue[] {2,3,4,5, 7,8,9}, row, column, region);
        var otherCells = new[]
        {
            new MutableCell(1, Array.Empty<CellValue>(), row, column, region),
            new MutableCell(null, new CellValue[] {5,  7,8,9}, row, column, region),
            new MutableCell(null, new CellValue[] {3,5,9}, row, column, region),
            new MutableCell(6, Array.Empty<CellValue>(), row, column, region),
            expectedCell,
            new MutableCell(null, new CellValue[] {3,5,7,9}, row, column, region),
        };
        var actual = NineCellExtensions.TryGetPointing(regionCells,otherRegionCells,otherCells, out var actualHidden);
        Assert.IsTrue(actual);

        var actualArray = actualHidden.ToArray();
        Assert.AreEqual(1, actualArray.Length);

        var firstActual = actualArray[0];
        Assert.AreEqual(1, firstActual.Cells.Count);
        Assert.AreEqual(expectedCell, firstActual.Cells.First());
        
        Assert.AreEqual(6,firstActual.NewRemainingValues.Count);
        CollectionAssert.AreEquivalent(new CellValue[]{2,3,5,7,8,9}, firstActual.NewRemainingValues);
    }
}