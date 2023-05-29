using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Read : CreatureInteractable
    {
        #region Fields
        [SerializeField] private string titleId;
        [SerializeField] private string messageId;
        #endregion

        #region Methods
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            InformationDialog.Inform(LocalizationUtility.Localize(titleId), LocalizationUtility.Localize(messageId));
        }
        #endregion
    }
}