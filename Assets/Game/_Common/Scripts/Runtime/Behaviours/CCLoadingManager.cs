using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CCLoadingManager : LoadingManager
    {
        public override IEnumerator WaitUntilRoutine()
        {
            if (GameSetup.Instance)
            {
                yield return new WaitUntilSetup(GameSetup.Instance);
                yield return new WaitUntil(() => Player.Instance && Player.Instance.IsSetup);
            }
        }
    }
}