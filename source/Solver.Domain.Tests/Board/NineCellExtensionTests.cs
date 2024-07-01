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
        var cells = new []{
            new MutableCell( null, new CellValue[]{3,4,7,8,9}, row, column, region),
            new MutableCell( 1, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{3,4,7,8}, row, column, region),
            new MutableCell( null, new CellValue[]{2,3,4,6,7,8}, row, column, region),
            new MutableCell( null, new CellValue[]{3,4,7,8}, row, column, region),
            new MutableCell( null, new CellValue[]{3,4,7,8}, row, column, region),
            new MutableCell( null, new CellValue[]{2,3,4,6,8,9}, row, column, region),
            new MutableCell( 5, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{3,4,8}, row, column, region),
            };
        
        Assert.IsTrue(cells.Check().IsValid);
        Assert.IsFalse(cells.Check().IsSolved);

        Assert.IsTrue(cells.TryGetHidden(out var tuple));
        Assert.NotNull(tuple);
        
        var pair = tuple.Value.pair;
        var expected = new CellValue[]{2,6};
        var actual = new[]{pair.Value.value1, pair.Value.value2};

        CollectionAssert.AreEquivalent(expected, actual);
    }
    
    [Test]
    public void GetHidden_TripleExists_AndFound()
    {
        var row = new MutableNineCell(0);
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var cells = new []{
            new MutableCell( 1, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( 3, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{2}, row, column, region),
            new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, row, column, region),
            new MutableCell( 9, Array.Empty<CellValue>(), row, column, region),
            new MutableCell( null, new CellValue[]{2,4,8}, row, column, region),
            new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, row, column, region),
            new MutableCell( null, new CellValue[]{2,4,5,6,7,8}, row, column, region),
            new MutableCell( null, new CellValue[]{2,4,8}, row, column, region),
        };
        
        Assert.IsTrue(cells.Check().IsValid);
        Assert.IsFalse(cells.Check().IsSolved);

        Assert.IsTrue(cells.TryGetHidden(out var tuple));
        Assert.NotNull(tuple);
        
        var triple = tuple.Value.triple;
        var expected = new CellValue[]{5,6,7};
        var actual = new[]{triple.Value.value1, triple.Value.value2, triple.Value.value3};

        CollectionAssert.AreEquivalent(expected, actual);
    }
    
    [Test]
    public void GetHidden_SingleExists_AndFound()
    {
        var row = new MutableNineCell(0);
        var column = new MutableNineCell(0);
        var region = new MutableNineCell(0);
        var cells = new []{
            new MutableCell( null, new CellValue[]{7,8,9}, row, column, region),
            new MutableCell( null, new CellValue[]{7,8,9}, row, column, region),
            new MutableCell( null, new CellValue[]{1,7,8}, row, column, region),
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
        
        var single = tuple.Value.single;

        Assert.AreEqual(1, single.Value.value1.Value);
    }
}