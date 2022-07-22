using ProfanityDetector;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets
{
    public class PlayerMessenger : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private GameObject messagePrefab;
        [Space]
        [SerializeField] private bool checkForProfanity;
        [SerializeField] private int characterLimit;
        [SerializeField] private float sendCooldown;
        [SerializeField] private float height;

        private ProfanityFilter filter = new ProfanityFilter();
        private float cooldownTimeLeft;
        protected GameObject messageGO;
        #endregion

        #region Properties
        public virtual bool CanSend => !InputDialog.Instance.IsOpen && cooldownTimeLeft < 0;
        public virtual bool CanReceive => true;
        #endregion

        #region Methods
        private void Update()
        {
            if (IsOwner) HandleInput();
        }
        private void OnDisable()
        {
            if (messageGO != null)
            {
                Destroy(messageGO);
            }
        }

        private void HandleInput()
        {
            if (CanSend && Input.GetKey(KeyCode.T))
            {
                InputDialog.Input("World Chat", onSubmit: SendMessage);
            }
            cooldownTimeLeft -= Time.deltaTime;
        }
        
        public new void SendMessage(string message)
        {
            if (!CanSend) return;

            if (message.Length == 0)
            {
                return;
            }
            if (message.Length > characterLimit)
            {
                InformationDialog.Inform("Too Long", $"Messages cannot be longer than {characterLimit} characters!");
                return;
            }
            if (checkForProfanity && filter.ContainsProfanity(message))
            {
                InformationDialog.Inform("Profanity Detected", "Please do not send messages that contain profanity!");
                return;
            }
            cooldownTimeLeft = sendCooldown;

            SendMessageServerRpc(message);
        }
        protected virtual void ReceiveMessage(string message)
        {
            if (!CanReceive) return;

            if (messageGO != null)
            {
                Destroy(messageGO);
            }
            messageGO = Instantiate(messagePrefab, transform.position + transform.up * height, transform.rotation, transform);

            messageGO.GetComponentInChildren<TextMeshProUGUI>().text = message;
            messageGO.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendMessageServerRpc(string message)
        {
            SendMessageClientRpc(message);
        }

        [ClientRpc]
        public void SendMessageClientRpc(string message)
        {
            ReceiveMessage(message);
        }
        #endregion
    }
}