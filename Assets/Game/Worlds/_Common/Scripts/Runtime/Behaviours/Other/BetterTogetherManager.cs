using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BetterTogetherManager : MonoBehaviour
    {
        private IEnumerator Start()
        {
            if (WorldManager.Instance.IsMultiplayer)
            {
                yield return new WaitUntil(() => GameSetup.Instance && GameSetup.Instance.IsSetup && NetworkPlayersMenu.Instance && NetworkPlayersMenu.Instance.NumPlayers > 1);
                StatsManager.Instance.UnlockAchievement("ACH_BETTER_TOGETHER");
            }
        }
    }
}