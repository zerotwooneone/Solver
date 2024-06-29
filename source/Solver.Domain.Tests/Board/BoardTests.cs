using Solver.Domain.Board;
using Solver.Domain.Cell;

namespace Solver.Domain.Tests.Board;

public class BoardTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void InvalidRow0Empty_NotSolvedNotInvalid()
    {
        var b = new GameBoard(
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(2), new SolvedCell(2), new SolvedCell(3), new SolvedCell(4),new SolvedCell(5),new SolvedCell(6),new SolvedCell(7),new SolvedCell(8),new SolvedCell(9) 
        );
        
        Assert.IsFalse(b.IsValid);
        Assert.IsFalse(b.IsSolved);
    }
    
    [Test]
    public void InvalidColumn0Empty_NotSolvedNotInvalid()
    {
        var b = new GameBoard(
            new SolvedCell(2), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(2), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(3), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(4), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(5), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(6), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(7), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(8), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(9), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance 
        );
        
        Assert.IsFalse(b.IsValid);
        Assert.IsFalse(b.IsSolved);
    }
    
    [Test]
    public void InvalidRegion0Empty_NotSolvedNotValid()
    {
        var b = new GameBoard(
            new SolvedCell(1), new SolvedCell(1), new SolvedCell(3), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(4), new SolvedCell(5), new SolvedCell(6), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(7), new SolvedCell(8), new SolvedCell(9), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance 
        );
        
        Assert.IsFalse(b.IsValid);
        Assert.IsFalse(b.IsSolved);
    }
    
    [Test]
    public void ValidRegion0Empty_NotSolvedIsValid()
    {
        var b = new GameBoard(
            new SolvedCell(1), new SolvedCell(2), new SolvedCell(3), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(4), new SolvedCell(5), new SolvedCell(6), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            new SolvedCell(7), new SolvedCell(8), new SolvedCell(9), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
            UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance 
        );
        
        Assert.IsTrue(b.IsValid);
        Assert.IsFalse(b.IsSolved);
    }
}