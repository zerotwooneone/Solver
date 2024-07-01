using Solver.Domain.Board;

namespace Solver.Domain.Tests.Board;

public class BoardSolverTests
{
    [Test]
    public void File1IsSolved()
    {
        var builder = new BoardBuilder();
        var b = builder.CreateFrom9x9Csv(@"Unsolved\input.txt");;
        var solver = new BoardSolver();
        var solution = solver.GetSolvedBoard(b);
        
        Assert.IsTrue(solution.GetIsValid());
        Assert.IsTrue(solution.GetIsSolved());
        
        var expected = builder.CreateFrom9x9Csv(@"Valid\Solution.txt");
        
        CollectionAssert.AreEqual(expected.Rows, solution.Rows);
        CollectionAssert.AreEqual(expected.Columns, solution.Columns);
        CollectionAssert.AreEqual(expected.Regions, solution.Regions);
    }
}