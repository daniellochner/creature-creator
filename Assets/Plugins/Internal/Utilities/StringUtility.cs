using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class StringUtility
    {
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string NoParse(this string str)
        {
            return $"<noparse>{str}</noparse>";
        }

        public static string ToBold(this string str)
        {
            return $"<b>{str}</b>";
        }

        public static string ToColour(this string str, Color colour)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(colour)}>{str}</color>";
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
                string buffer = "";
                int n = list.Count;
                for (int i = 0; i < n - 1; i++)
                {
                    buffer += list[i];
                    if (i < n - 2)
                    {
                        buffer += ", ";
                    }
                }
                return buffer + " and " + list[n - 1];
            }
        }
    }
}