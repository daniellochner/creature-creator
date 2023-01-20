using UnityEngine.Localization.Settings;

namespace DanielLochner.Assets
{
    public static class LocalizeUtility
    {
        public static string Localize(string entry, params object[] arguments)
        {
            return string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("ui-static", entry), arguments);
        }
    }
}