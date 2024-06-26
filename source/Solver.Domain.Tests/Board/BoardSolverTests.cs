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
        
        Assert.IsTrue(solution.IsValid);
        Assert.IsTrue(solution.IsSolved);
    }
}