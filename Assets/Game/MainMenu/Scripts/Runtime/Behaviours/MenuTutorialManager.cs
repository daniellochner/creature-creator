using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MenuTutorialManager : MonoBehaviourSingleton<MenuTutorialManager>
    {
        #region Fields
        [SerializeField] private Menu playMenu;
        [SerializeField] private Menu singleplayerMenu;
        [SerializeField] private Menu createMenu;
        #endregion

        #region Methods
        public IEnumerator Setup()
        {
            // Play
            playMenu.Open();
            yield return new WaitUntil(() => !playMenu.IsOpen);

            // Singleplayer
            yield return new WaitForSeconds(0.5f);
            singleplayerMenu.Open();
            yield return new WaitUntil(() => !singleplayerMenu.IsOpen);

            // Create
            yield return new WaitForSeconds(0.5f);
            createMenu.Open();
        }
        #endregion
    }
}