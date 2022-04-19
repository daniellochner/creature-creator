// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockableBodyPart : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string bodyPartID;
        [SerializeField] private bool isUnlockable = true;
        #endregion

        #region Properties
        public BodyPart BodyPart => DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);
        #endregion

        #region Methods
        private void Start()
        {
            if (ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartID))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isUnlockable)
            {
                EditorManager.Instance.UnlockBodyPart(bodyPartID);
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}