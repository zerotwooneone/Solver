using Solver.Domain.Board;
using Solver.Domain.Cell;

namespace Solver.Domain.Tests.Board;

public class BoardTests
{
    private static readonly ICell UnsolvedCellInstance = new UnsolvedCell(null, null, null);

    private static readonly NineCell DummyNineCell = new NineCell(-1,UnsolvedCellInstance, UnsolvedCellInstance,
        UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
        UnsolvedCellInstance, UnsolvedCellInstance);
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void InvalidRow0Empty_NotSolvedNotInvalid()
    {
        IRow dummyRow = DummyNineCell;
        IColumn dummyColumn = DummyNineCell;
        IRegion dummyRegion = DummyNineCell;
        var builder = new BoardBuilder();
        var groups = builder.GetGroups(UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(2, dummyRow, dummyColumn, dummyRegion), new SolvedCell(2, dummyRow, dummyColumn, dummyRegion), new SolvedCell(3, dummyRow, dummyColumn, dummyRegion), new SolvedCell(4, dummyRow, dummyColumn, dummyRegion),new SolvedCell(5, dummyRow, dummyColumn, dummyRegion),new SolvedCell(6, dummyRow, dummyColumn, dummyRegion),new SolvedCell(7, dummyRow, dummyColumn, dummyRegion),new SolvedCell(8, dummyRow, dummyColumn, dummyRegion),new SolvedCell(9, dummyRow, dummyColumn, dummyRegion));
        var b = new GameBoard(groups.rows, groups.columns, groups.regions);
        
        Assert.IsFalse(b.GetIsValid());
        Assert.IsFalse(b.GetIsSolved());
    }
    
    [Test]
    public void InvalidColumn0Empty_NotSolvedNotInvalid()
    {
        IRow dummyRow = DummyNineCell;
        IColumn dummyColumn = DummyNineCell;
        IRegion dummyRegion = DummyNineCell;
        var builder = new BoardBuilder();
        var groups = builder.GetGroups(new SolvedCell(2, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(2, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(3, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(4, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(5, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(6, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(7, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(8, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(9, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance);
        var b = new GameBoard(groups.rows, groups.columns, groups.regions);
        
        Assert.IsFalse(b.GetIsValid());
        Assert.IsFalse(b.GetIsSolved());
    }
    
    [Test]
    public void InvalidRegion0Empty_NotSolvedNotValid()
    {
        IRow dummyRow = DummyNineCell;
        IColumn dummyColumn = DummyNineCell;
        IRegion dummyRegion = DummyNineCell;
        var builder = new BoardBuilder();
        var groups = builder.GetGroups(new SolvedCell(1, dummyRow, dummyColumn, dummyRegion), new SolvedCell(1, dummyRow, dummyColumn, dummyRegion), new SolvedCell(3, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(4, dummyRow, dummyColumn, dummyRegion), new SolvedCell(5, dummyRow, dummyColumn, dummyRegion), new SolvedCell(6, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(7, dummyRow, dummyColumn, dummyRegion), new SolvedCell(8, dummyRow, dummyColumn, dummyRegion), new SolvedCell(9, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance);
        var b = new GameBoard(groups.rows, groups.columns, groups.regions);
        
        Assert.IsFalse(b.GetIsValid());
        Assert.IsFalse(b.GetIsSolved());
    }
    
    [Test]
    public void ValidRegion0Empty_NotSolvedIsValid()
    {
        IRow dummyRow = DummyNineCell;
        IColumn dummyColumn = DummyNineCell;
        IRegion dummyRegion = DummyNineCell;
        var builder = new BoardBuilder();
        var groups = builder.GetGroups(new SolvedCell(1, dummyRow, dummyColumn, dummyRegion), new SolvedCell(2, dummyRow, dummyColumn, dummyRegion), new SolvedCell(3, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(4, dummyRow, dummyColumn, dummyRegion), new SolvedCell(5, dummyRow, dummyColumn, dummyRegion), new SolvedCell(6, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            new SolvedCell(7, dummyRow, dummyColumn, dummyRegion), new SolvedCell(8, dummyRow, dummyColumn, dummyRegion), new SolvedCell(9, dummyRow, dummyColumn, dummyRegion), UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance,
            UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance, UnsolvedCellInstance);
        var b = new GameBoard(groups.rows, groups.columns, groups.regions);
        
        Assert.IsTrue(b.GetIsValid());
        Assert.IsFalse(b.GetIsSolved());
    }
}