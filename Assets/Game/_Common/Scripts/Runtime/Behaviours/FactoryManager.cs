using UnityEngine;
using System.IO;
using System.Collections;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryManager : MonoBehaviourSingleton<FactoryManager>
    {
#if UNITY_STANDALONE
        public PublishedFileId_t[] Files { get; private set; } = new PublishedFileId_t[0];

        private IEnumerator Start()
        {
            if (EducationManager.Instance.IsEducational) yield break;

            yield return new WaitUntil(() => SteamManager.Initialized);
            LoadWorkshopItems();
        }

        public void LoadWorkshopItems()
        {
            uint n = SteamUGC.GetNumSubscribedItems();
            if (n > 0)
            {
                Files = new PublishedFileId_t[n];
                SteamUGC.GetSubscribedItems(Files, n);

                foreach (PublishedFileId_t fileId in Files)
                {
                    if (SteamUGC.GetItemInstallInfo(fileId, out ulong sizeOnDisk, out string folder, 1024, out uint timeStamp) && Directory.Exists(folder))
                    {
                        string src = Directory.GetFiles(folder)[0];

                        string creaturesDir = Path.Combine(Application.persistentDataPath, "creature");
                        if (!Directory.Exists(creaturesDir))
                        {
                            Directory.CreateDirectory(creaturesDir);
                        }
                        string dst = Path.Combine(creaturesDir, Path.GetFileName(src));

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