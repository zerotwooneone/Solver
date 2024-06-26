using Solver.Domain.Board;
using Solver.Domain.Cell;

namespace Solver.Domain.Tests.Board;

public class BoardBuilderTests
{
    [Test]
    public void File1IsSolved()
    {
        var builder = new BoardBuilder();
        
        var b = builder.CreateFrom9x9Csv(@"Valid\Solution.txt");;
        
        Assert.IsTrue(b.IsValid);
        Assert.IsTrue(b.IsSolved);
    }
}