using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureNamer : PlayerNamer
    {
        #region Properties
        public CreatureConstructor Constructor { get; set; }
        public CreatureLoader Loader { get; set; }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();
            Constructor = GetComponent<CreatureConstructor>();
            Loader = GetComponent<CreatureLoader>();
        }

        public override void SetVisible(bool isActive)
        {
            base.SetVisible(isActive);
            if (isActive && playerName != null)
            {
                playerName.transform.localPosition = Vector3.up * (Constructor.Dimensions.Height + height); // this.height functions as an offset in this case
            }
        }
        #endregion
    }
}