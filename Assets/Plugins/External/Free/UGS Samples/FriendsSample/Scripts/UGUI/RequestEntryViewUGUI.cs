using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class RequestEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText = null;

        public Button acceptButton = null;
        public Button declineButton = null;
        public Button blockButton = null;

        public void Init(string playerName)
        {
            m_NameText.text = playerName;
        }
    }
}