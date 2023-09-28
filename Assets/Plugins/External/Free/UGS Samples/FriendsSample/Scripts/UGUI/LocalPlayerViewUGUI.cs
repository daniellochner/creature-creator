using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class LocalPlayerViewUGUI : MonoBehaviour, ILocalPlayerView
    {
        public Action<(Availability, string)> onPresenceChanged { get; set; }

        [SerializeField] TextMeshProUGUI m_NameText = null;
        [SerializeField] TMP_InputField m_Activity = null;
        [SerializeField] TMP_Dropdown m_PresenceSelector = null;
        [SerializeField] Image m_PresenceColor = null;
        [SerializeField] Button m_CopyButton = null;
        
        void Awake()
        {
            var names = new List<string>
            {
                "Online",
                "Busy",
                "Away",
                "Invisible"
            };

            m_PresenceSelector.AddOptions(names);
            m_PresenceSelector.onValueChanged.AddListener((value) => { OnStatusChanged(value, m_Activity.text); });
            m_Activity.onEndEdit.AddListener((value) => { OnStatusChanged(m_PresenceSelector.value, value); });
            m_CopyButton.onClick.AddListener(() => { GUIUtility.systemCopyBuffer = m_NameText.text; });
        }

        void OnStatusChanged(int value, string activity)
        {
            var presence = (Availability)Enum.Parse(typeof(Availability),
                m_PresenceSelector.options[value].text, true);

            onPresenceChanged?.Invoke((presence, activity));
        }

        public void Refresh(string name, string activity, Availability availability)
        {
            m_NameText.text = name;

            //Presence
            var index = (int)availability - 1;
            m_PresenceSelector.SetValueWithoutNotify(index);
            var presenceColor = ColorUtils.GetPresenceColor(index);
            m_PresenceColor.color = presenceColor;

            m_Activity.text = activity;
        }
    }
}
