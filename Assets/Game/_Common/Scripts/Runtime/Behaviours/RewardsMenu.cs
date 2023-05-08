using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class RewardsMenu : Dialog<RewardsMenu>
    {
        [SerializeField] private GameObject rewardPrefab;
        [SerializeField] private RectTransform rewardsRT;

        public void ClearRewards()
        {
            rewardsRT.DestroyChildren();
        }
        public void AddReward(Sprite icon)
        {
            Instantiate(rewardPrefab, rewardsRT).transform.GetChild(0).GetComponent<Image>().sprite = icon;
        }
    }
}