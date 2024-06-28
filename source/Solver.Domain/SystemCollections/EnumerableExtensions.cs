namespace Solver.Domain.SystemCollections;

public static class EnumerableExtensions
{
    public static IEnumerable<IReadOnlyList<T>> Permutate<T>(this IEnumerable<T> items, int count)
    {
        if (count < 1) yield break;
        if(count == 1)
        {
            foreach (var x in items.Select(x => new T[] {x}))
            {
                yield return x;
            }
            yield break;
        }

        var itemsArray = items as T[] ?? items.ToArray();
        if (itemsArray.Length == 0 || itemsArray.Length < count)
        {
            yield break;
        }
        if (count >= itemsArray.Length)
        {
            yield return itemsArray;
            yield break;
        }
        var rest = new Queue<T>(itemsArray);

        do
        {
            var first = rest.Dequeue();
            foreach (var window in rest.Permutate(count-1))
            {
                yield return window.Prepend(first).ToArray();
            }
        } while (rest.Count> count-2);
        
    }
}