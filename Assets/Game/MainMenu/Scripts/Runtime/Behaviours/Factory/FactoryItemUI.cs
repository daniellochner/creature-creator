// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryItemUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI upVotesText;
        [SerializeField] private Image previewImg;
        [SerializeField] private Button pageBtn;
        [SerializeField] private Button subscribeBtn;
        [SerializeField] private Button likeBtn;
        [SerializeField] private Sprite addIcon;
        [SerializeField] private Sprite removeIcon;
        [Space]
        [SerializeField] private GameObject info;
        [SerializeField] private GameObject refreshIcon;
        [SerializeField] private GameObject errorIcon;

        private Coroutine previewCoroutine;
        private bool isLiked, isDisliked, isSubscribed;
        private FactoryItem item;
        #endregion

        #region Methods
        public void Setup(FactoryItem item)
        {
            this.item = item;

            nameText.text = item.name;
            upVotesText.text = item.upVotes.ToString();

            SetSubscribed(FactoryManager.Data.SubscribedItems.Contains(item.id));
            SetLiked(FactoryManager.Data.LikedItems.Contains(item.id));
            SetDisliked(FactoryManager.Data.DislikedItems.Contains(item.id));
            SetPreview(item.previewURL);
        }

        public void View()
        {
            FactoryItemMenu.Instance.View(item, this, previewImg.sprite, isSubscribed, isLiked, isDisliked);
        }
        public void Like()
        {
            if (!isLiked)
            {
                FactoryManager.Instance.LikeItem(item.id);
            }
            SetLiked(!isLiked);
        }
        public void Subscribe()
        {
            if (isSubscribed)
            {
                FactoryManager.Instance.UnsubscribeItem(item.id);
            }
            else
            {
                FactoryManager.Instance.SubscribeItem(item.id);
            }
            SetSubscribed(!isSubscribed);
        }

        public void SetLiked(bool isLiked)
        {
            this.isLiked = isLiked;
            uint likes = item.upVotes + (isLiked ? 1u : 0u);
            upVotesText.text = $"{likes}";
        }
        public void SetDisliked(bool isDisliked)
        {
            this.isDisliked = isDisliked;
        }
        public void SetSubscribed(bool isSubscribed)
        {
            this.isSubscribed = isSubscribed;
            subscribeBtn.image.sprite = isSubscribed ? removeIcon : addIcon;
        }

        public void SetPreview(string url)
        {
            this.StopStartCoroutine(SetPreviewRoutine(url), ref previewCoroutine);
        }
        private IEnumerator SetPreviewRoutine(string url)
        {
            refreshIcon.SetActive(true);

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D preview = ((DownloadHandlerTexture)request.downloadHandler).texture;
                previewImg.sprite = Sprite.Create(preview, new Rect(0, 0, preview.width, preview.height), new Vector2(0.5f, 0.5f));
                info.SetActive(true);
            }
            else
            {
                errorIcon.SetActive(true);
            }

            refreshIcon.SetActive(false);
        }
        #endregion
    }
}