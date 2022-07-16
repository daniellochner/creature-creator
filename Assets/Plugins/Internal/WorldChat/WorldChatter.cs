using ProfanityDetector;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets
{
    public class WorldChatter : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject messagePrefab;
        [Space]
        [SerializeField] private float cooldown;
        [SerializeField] private int characterLimit;

        private ProfanityFilter filter = new ProfanityFilter();
        private GameObject messageGO;
        protected float cooldownTimeLeft, height;
        #endregion

        #region Properties
        public virtual bool CanChat => !InputDialog.Instance.IsOpen && cooldownTimeLeft < 0;
        #endregion

        #region Methods
        protected virtual void Update()
        {
            if (IsOwner) HandleInput();
        }

        private void HandleInput()
        {
            if (CanChat && Input.GetKey(KeyCode.T))
            {
                InputDialog.Input("World Chat", submitEvent: SendChatMessage);
            }
            cooldownTimeLeft -= Time.deltaTime;
        }

        public void SendChatMessage(string message)
        {
            if (message.Length > characterLimit)
            {
                InformationDialog.Inform("Too Long", $"Messages cannot be longer than {characterLimit} characters!");
                return;
            }
            if (filter.ContainsProfanity(message))
            {
                InformationDialog.Inform("Profanity Detected", "Please do not send messages that contain profanity!");
                return;
            }

            SendChatMessageServerRpc(message);
            cooldownTimeLeft = cooldown;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendChatMessageServerRpc(string message)
        {
            SendChatMessageClientRpc(message);
        }

        [ClientRpc]
        public virtual void SendChatMessageClientRpc(string message)
        {
            if (messageGO != null)
            {
                Destroy(messageGO);
            }
            
            messageGO = Instantiate(messagePrefab, transform, false);
            messageGO.transform.localPosition = Vector3.up * height;

            messageGO.GetComponentInChildren<TextMeshProUGUI>().text = message;
            messageGO.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });
            messageGO.GetComponent<SizeMatcher>().Match();
        }
        #endregion
    }
}