using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    [RequireComponent(typeof(CreatureHider))]
    public class CreatureNamer : PlayerNamer
    {
        #region Properties
        public CreatureConstructor Constructor { get; set; }
        public CreatureHider Hider { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Hider = GetComponent<CreatureHider>();
        }

        public override void Setup()
        {
            base.Setup();

            Hider.OnHide += OnHide;
            Hider.OnShow += OnShow;


            if (Hider.IsHidden)
            {
                OnHide();
            }
        }

        private void OnShow()
        {
            if (!IsOwner)
            {
                enabled = true;
                nameGO.transform.localPosition = Vector3.up * (Constructor.Dimensions.height + 0.25f);
            }

            if (NetworkCreaturesMenu.Instance)
            {
                NetworkCreaturesMenu.Instance.SetName(OwnerClientId, Constructor.Data.Name);
            }
        }
        private void OnHide()
        {
            if (!IsOwner)
            {
                enabled = false;
            }

            if (NetworkCreaturesMenu.Instance)
            {
                NetworkCreaturesMenu.Instance.SetName(OwnerClientId, "...");
            }
        }
        #endregion
    }
}