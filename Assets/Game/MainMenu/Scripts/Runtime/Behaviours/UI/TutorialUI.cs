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
            if (PlayerPrefs.GetInt("MAIN_MENU_TUTORIAL") == 0 && (ProgressManager.Data.UnlockedBodyParts.Count == 0 && ProgressManager.Data.UnlockedPatterns.Count == 0))
            {
                yield return new WaitForSeconds(0.5f);
                playMenu.Open();
                yield return new WaitUntil(() => !playMenu.IsOpen);

                yield return new WaitForSeconds(0.5f);
                singleplayerMenu.Open();
                yield return new WaitUntil(() => !singleplayerMenu.IsOpen);

                yield return new WaitForSeconds(0.5f);
                createMenu.Open();

                PlayerPrefs.SetInt("MAIN_MENU_TUTORIAL", 1);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}