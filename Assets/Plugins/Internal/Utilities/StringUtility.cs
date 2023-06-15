using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DanielLochner.Assets
{
    public static class StringUtility
    {
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string JoinAnd<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return "";
            }
            else
            if (list.Count == 1)
            {
                return list[0].ToString();
            }
            else
            {
                return string.Join(", ", list.ToArray(), 0, list.Count - 1) + " and " + list.LastOrDefault();
            }
        }
    }
}