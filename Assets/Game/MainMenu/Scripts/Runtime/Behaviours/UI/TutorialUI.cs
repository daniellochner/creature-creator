using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TutorialUI : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(SettingsManager.Instance.ShowTutorial);
        }
    }
}