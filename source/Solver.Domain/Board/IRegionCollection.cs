using System.Collections;

namespace Solver.Domain.Board;

public interface IRegionCollection : IReadOnlyList<IRegion>
{
    IRegion NW { get; }
    IRegion N { get; }
    IRegion NE { get; }
    IRegion W { get; }
    IRegion C { get; }
    IRegion E { get; }
    IRegion SW { get; }
    IRegion S { get; }
    IRegion SE { get; }
    int IReadOnlyCollection<IRegion>.Count => 9;
    
    IEnumerator<IRegion> IEnumerable<IRegion>.GetEnumerator()
    {
        yield return NW;
        yield return N;
        yield return NE;
        yield return W;
        yield return C;
        yield return E;
        yield return SW;
        yield return S;
        yield return SE;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    IRegion IReadOnlyList<IRegion>.this[int index] => index switch
    {
        0 => NW,
        1 => N,
        2 => NE,
        3 => W,
        4 => C,
        5 => E,
        6 => SW,
        7 => S,
        8 => SE,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    /// <summary>
    /// Index starts at the upper left in row major order
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columnIndex"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    IRegion this[int rowIndex, int columnIndex]
    {
        get
        {
            if (rowIndex < 0  || rowIndex > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }
            if (columnIndex < 0 || columnIndex > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            var index = rowIndex * 3 + columnIndex;
            return this[index];
        }
    }
}