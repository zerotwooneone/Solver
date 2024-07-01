using System.Collections;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public interface IRow : IReadOnlyList<ICell>
{
    ICell A { get; }
    ICell B { get; }
    ICell C { get; }
    ICell D { get; }
    ICell E { get; }
    ICell F { get; }
    ICell G { get; }
    ICell H { get; }
    ICell I { get; }
    int IReadOnlyCollection<ICell>.Count => 9;
    string MonoSpacedString => $"{A} {B} {C}{Environment.NewLine}{D} {E} {F}{Environment.NewLine}{G} {H} {I}";
    
    IEnumerator<ICell> IEnumerable<ICell>.GetEnumerator()
    {
        yield return A;
        yield return B;
        yield return C;
        yield return D;
        yield return E;
        yield return F;
        yield return G;
        yield return H;
        yield return I;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    ICell IReadOnlyList<ICell>.this[int i] => i switch
    {
        0 => A,
        1 => B,
        2 => C,
        3 => D,
        4 => E,
        5 => F,
        6 => G,
        7 => H,
        8 => I,
        _ => throw new ArgumentOutOfRangeException(nameof(i))
    };
}