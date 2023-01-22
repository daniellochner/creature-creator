using TMPro;

namespace DanielLochner.Assets
{
    public static class TMPUtility
    {
        public static void SetArguments(this TextMeshProUGUI text, params object[] arguments)
        {
            (text as LocalizedText).SetArguments(arguments);
        }
    }
}