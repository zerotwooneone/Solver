using Solver.Domain.Cell;
using Solver.Domain.SystemCollections;

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
    
    public readonly struct CellsToUpdate(
        IReadOnlyCollection<ICell> cells,
        IReadOnlyCollection<CellValue> newRemainingValues)
    {
        public IReadOnlyCollection<ICell> Cells { get; } = cells;
        public IReadOnlyCollection<CellValue> NewRemainingValues { get; } = newRemainingValues;
    }

    public static bool TryGetHidden(
        this IReadOnlyList<ICell> cells,
        out CellsToUpdate? result)
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
                            result = new CellsToUpdate(new []{first} , new []{distinctIntersect3[0]});
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

                    result = result = new CellsToUpdate(new []{first, second, third} , new []{distinctIntersect3[0], distinctIntersect3[1], distinctIntersect3[2]});
                    return true;
                }

                var intersect = first.RemainingCellValues
                    .Intersect(second.RemainingCellValues)
                    .ToArray();
                if (intersect.Length != 2)
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

                result =result = new CellsToUpdate(new []{first, second} , new []{distinctIntersect[0], distinctIntersect[1]});
                return true;
            }
        }

        result = null;
        return false;
    }
    
    public static bool TryGetPointing(
        IReadOnlyList<ICell> regionCells,
        IReadOnlyList<ICell> otherRegionCells,
        IReadOnlyList<ICell> otherCells,
        out IEnumerable<CellsToUpdate>? result)
    {
        if (regionCells.Count != 3)
        {
            throw new ArgumentException($"{nameof(regionCells)} must have 3 cells");
        }
        if (otherRegionCells.Count != 6)
        {
            throw new ArgumentException($"{nameof(otherRegionCells)} must have 6 cells");
        }

        if (otherCells.Count != 6)
        {
            throw new ArgumentException($"{nameof(otherCells)} must have 6 cells");
        }

        var otherRegionRemaining = otherRegionCells.Select(c => c.RemainingCellValues).UnionSelf().ToHashSet();

        var commonTriple = regionCells.Select(c=>c.RemainingCellValues.Where(v=>!otherRegionRemaining.Contains(v))).IntersectSelf().ToHashSet();
        
        var list = new List<CellsToUpdate>();
        if (commonTriple.Count != 0)
        {
            var tripleMatches = otherCells
                .Select(c => (cell: c, remaining:c.RemainingCellValues.Where(v => !commonTriple.Contains(v)).ToArray()))
                .Where(t => t.remaining.Length < t.cell.RemainingCellValues.Count);
            foreach (var tripleMatch in tripleMatches)
            {
                list.Add(new CellsToUpdate( new []{tripleMatch.cell}, tripleMatch.remaining));
            }
        }
        
        var pair1 = regionCells.Take(2);
        var pair1Commons = pair1.Select(c=>c.RemainingCellValues.Where(v=>!otherRegionRemaining.Contains(v))).IntersectSelf().Where(v=> !commonTriple.Contains(v));
        if (pair1Commons.Count() != 0)
        {
            var matches = otherCells
                .Select(c => (cell: c, remaining:c.RemainingCellValues.Where(v => !pair1Commons.Contains(v)).ToArray()))
                .Where(t => t.remaining.Length < t.cell.RemainingCellValues.Count);
            foreach (var match in matches)
            {
                list.Add(new CellsToUpdate( new []{match.cell}, match.remaining));
            }
        }
        var pair2 = regionCells.Skip(1).Take(2);
        var pair2Commons = pair2.Select(c=>c.RemainingCellValues.Where(v=>!otherRegionRemaining.Contains(v))).IntersectSelf().Where(v=> !commonTriple.Contains(v));
        if (pair2Commons.Count() != 0)
        {
            var matches = otherCells
                .Select(c => (cell: c, remaining:c.RemainingCellValues.Where(v => !pair2Commons.Contains(v)).ToArray()))
                .Where(t => t.remaining.Length < t.cell.RemainingCellValues.Count);
            foreach (var match in matches)
            {
                list.Add(new CellsToUpdate( new []{match.cell}, match.remaining));
            }
        }
        var pair3 = new[]{ regionCells[0], regionCells[2] };
        var pair3Commons = pair3.Select(c=>c.RemainingCellValues.Where(v=>!otherRegionRemaining.Contains(v))).IntersectSelf().Where(v=> !commonTriple.Contains(v));
        if (pair3Commons.Count() != 0)
        {
            var matches = otherCells
                .Select(c => (cell: c, remaining:c.RemainingCellValues.Where(v => !pair3Commons.Contains(v)).ToArray()))
                .Where(t => t.remaining.Length < t.cell.RemainingCellValues.Count);
            foreach (var match in matches)
            {
                list.Add(new CellsToUpdate( new []{match.cell}, match.remaining));
            }
        }

        if (list.Count == 0)
        {
            result = null;
            return false;
        }

        result = list;
        return true;
    }

    private static IEnumerable<T> IntersectSelf<T>(
        this IEnumerable<IEnumerable<T>> outer)
    {
        return outer.Skip(1).Aggregate(outer.First(),
            (a, c) => a.Intersect(c));
    }
    
    private static IEnumerable<T> UnionSelf<T>(
        this IEnumerable<IEnumerable<T>> outer)
    {
        return outer.Skip(1).Aggregate(outer.First(),
            (a, c) => a.Union(c));
    }

    public static bool TryGetNaked(this IReadOnlyList<ICell> cells, out IEnumerable<CellsToUpdate>? hiddenRemaining)
    {
        if (cells.Count != 9) throw new ArgumentException("cells must have a count of 9");
        //todo: extend this to include triples and quads
        var withoutValues = cells.Where(c => c.Value == null).ToArray();
        if (withoutValues.Length < 2)
        {
            hiddenRemaining = null;
            return false;
        }

        var result = new List<CellsToUpdate>();

        var with2Remaining = withoutValues
            .Where(c=>c.RemainingCellValues.Count == 2)
            .ToArray();
        var withGt2Remaining = withoutValues
            .Where(c=>c.RemainingCellValues.Count > 2)
            .ToArray();
        if (with2Remaining.Length > 1 && withGt2Remaining.Length > 0)
        {
            var pairs = with2Remaining
                .Permutate(2)
                .Select(p =>
                {
                    var union = p.Select(c => c.RemainingCellValues).UnionSelf().ToArray();
                    return (cells: p, commonRemaining: union);
                })
                .Where(t=>t.commonRemaining.Length == 2)
                .ToArray();
            foreach (var pair in pairs)
            {
                var withCommonRemaining = withGt2Remaining
                    .Where(c=>pair.commonRemaining.Any(v=>c.RemainingCellValues.Contains(v)))
                    .ToArray();
                foreach (var target in withCommonRemaining)
                {
                    var targetRemaining = target.RemainingCellValues.Where(v=>!pair.commonRemaining.Contains(v)).ToArray();
                    result.Add(new CellsToUpdate(
                        new []{target}, 
                        targetRemaining));
                }
            }
        }
        if (result.Count > 0)
        {
            hiddenRemaining = result;
            return true;
        }

        hiddenRemaining = null;
        return false;
    }
}