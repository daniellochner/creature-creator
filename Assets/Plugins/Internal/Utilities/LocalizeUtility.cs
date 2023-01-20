using UnityEngine.Localization.Settings;

namespace DanielLochner.Assets
{
    public static class LocalizeUtility
    {
        public static bool HasEntry(string entry)
        {
            return LocalizationSettings.StringDatabase.GetTable("ui-static").GetEntry(entry) != null;
        }

        public static string Localize(string entry, params object[] arguments)
        {
            return string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("ui-static", entry), arguments);
        }
    }
}