// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Toggle selectToggle;
        [SerializeField] private Button errorButton;
        [SerializeField] private Button removeButton;
        [SerializeField] private Button shareButton;
        [SerializeField] private GameObject progress;
        #endregion

        #region Properties
        public TextMeshProUGUI NameText => nameText;
        public Toggle SelectToggle => selectToggle;
        public Button ErrorButton => errorButton;
        public Button RemoveButton => removeButton;
        public Button ShareButton => shareButton;
        public GameObject Progress => progress;

        public bool IsSharing
        {
            set
            {
                progress.SetActive(value);
                shareButton.gameObject.SetActive(!value);
            }
        }
        #endregion

        #region Methods
        public void Setup(string creatureName)
        {
            nameText.text = name = creatureName;
            transform.SetAsFirstSibling();

#if UNITY_STANDALONE
            foreach (PublishedFileId_t fileId in FactoryManager.Instance.Files)
            {
                if (SteamUGC.GetItemInstallInfo(fileId, out ulong sizeOnDisk, out string folder, 1024, out uint timeStamp))
                {
                    string src = Directory.GetFiles(folder)[0];
                    if (Path.GetFileName(src) == creatureName)
                    {
                        shareButton.gameObject.SetActive(false);
                    }
                }
            }
#endif
        }
        #endregion
    }
}