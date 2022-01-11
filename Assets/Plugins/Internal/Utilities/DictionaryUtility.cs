using System.Collections.Generic;
using System.Linq;

namespace DanielLochner.Assets
{
    public static class DictionaryUtility
    {
        public static K GetKeyFromValue<K, V>(this Dictionary<K, V> dict, V value)
        {
            return dict.FirstOrDefault(x => x.Value.Equals(value)).Key;
        }
    }
}