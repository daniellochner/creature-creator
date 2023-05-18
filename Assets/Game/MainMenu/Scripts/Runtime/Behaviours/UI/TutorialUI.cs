using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TutorialUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Menu playMenu;
        [SerializeField] private Menu singleplayerMenu;
        [SerializeField] private Menu createMenu;
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            if (SettingsManager.Instance.ShowTutorial)
            {
                yield return new WaitForSeconds(0.5f);
                playMenu.Open();
                yield return new WaitUntil(() => !playMenu.IsOpen);

                yield return new WaitForSeconds(0.5f);
                singleplayerMenu.Open();
                yield return new WaitUntil(() => !singleplayerMenu.IsOpen);

                yield return new WaitForSeconds(0.5f);
                createMenu.Open();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}