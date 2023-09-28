using System;
using System.Linq;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.Samples.Friends.UIToolkit
{
    public class LocalPlayerViewUIToolkit : ILocalPlayerView
    {
        const string k_PlayerEntryRootName = "local-player-entry";

        //We dont support the player selecting OFFLINE or UNKNOWN with the UI
        static readonly string[] k_LocalPlayerChoices = { "Online", "Busy", "Away", "Invisible" };

        public Action<(Availability, string)> onPresenceChanged { get; set; }

        DropdownField m_PlayerStatusDropDown;
        Label m_PlayerName;
        TextField m_PlayerActivity;
        VisualElement m_PlayerStatusCircle;
        Button m_AcceptChangeButton;
        Button m_CancelChangeButton;
        Button m_CopyButton;
        string m_LastActivityString;

        public LocalPlayerViewUIToolkit(VisualElement viewParent)
        {
            var playerEntryView = viewParent.Q(k_PlayerEntryRootName);
            m_PlayerStatusDropDown = playerEntryView.Q<DropdownField>("player-status-dropdown");
            m_PlayerName = playerEntryView.Q<Label>("player-name-label");
            m_PlayerStatusCircle = playerEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = playerEntryView.Q<TextField>("player-activity-field");
            m_AcceptChangeButton = m_PlayerActivity.Q<Button>("player-accept");
            m_CancelChangeButton = m_PlayerActivity.Q<Button>("player-cancel");
            m_CopyButton = playerEntryView.Q<Button>("copy-name-button");
            m_CopyButton.RegisterCallback<ClickEvent>((_) =>
            {
                GUIUtility.systemCopyBuffer = m_PlayerName.text;
            });
            
            m_AcceptChangeButton.RegisterCallback<ClickEvent>((_) =>
            {
                var currentOption = ParseStatus(m_PlayerStatusDropDown.value);
                onPresenceChanged?.Invoke((currentOption, m_PlayerActivity.text));
                m_PlayerActivity.Blur();
            });

            m_CancelChangeButton.RegisterCallback<ClickEvent>((_) =>
            {
                m_PlayerActivity.Blur();
            });

            m_PlayerStatusDropDown.choices = k_LocalPlayerChoices.ToList();
            m_PlayerStatusDropDown.RegisterValueChangedCallback(choice =>
            {
                var choiceInt = m_PlayerStatusDropDown.choices.IndexOf(choice.newValue);
                SetPresenceColor(choiceInt);
                var option = ParseStatus(choice.newValue);
                onPresenceChanged?.Invoke((option, m_PlayerActivity.text));
            });

            SetPresence(Availability.Invisible);
        }

        //Keeping these setters seperate in case we wan to support name and activity changes`
        public void Refresh(string name, string activity,
            Availability availability)
        {
            m_PlayerName.text = name;
            m_PlayerActivity.SetValueWithoutNotify(activity);
            SetPresence(availability);
        }

        void SetPresence(Availability presenceStatus)
        {
            var index = (int)presenceStatus - 1;
            var dropDownChoice = m_PlayerStatusDropDown.choices[index];
            m_PlayerStatusDropDown.SetValueWithoutNotify(dropDownChoice);
            SetPresenceColor(index);
        }

        void SetPresenceColor(int index)
        {
            var presenceColor = ColorUtils.GetPresenceColor(index);
            m_PlayerStatusCircle.style.backgroundColor = presenceColor;
        }

        Availability ParseStatus(string status)
        {
            var capsStatus = status.ToUpper();
            if (Enum.TryParse(capsStatus, out Availability parsedOption))
            {
                return parsedOption;
            }

            return Availability.Unknown;
        }
    }
}
