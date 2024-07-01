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

    public static bool TryGetHidden(
        this IReadOnlyList<MutableCell> cells,
        out ((MutableCell one, CellValue value1)? single,
            (MutableCell one, MutableCell two, CellValue value1, CellValue value2)? pair,
            (MutableCell one, MutableCell two, MutableCell three, CellValue value1, CellValue value2, CellValue value3)? triple)? result)
    {
        for (var firstIndex = 0; firstIndex < cells.Count - 1; firstIndex++)
        {
            var first = cells[firstIndex];
            if (first.Value.HasValue || first.RemainingCellValues.Count == 1 || first.RemainingCellValues.Count == CellValue.AllValues.Count)
            {
                continue;
            }

            if (first.RemainingCellValues.Count >= 2)
            {
                var others3 = cells
                    .Where(c => !c.Value.HasValue && !first.Equals(c))
                    .ToArray();
                if (others3.Length > 0)
                {
                    var otherRemainingValues3 = others3
                        .Select(m => m.RemainingCellValues)
                        .ToArray();
                    var otherUnion3 = otherRemainingValues3
                        .Skip(1)
                        .Aggregate((IEnumerable<CellValue>) otherRemainingValues3.First(), (a, b) => a.Union(b));
                    var otherHash3 = new HashSet<CellValue>(otherUnion3);

                    var distinctIntersect3 = first.RemainingCellValues
                        .Where(i => !otherHash3.Contains(i))
                        .ToArray();
                    if (distinctIntersect3.Length == 1)
                    {
                        //todo: try to remove this check
                        if (cells.FirstOrDefault(c => c.Value == distinctIntersect3[0]) == null)
                        {
                            result = (single: (first, distinctIntersect3[0]),
                                pair: null,
                                triple: null);
                            return true;
                        }
                    }
                }
            }
            
            for (var secondIndex = firstIndex + 1; secondIndex < cells.Count; secondIndex++)
            {
                var second = cells[secondIndex];
                if (second.Value.HasValue || second.RemainingCellValues.Count == 1 || second.RemainingCellValues.Count == CellValue.AllValues.Count)
                {
                    continue;
                }

                for (var thirdIndex = secondIndex + 1; thirdIndex < cells.Count; thirdIndex++)
                {
                    var third = cells[thirdIndex];
                    if (third.Value.HasValue || third.RemainingCellValues.Count == 1 || third.RemainingCellValues.Count == CellValue.AllValues.Count)
                    {
                        continue;
                    }

                    var intersect3 = first.RemainingCellValues
                        .Intersect(second.RemainingCellValues)
                        .Intersect(third.RemainingCellValues)
                        .ToArray();
                    if (intersect3.Length < 4)
                    {
                        continue;
                    }

                    var others3 = cells
                        .Where(c => !c.Value.HasValue && !first.Equals(c) && !second.Equals(c) && !third.Equals(c))
                        .ToArray();
                    if (others3.Length == 0)
                    {
                        continue;
                    }

                    var otherRemainingValues3 = others3
                        .Select(m => m.RemainingCellValues)
                        .ToArray();
                    var otherUnion3 = otherRemainingValues3
                        .Skip(1)
                        .Aggregate((IEnumerable<CellValue>) otherRemainingValues3.First(), (a, b) => a.Union(b));
                    var otherHash3 = new HashSet<CellValue>(otherUnion3);

                    var distinctIntersect3 = intersect3
                        .Where(i => !otherHash3.Contains(i))
                        .ToArray();
                    if (distinctIntersect3.Length != 3)
                    {
                        continue;
                    }

                    result = (single: null,
                        pair: null,
                            triple: (first, second, third, distinctIntersect3[0], distinctIntersect3[1], distinctIntersect3[2]));
                    return true;
                }

                var intersect = first.RemainingCellValues
                    .Intersect(second.RemainingCellValues)
                    //dds .Where(v=>!first.Row.Solved.Contains(v) && !first.Column.Solved.Contains(v) && !first.Region.Solved.Contains(v) && !second.Row.Solved.Contains(v) && !second.Column.Solved.Contains(v) && !second.Region.Solved.Contains(v))
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

                result = (single: null,
                    pair: (first, second, distinctIntersect[0], distinctIntersect[1]), 
                    triple: null);
                return true;
            }
        }

        result = null;
        return false;
    }
}