﻿using System.Collections;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class NineCell : IRow, IColumn, IRegion
{
    public NineCell(IEnumerable<ICell>  cells): this(cells as ICell[]?? cells.ToArray())
    {
    }

    public NineCell(params ICell[] cells)
    {
        if (cells.Length != 9)
        {
            throw new ArgumentException("NineCell must have exactly 9 cells", nameof(cells));
        }
        
        A = cells[0];
        B = cells[1];
        C = cells[2];
        D = cells[3];
        E = cells[4];
        F = cells[5];
        G = cells[6];
        H = cells[7];
        I = cells[8];
    }

    public ICell A { get; } = UnsolvedCell.Instance;
    public ICell B { get; } = UnsolvedCell.Instance;
    public ICell C { get; } = UnsolvedCell.Instance;
    public ICell D { get; } = UnsolvedCell.Instance;
    public ICell E { get; } = UnsolvedCell.Instance;
    public ICell F { get; } = UnsolvedCell.Instance;
    public ICell G { get; } = UnsolvedCell.Instance;
    public ICell H { get; } = UnsolvedCell.Instance;
    public ICell I { get; } = UnsolvedCell.Instance;
    
    public int Count => 9;
    public override string ToString()
    {
        return $"{A},{B},{C} {D},{E},{F} {G},{H},{I}";
    }
    
    public IEnumerator<ICell> GetEnumerator()
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
    
    public ICell this[int i] => i switch
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
    
    ICell IRegion.this[int rowIndex, int columnIndex]
    {
        get
        {
            if (rowIndex < 0  || rowIndex > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }
            if (columnIndex < 0 || columnIndex > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(columnIndex));
            }

            var index = rowIndex * 3 + columnIndex;
            return this[index];
        }
    }
}