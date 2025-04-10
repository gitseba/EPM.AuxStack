using LogProcessor.Application.Abstractions;
using System.Collections.ObjectModel;

namespace LogProcessor.Shared.Extensions;

/// <summary>
/// Purpose: 
/// Created by: tseb
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Check all elements/models that implement <see cref="ICheckable"/>
    /// </summary>
    public static void CheckAll<T>(this Collection<T> collection, bool input) where T : ICheckable
    {
        foreach (var item in collection)
        {
            item.IsChecked = input;
        }
    }

    /// <summary>
    /// Remove only checked items <see cref="ICheckable"/>
    /// </summary>
    public static bool RemoveChecked<T>(this Collection<T> collection) where T : ICheckable
    {
        var results = collection.Where(x => x.IsChecked).ToList();

        foreach (var item in results)
        {
            collection.Remove(item);
        }

        return results.Count != 0;
    }
}
