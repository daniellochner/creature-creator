namespace DanielLochner.Assets.CreatureCreator
{
    public class Battle : CreatureInteractable
    {
        public override bool CanInteract(Interactor interactor)
        {
            return base.CanInteract(interactor) && !BattleManager.Instance.InBattle;
        }
        protected override void OnInteract(Interactor interactor)
        {
            base.OnInteract(interactor);
            BattleManager.Instance.TryBattle();
        }
    }
}