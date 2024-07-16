using Solver.Domain.Board;
using Solver.Domain.Cell;

namespace Solver.Domain.Tests.Board;

public class NineCellExtensionTests
{
    [Test]
    public void TryGetHidden_PairExists_AndFound()
    {
        var rows = Enumerable.Range(0, 9).Select(i => new MutableNineCell(i)).ToArray();
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var expectedCell1 = new MutableCell( null, new CellValue[]{2,3,4,6,7,8}, rows[3], column, region);
        var expectedCell2 = new MutableCell( null, new CellValue[]{2,3,4,6,8,9}, rows[6], column, region);
        var cells = new []{
            new MutableCell( null, new CellValue[]{3,4,7,8,9}, rows[0], column, region),
            new MutableCell( 1, Array.Empty<CellValue>(), rows[1], column, region),
            new MutableCell( null, new CellValue[]{3,4,7,8}, rows[2], column, region),
            expectedCell1,
            new MutableCell( null, new CellValue[]{3,4,7,8}, rows[4], column, region),
            new MutableCell( null, new CellValue[]{3,4,7,8}, rows[5], column, region),
            expectedCell2,
            new MutableCell( 5, Array.Empty<CellValue>(), rows[7], column, region),
            new MutableCell( null, new CellValue[]{3,4,8}, rows[8], column, region),
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
    public void TryGetHidden_TripleExists_AndFound()
    {
        var rows = Enumerable.Range(0, 9).Select(i => new MutableNineCell(i)).ToArray();
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var expectedCell1 = new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, rows[3], column, region);
        var expectedCell2 = new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, rows[6], column, region);
        var expectedCell3 = new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, rows[7], column, region);
        var cells = new []{
            new MutableCell( 1, Array.Empty<CellValue>(), rows[0], column, region),
            new MutableCell( 3, Array.Empty<CellValue>(), rows[1], column, region),
            new MutableCell( null, new CellValue[]{2}, rows[2], column, region),
            expectedCell1,
            new MutableCell( 9, Array.Empty<CellValue>(), rows[4], column, region),
            new MutableCell( null, new CellValue[]{2,4,8}, rows[5], column, region),
            expectedCell2,
            expectedCell3,
            new MutableCell( null, new CellValue[]{2,4,8}, rows[8], column, region),
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
    public void TryGetHidden_SingleExists_AndFound()
    {
        var rows = Enumerable.Range(0, 9).Select(i => new MutableNineCell(i)).ToArray();
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var expectedCell1 = new MutableCell( null, new CellValue[]{1,7,8}, rows[2], column, region);
        var cells = new []{
            new MutableCell( null, new CellValue[]{7,8,9}, rows[0], column, region),
            new MutableCell( null, new CellValue[]{7,8,9}, rows[1], column, region),
            expectedCell1,
            new MutableCell( null, new CellValue[]{3,4,7,8}, rows[3], column, region),
            new MutableCell( 2, Array.Empty<CellValue>(), rows[4], column, region),
            new MutableCell( 6, Array.Empty<CellValue>(), rows[5], column, region),
            new MutableCell( null, new CellValue[]{3,4,9}, rows[6], column, region),
            new MutableCell( null, new CellValue[]{3,4,9}, rows[7], column, region),
            new MutableCell( 5, Array.Empty<CellValue>(), rows[8], column, region),
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
    
    [Test]
    public void TryGetNaked_SingleExists_AndFound()
    {
        var row = new MutableNineCell(0);
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var expectedCell1 = new MutableCell(null, new CellValue[] {1,2,4, 7}, row, column, region);
        var expectedCell2 = new MutableCell(null, new CellValue[] {1,2,4, 7}, row, column, region);
        var cells = new[]
        {
            new MutableCell(3, Array.Empty<CellValue>(), row, column, region),
            new MutableCell(5, Array.Empty<CellValue>(), row, column, region),
            new MutableCell(null, new CellValue[] {1, 7}, row, column, region),
            new MutableCell(null, new CellValue[] {1,7}, row, column, region),
            expectedCell1,
            expectedCell2,
            new MutableCell(8, Array.Empty<CellValue>(), row, column, region),
            new MutableCell(6, Array.Empty<CellValue>(), row, column, region),
            new MutableCell(9, Array.Empty<CellValue>(), row, column, region),
        };
        var actual = cells.TryGetNaked(out var hidden);
        Assert.IsTrue(actual);
        
        var expectedRemaining = new CellValue[] { 2, 4};

        Assert.IsNotNull(hidden);
        var actualArray = hidden.ToArray();
        var actualCell1 = actualArray.FirstOrDefault(t=>t.Cells.Count == 1 && t.Cells.First().Equals(expectedCell1));
        Assert.IsNotNull(actualCell1);
        CollectionAssert.AreEquivalent(expectedRemaining, actualCell1.NewRemainingValues);
        
        var actualCell2 = actualArray.FirstOrDefault(t=>t.Cells.Count == 1 && t.Cells.First().Equals(expectedCell2));
        Assert.IsNotNull(actualCell2);
        CollectionAssert.AreEquivalent(expectedRemaining, actualCell2.NewRemainingValues);
        
    }
}