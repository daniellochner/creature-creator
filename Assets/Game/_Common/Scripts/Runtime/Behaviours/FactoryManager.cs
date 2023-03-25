using UnityEngine;
using System.IO;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryManager : MonoBehaviourSingleton<FactoryManager>
    {
#if UNITY_STANDALONE
        public void LoadWorkshopItems()
        {
            uint n = SteamUGC.GetNumSubscribedItems();
            if (n > 0)
            {
                PublishedFileId_t[] files = new PublishedFileId_t[n];
                SteamUGC.GetSubscribedItems(files, n);

                foreach (PublishedFileId_t fileId in files)
                {
                    if (SteamUGC.GetItemInstallInfo(fileId, out ulong sizeOnDisk, out string folder, 1024, out uint timeStamp))
                    {
                        string src = Directory.GetFiles(folder)[0];
                        string dst = Path.Combine(Application.persistentDataPath, "creature", Path.GetFileName(src));
                        if (!File.Exists(dst))
                        {
                            File.Copy(src, dst);
                        }
                    }
                }
            }
        }
#endif
    }
}