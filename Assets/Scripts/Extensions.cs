using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static void ForEach<T>(this IEnumerable<T> items, System.Action<T> action)
    {
        if (items is List<T> list)
        {
            list.ForEach(action);
        }
        else
        {
            foreach (var item in items)
            {
                action?.Invoke(item);
            }
        }
    }
}
