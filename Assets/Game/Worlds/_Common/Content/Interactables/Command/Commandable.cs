using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Commander))]
    public class Commandable : CreatureInteractable
    {
        #region Fields
        private Commander commander;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            commander = GetComponent<Commander>();
        }
        protected override void OnInteract(Interactor interactor)
        {
            commander.TryCommand(interactor);
        }
        #endregion
    }
}