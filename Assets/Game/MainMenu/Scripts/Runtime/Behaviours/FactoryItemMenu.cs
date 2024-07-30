using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryItemMenu : MenuSingleton<FactoryItemMenu>
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI authorText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI upVotesText;
        public TextMeshProUGUI downVotesText;

        public Image iconImg;
        public Image subscribeImg;
        public Sprite addIcon;
        public Sprite removeIcon;

        public GameObject previewIcon;
        public GameObject refreshIcon;
        public GameObject errorIcon;


        private FactoryItem item;

        private bool isLiked, isDisliked, isSubscribed;
        private Coroutine previewCoroutine;
        private FactoryItemUI itemUI;


        public void View(FactoryItem item, FactoryItemUI itemUI, Sprite preview = null, bool isSubscribed = false, bool isLiked = false, bool isDisliked = false)
        {
            this.item = item;
            this.isSubscribed = isSubscribed;
            this.isLiked = isLiked;
            this.isDisliked = isDisliked;
            this.itemUI = itemUI;

            nameText.text = item.name;
            authorText.text = item.author;
            descriptionText.text = item.description;
            upVotesText.text = item.upVotes.ToString();
            downVotesText.text = item.downVotes.ToString();

            if (preview == null)
            {
                SetPreview(item.previewURL);
            }
            else
            {
                iconImg.sprite = preview;
            }

            SetSubscribed(isSubscribed);
            SetLiked(isLiked);
            SetDisliked(isDisliked);

            Open();
        }

        public void Like()
        {
            if (!isLiked)
            {
                FactoryManager.Instance.LikeItem(item.id);
            }
            SetLiked(!isLiked);

            if (isDisliked)
            {
                SetDisliked(false);
            }
        }
        public void Dislike()
        {
            if (!isDisliked)
            {
                FactoryManager.Instance.DislikeItem(item.id);
            }
            SetDisliked(!isDisliked);

            if (isLiked)
            {
                SetLiked(false);
            }
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
        public void ViewMore()
        {
            FactoryManager.Instance.ViewWorkshopItem(item.id);
        }

        public void SetLiked(bool isLiked)
        {
            this.isLiked = isLiked;

            uint likes = item.upVotes + (isLiked ? 1u : 0u);
            upVotesText.text = $"{likes}";

            itemUI.SetLiked(isLiked);
            itemUI.SetDisliked(isDisliked);
        }
        public void SetDisliked(bool isDisliked)
        {
            this.isDisliked = isDisliked;

            uint dislikes = item.downVotes + (isDisliked ? 1u : 0u);
            downVotesText.text = $"{dislikes}";

            itemUI.SetDisliked(isDisliked);
            itemUI.SetLiked(isLiked);
        }
        public void SetSubscribed(bool isSubscribed)
        {
            this.isSubscribed = isSubscribed;
            subscribeImg.sprite = this.isSubscribed ? removeIcon : addIcon;

            itemUI.SetSubscribed(isSubscribed);
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
                iconImg.sprite = Sprite.Create(preview, new Rect(0, 0, preview.width, preview.height), new Vector2(0.5f, 0.5f));
                previewIcon.SetActive(true);
            }
            else
            {
                errorIcon.SetActive(true);
            }

            refreshIcon.SetActive(false);
        }
    }
}