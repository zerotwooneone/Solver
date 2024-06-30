using Solver.Domain.Board;
using Solver.Domain.Cell;

namespace Solver.Domain.Tests.Board;

public class NineCellExtensionTests
{
    [Test]
    public void GetHiddenPair_PairExists_AndFound()
    {
        var cells = new []{
            new MutableCell( null, new CellValue[]{3,4,7,8,9}),
            new MutableCell( 1, Array.Empty<CellValue>()),
            new MutableCell( null, new CellValue[]{3,4,7,8}),
            new MutableCell( null, new CellValue[]{2,3,4,6,7,8}),
            new MutableCell( null, new CellValue[]{3,4,7,8}),
            new MutableCell( null, new CellValue[]{3,4,7,8}),
            new MutableCell( null, new CellValue[]{2,3,4,6,8,9}),
            new MutableCell( 5, Array.Empty<CellValue>()),
            new MutableCell( null, new CellValue[]{3,4,8}),
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
}