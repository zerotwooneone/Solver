using Solver.Domain.SystemCollections;

namespace Solver.Domain.Tests.SystemCollections;

public class EnumerableTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void PermutateBy2()
    {
        var input = new[] {1, 2, 3, 4, 5};

        var actual = input
            .Permutate(2)
            .ToArray();

        var expected = new int[][]
        {
            [1, 2], [1, 3], [1, 4], [1, 5],
            [2, 3], [2, 4], [2, 5],
            [3, 4], [3, 5],
            [4, 5]
        };

        CollectionAssert.AreEquivalent(expected, actual);
    }

    [Test]
    public void PermutateBy3()
    {
        var input = new[] {1, 2, 3, 4, 5};

        var actual = input
            .Permutate(3)
            .ToArray();

        var expected = new int[][]
        {
            [1, 2, 3], [1, 2, 4], [1, 2, 5], [1, 3, 4], [1, 3, 5],
            [1, 4, 5],
            [2, 3, 4], [2, 3, 5],
            [2, 4, 5],
            [3, 4, 5]
        };

        CollectionAssert.AreEquivalent(expected, actual);
    }

    [Test]
    public void PermutateBy4()
    {
        var input = new[] {1, 2, 3, 4, 5};

        var actual = input
            .Permutate(4)
            .ToArray();

        var expected = new int[][]
        {
            [1, 2, 3, 4], [1, 3, 4, 5],
            [1, 2, 4, 5], [1, 2, 3, 5],
            [2, 3, 4, 5]
        };

        CollectionAssert.AreEquivalent(expected, actual);
    }
}