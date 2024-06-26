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
        if(cells == null)
        {
            return (false, false);
        }

        var hash = new HashSet<ICell>(9);
        var count = 0;
        foreach (var cell in cells)
        {
            if (cell.Value ==null)
            {
                continue;
            }
            if(!hash.Add(cell))
            {
                return (false, false);
            }

            count++;
        }

        if (count != hash.Count)
        {
            return (false, false);
        }
        return (true, hash.Count == 9);;
    }

    public static IEnumerable<ICell> GetValues(this IEnumerable<ICell>? cells)
    {
        if (cells == null)
        {
            return Array.Empty<ICell>();
        }

        return cells.Where(c => c.Value != null);
    }
}