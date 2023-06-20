using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Mineable : CreatureInteractable
    {
        #region Fields
        private Miner miner;
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            miner = GetComponent<Miner>();
        }

        public override bool CanHighlight(Interactor interactor)
        {
            return base.CanHighlight(interactor) && Player.Instance.Holder.IsHolding.Value;
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            miner.TryMineServerRpc(interactor.NetworkObject);
        }
        #endregion
    }
}