using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FastTravelReminder : MonoBehaviour
    {
        private IEnumerator Start()
        {
            if (PlayerPrefs.GetInt("FAST_TRAVEL_REMINDER") == 0)
            {
                yield return new WaitUntil(() => EditorManager.Instance.IsPlaying);
                yield return new WaitForSeconds(1f);

                InformationDialog.Inform("Fast Travel!", "Click on the minimap to view the entire world. You can pan and zoom using your cursor. Then, click on an editing platform to teleport to it!");
                PlayerPrefs.SetInt("FAST_TRAVEL_REMINDER", 1);
            }
        }
    }
}