using System.Collections;
using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public class MutableNineCell : IRow, IColumn, IRegion, IReadOnlyList<MutableCell>
{
    public int Index { get; }

    public MutableNineCell(int index)
    {
        Index = index;
        Solved = new HashSet<CellValue>(9);
        Remaining = new HashSet<CellValue>(CellValue.AllValues);
    }
    /*public MutableNineCell(IEnumerable<MutableCell>  cells): this(cells as MutableCell[]?? cells.ToArray())
    {
    }

    public MutableNineCell(params MutableCell[] cells)
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
        
        var (remaining, solved) = this.GetStats();
        Remaining = remaining;
        Solved = solved;
    }*/

    public HashSet<CellValue> Solved { get; }

    public HashSet<CellValue> Remaining { get; }

    public MutableCell A { get; set; }
    public MutableCell B { get; set; }
    public MutableCell C { get; set; }
    public MutableCell D { get; set; }
    public MutableCell E { get; set; }
    public MutableCell F { get; set; }
    public MutableCell G { get; set; }
    public MutableCell H { get; set; }
    public MutableCell I { get; set; }
    
    ICell IRow.A => A;
    ICell IRow.B => B;
    ICell IRow.C => C;
    ICell IRow.D => D;
    ICell IRow.E => E;
    ICell IRow.F => F;
    ICell IRow.G => G;
    ICell IRow.H => H;
    ICell IRow.I => I;
    
    ICell IColumn.A => A;
    ICell IColumn.B => B;
    ICell IColumn.C => C;
    ICell IColumn.D => D;
    ICell IColumn.E => E;
    ICell IColumn.F => F;
    ICell IColumn.G => G;
    ICell IColumn.H => H;
    ICell IColumn.I => I;
    
    ICell IRegion.A => A;
    ICell IRegion.B => B;
    ICell IRegion.C => C;
    ICell IRegion.D => D;
    ICell IRegion.E => E;
    ICell IRegion.F => F;
    ICell IRegion.G => G;
    ICell IRegion.H => H;
    ICell IRegion.I => I;
    
    public int Count => 9;
    public override string ToString()
    {
        return $"{A},{B},{C} {D},{E},{F} {G},{H},{I}";
    }
    
    public IEnumerator<MutableCell> GetEnumerator()
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

    ICell IReadOnlyList<ICell>.this[int i]=> i switch
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
    public MutableCell this[int i]
    {
        get => i switch
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
        set
        {
            switch (i)
            {
                case 0:
                    A = value;
                    break;
                case 1:
                    B = value;
                    break;
                case 2:
                    C = value;
                    break;
                case 3:
                    D = value;
                    break;
                case 4:
                    E = value;
                    break;
                case 5:
                    F = value;
                    break;
                case 6:
                    G = value;
                    break;
                case 7:
                    H = value;
                    break;
                case 8:
                    I = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i));
            }
        }
    }
    
    public MutableCell this[int rowIndex, int columnIndex]
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
        set
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
            this[index] = value;
        }
    }

    public bool SetSolved(CellValue value)
    {
        var changed = Remaining.Remove(value);
        changed = Solved.Add(value)|| changed;
        //todo: consider looking for more solutions
        return changed;
    }
}