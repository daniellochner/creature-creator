using UnityEngine.Localization.Settings;

namespace DanielLochner.Assets
{
    public static class LocalizationUtility
    {
        public static bool HasEntry(string entry)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString("ui-static", entry) != "<empty>";
        }

        public static string Localize(string entry, params object[] arguments)
        {
            return string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("ui-static", entry), arguments);
        }
    }
}