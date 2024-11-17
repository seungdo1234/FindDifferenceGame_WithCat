using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class CollectionUtils
{
    
    public static HashSet<T> GetUniqueCollections<T>(IList<T> collection, int count, Predicate<T> predicate = null)
    {
        HashSet<T> hash = new HashSet<T>();
        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, collection.Count);

            if ((predicate == null || predicate(collection[rand])) && !hash.Add(collection[rand]))
            {
                i--;
            }
        }
        return hash;
    }
}
