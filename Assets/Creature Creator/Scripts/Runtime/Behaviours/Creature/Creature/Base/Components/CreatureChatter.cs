// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureChatter : WorldChatter
    {
        #region Properties
        public CreatureConstructor Constructor { get; set; }

        public override bool CanChat => base.CanChat && EditorManager.Instance && EditorManager.Instance.IsPlaying;
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        protected override void Update()
        {
            base.Update();
            height = Constructor.Dimensions.height + 0.75f;
        }
        #endregion
    }
}