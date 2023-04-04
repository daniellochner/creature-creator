namespace Gley.About
{
    using UnityEditor;
    using UnityEngine;

    public class AssetStorePackage
    {
        public AssetState assetState;
        public Texture2D texture;
        public string name;
        public string folderName;
        public string description;
        public string url;

        public AssetStorePackage(string folderName, string name, Texture2D texture, string description, string url)
        {
            this.name = name;
            this.folderName = folderName;
            this.description = description;
            this.url = url;
            this.texture = texture;
        }

        public void SetAssetState(AssetState newState)
        {
            assetState = newState;
        }
    }
}
