using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class FriendUI : RelationshipUI
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image statusImg;
        [SerializeField] private Button joinButton;
        [SerializeField] private Sprite joinIcon;
        [SerializeField] private Sprite pingIcon;
        [SerializeField] private Color onlineColour;
        [SerializeField] private Color offlineColour;
        [SerializeField] private Color busyColour;
        #endregion

        #region Properties
        public string JoinedLobbyId
        {
            get
            {
                if (relationship.Member.Presence != null)
                {
                    return relationship.Member.Presence.GetActivity<FriendData>()?.LobbyId;
                }
                return null;
            }
        }
        #endregion

        #region Methods
        public override void Setup(FriendsMenu menu, Relationship relationship)
        {
            base.Setup(menu, relationship);

            if (relationship.Member.Presence != null)
            {
                SetPresence(relationship.Member.Presence);
            }
        }

        public void SetPresence(Presence presence)
        {
            joinButton.enabled = false;

            statusImg.sprite = pingIcon;

            switch (presence.Availability)
            {
                case Availability.Online:
                    if (!string.IsNullOrEmpty(JoinedLobbyId))
                    {
                        statusText.text = LocalizationUtility.Localize("friends_status_in-game");
                        statusImg.sprite = joinIcon;
                        joinButton.enabled = true;
                    }
                    else
                    {
                        statusText.text = LocalizationUtility.Localize("friends_status_online");
                    }
                    statusImg.color = statusText.color = onlineColour;
                    break;
                case Availability.Busy:
                case Availability.Away:
                    statusText.text = LocalizationUtility.Localize("friends_status_busy");
                    statusImg.color = statusText.color = busyColour;
                    break;
                case Availability.Invisible:
                case Availability.Offline:
                case Availability.Unknown:
                    statusText.text = LocalizationUtility.Localize("friends_status_offline");
                    statusImg.color = statusText.color = offlineColour;
                    break;
            }
        }

        public void Join()
        {
            if (!string.IsNullOrEmpty(JoinedLobbyId))
            {
                ConfirmationDialog.Confirm(LocalizationUtility.Localize("friends_join_title"), LocalizationUtility.Localize("friends_join_message", relationship.Member.Profile.Name), onYes: delegate
                {
                    menu.Join(JoinedLobbyId);
                });
            }
        }
        public void Remove()
        {
            ConfirmationDialog.Confirm(LocalizationUtility.Localize("friends_remove_title"), LocalizationUtility.Localize("friends_remove_message", relationship.Member.Profile.Name), onYes: delegate
            {
                FriendsManager.Instance.DeleteFriend(relationship.Member.Id, delegate
                {
                    menu.RefreshOnline();
                });
                Destroy(gameObject);
            });
        }
        #endregion
    }
}