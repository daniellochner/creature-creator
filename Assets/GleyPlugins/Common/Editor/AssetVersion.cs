namespace Gley.Common
{
    [System.Serializable]
    public class AssetVersion
    {
        public string folderName;
        public string longVersion;
        public int shortVersion;

        public AssetVersion(string folderName)
        {
            this.folderName = folderName;
        }
    }
}

