using Solver.Domain.Cell;

namespace Solver.Domain.Board;

public static class NineCellExtensions
{
    /// <summary>
    /// IsValid if all the non-null cells are unique. Is Solved if all distinct values are present 
    /// </summary>
    /// <param name="cells"></param>
    /// <returns></returns>
    public static (bool IsValid, bool IsSolved) Check(this IEnumerable<ICell>? cells)
    {
        if (cells == null)
        {
            return (false, false);
        }

        var hash = new HashSet<ICell>(9);
        var count = 0;
        foreach (var cell in cells)
        {
            if (cell.Value == null)
            {
                continue;
            }

            if (!hash.Add(cell))
            {
                return (false, false);
            }

            count++;
        }

        if (count != hash.Count)
        {
            return (false, false);
        }

        return (true, hash.Count == 9);
        ;
    }

    public static IEnumerable<ICell> GetValues(this IEnumerable<ICell>? cells)
    {
        if (cells == null)
        {
            return Array.Empty<ICell>();
        }

        return cells.Where(c => c.Value != null);
    }

    public static (HashSet<CellValue> remaining, HashSet<CellValue> solved) GetStats(this IReadOnlyList<ICell> cells)
    {
        var existing = cells.Aggregate(new HashSet<CellValue>(), (hashSet, cell) =>
        {
            if (cell.Value.HasValue)
            {
                hashSet.Add(cell.Value.Value);
            }

            return hashSet;
        });
        var remaining = new HashSet<CellValue>(CellValue.AllValues.Where(c => !existing.Contains(c)));
        return (remaining, existing);
    }

    internal static MutableNineCell GetMutableNineCell(this IReadOnlyCollection<ICell> cells)
    {
        if (cells is MutableNineCell mutable)
        {
            return mutable;
        }

        if (cells.Count != 9)
        {
            throw new ArgumentException("NineCell must have exactly 9 cells", nameof(cells));
        }

        return new MutableNineCell(GetMutableCells(cells));
    }

    internal static IReadOnlyCollection<MutableCell> GetMutableCells(this IReadOnlyCollection<ICell> cells)
    {
        return cells.Select(c => new MutableCell(c.Value, c.State.RemainingValues)).ToArray();
    }

    public static bool TryGetHiddenPair(
        this IReadOnlyList<MutableCell> cells,
        out (MutableCell one, MutableCell two, CellValue value1, CellValue value2)? result)
    {
        for (var firstIndex = 0; firstIndex < cells.Count - 1; firstIndex++)
        {
            var first = cells[firstIndex];
            if (first.Value.HasValue || first.RemainingCellValues.Count == 1 || first.RemainingCellValues.Count == CellValue.AllValues.Count)
            {
                continue;
            }

            for (var secondIndex = firstIndex + 1; secondIndex < cells.Count; secondIndex++)
            {
                var second = cells[secondIndex];
                if (second.Value.HasValue || second.RemainingCellValues.Count == 1 || second.RemainingCellValues.Count == CellValue.AllValues.Count)
                {
                    continue;
                }

                var intersect = first.RemainingCellValues
                    .Intersect(second.RemainingCellValues)
                    .ToArray();
                if (intersect.Length < 3)
                {
                    continue;
                }

                var others = cells
                    .Where(c => !c.Value.HasValue && !first.Equals(c) && !second.Equals(c))
                    .ToArray();
                if (others.Length == 0)
                {
                    continue;
                }

                var otherRemainingValues = others
                    .Select(m => m.RemainingCellValues)
                    .ToArray();
                var otherUnion = otherRemainingValues
                    .Skip(1)
                    .Aggregate((IEnumerable<CellValue>) otherRemainingValues.First(), (a, b) => a.Union(b));
                var otherHash = new HashSet<CellValue>(otherUnion);

                var distinctIntersect = intersect
                    .Where(i => !otherHash.Contains(i))
                    .ToArray();
                if (distinctIntersect.Length != 2)
                {
                    continue;
                }

                result = (first, second, distinctIntersect[0], distinctIntersect[1]);
                return true;
            }
        }

        result = null;
        return false;
    }
}