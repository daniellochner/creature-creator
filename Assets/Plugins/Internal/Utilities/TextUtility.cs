
namespace DanielLochner.Assets
{
    public static class TextUtility
    {
        public static string FormatError(object obj, bool isError)
        {
            if (isError)
            {
                return $"<color=red>{obj}</color>";
            }
            return obj.ToString();
        }
    }
}