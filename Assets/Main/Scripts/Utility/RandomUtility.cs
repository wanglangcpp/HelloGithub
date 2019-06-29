using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public static class RandomUtility
    {
        public static IList<T> RandomSelectFromCollection<T>(IList<T> collection, int count)
        {
            IList<T> res = new List<T>();
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                int index = r.Next(i, collection.Count);
                T temp = collection[i];
                collection[i] = collection[index];
                res.Add(collection[index]);
                collection[index] = temp;
            }
            return res;
        }
    }
}
