using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class RewardsMenu : Dialog<RewardsMenu>
    {
        #region Fields
        [SerializeField] private GameObject rewardPrefab;
        [SerializeField] private RectTransform rewardsRT;
        private AudioSource audioSource;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        public void Add(Item item)
        {
            Instantiate(rewardPrefab, rewardsRT).transform.GetChild(0).GetComponent<Image>().sprite = item.Icon;
        }
        public void Clear()
        {
            rewardsRT.DestroyChildren();
        }

        public override void Open(bool instant = false)
        {
            base.Open(instant);
            audioSource.Play();
        }
        #endregion
    }
}