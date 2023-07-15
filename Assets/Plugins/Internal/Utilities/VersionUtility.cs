using UnityEngine;

namespace DanielLochner.Assets
{
    public static class VersionUtility
    {
        public static System.Version GetVersion(string version)
        {
            string v = version.Replace("-edu", "").Replace("-beta", "");
            return new System.Version(v);
        }
    }
}