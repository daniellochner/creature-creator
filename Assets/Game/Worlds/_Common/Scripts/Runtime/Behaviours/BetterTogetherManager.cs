using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BetterTogetherManager : MonoBehaviour
    {
        private IEnumerator Start()
        {
            if (GameSetup.Instance.IsMultiplayer)
            {
                yield return new WaitUntil(() => GameSetup.Instance && GameSetup.Instance.IsSetup && NetworkPlayersMenu.Instance && NetworkPlayersMenu.Instance.NumPlayers > 1);
#if USE_STATS
                StatsManager.Instance.UnlockAchievement("ACH_BETTER_TOGETHER");
#endif
            }
        }
    }
}