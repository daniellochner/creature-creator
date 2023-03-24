
using System.IO;
using UnityEngine;
using Steamworks;
using System.Collections;

namespace DanielLochner.Assets.CreatureCreator
{
    public class SteamWorkshopManager : MonoBehaviourSingleton<SteamWorkshopManager>
    {
#if UNITY_STANDALONE

        private CreatureWorkshopData currentWorkshopData;

        private string creaturesDirectory;

        private void Start()
        {
            creaturesDirectory = Path.Combine(Application.persistentDataPath, "creature");
        }

        private void LoadWorkshopItems()
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
                        string srcPath = Directory.GetFiles(folder)[0];

                        string fileName = Path.GetFileName(srcPath);

                        string dstPath = Path.Combine(Path.Combine(creaturesDirectory, fileName));


                        if (!File.Exists(dstPath))
                        {
                            File.Copy(srcPath, dstPath);
                        }

                    }
                }
            }
        }

        public void SubmitToWorkshop(string data, string preview, string title, string description)
        {
            if (SteamManager.Initialized)
            {
                currentWorkshopData = new CreatureWorkshopData()
                {
                    dataPath = data,
                    previewPath = preview,
                    title = title,
                    description = description
                };

                CallResult<CreateItemResult_t> item = CallResult<CreateItemResult_t>.Create(OnCreateItem);

                SteamAPICall_t handle = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeCommunity);
                item.Set(handle);
            }
        }

        UGCUpdateHandle_t updateHandle;

        private void OnCreateItem(CreateItemResult_t item, bool ioFailure)
        {
            if (ioFailure)
            {
                return;
            }
            if (item.m_bUserNeedsToAcceptWorkshopLegalAgreement)
            {
                SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/workshop/workshoplegalagreement/");
                return;
            }

            updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), item.m_nPublishedFileId);


            SteamUGC.SetItemTitle(updateHandle, currentWorkshopData.title);
            SteamUGC.SetItemDescription(updateHandle, currentWorkshopData.description);
            SteamUGC.SetItemContent(updateHandle, currentWorkshopData.dataPath);
            SteamUGC.SetItemPreview(updateHandle, currentWorkshopData.previewPath);

            SteamUGC.SetItemVisibility(updateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);

            SteamUGC.SubmitItemUpdate(updateHandle, null);

            StartCoroutine(UploadRoutine(item.m_nPublishedFileId));
        }

        private IEnumerator UploadRoutine(PublishedFileId_t pf)
        {
            while (true)
            {
                var u = SteamUGC.GetItemUpdateProgress(updateHandle, out ulong p, out ulong t);

                Debug.Log(u + ": " + p + "/" + t);


                if (u == EItemUpdateStatus.k_EItemUpdateStatusInvalid)
                {
                    break;
                }
                yield return null;
            }
            SteamFriends.ActivateGameOverlayToWebPage($"steam://url/CommunityFilePage/{pf}");

        }

        private struct CreatureWorkshopData
        {
            public string dataPath;
            public string previewPath;
            public string title;
            public string description;
        }
#endif
    }
}

