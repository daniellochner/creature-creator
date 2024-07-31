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
        [SerializeField] protected float height;

        private ProfanityFilter filter = new ProfanityFilter();
        private float cooldownTimeLeft;
        protected GameObject messageGO;
        #endregion

        #region Properties
        public bool CheckForProfanity
        {
            get => checkForProfanity;
            set => checkForProfanity = value;
        }

        public virtual bool CanSend => !InputDialog.Instance.IsOpen && cooldownTimeLeft < 0;
        public virtual bool CanReceive => Camera.main != null;

        public virtual bool TryOpen => Input.GetKeyDown(KeyCode.T);
        #endregion

        #region Methods
        private void Update()
        {
            if (IsOwner)
            {
                HandleInput();
            }
        }
        private void OnDisable()
        {
            if (messageGO != null)
            {
                Destroy(messageGO);
            }
        }

        protected void HandleInput()
        {
            if (TryOpen)
            {
                Open();
            }
            cooldownTimeLeft -= Time.deltaTime;
        }

        public void Open()
        {
            if (CanSend)
            {
                InputDialog.Input(LocalizationUtility.Localize("send_message_title"), onSubmit: SendMessage, maxCharacters: characterLimit);
            }
        }

        public new void SendMessage(string message)
        {
            if (message.Length == 0)
            {
                return;
            }
            if (message.Length > characterLimit)
            {
                InformationDialog.Inform(LocalizationUtility.Localize("sent_message_too_long_title"), LocalizationUtility.Localize("sent_message_too_long_message", characterLimit));
                return;
            }
            if (checkForProfanity && filter.ContainsProfanity(message))
            {
                InformationDialog.Inform(LocalizationUtility.Localize("profanity_detected_title"), LocalizationUtility.Localize("profanity_detected_message"));
                return;
            }
            cooldownTimeLeft = sendCooldown;

            SendMessageServerRpc(message);
        }
        protected virtual void ReceiveMessage(string message)
        {
            if (messageGO != null)
            {
                Destroy(messageGO);
            }
            messageGO = Instantiate(messagePrefab, transform.position + transform.up * height, transform.rotation, transform);

            messageGO.GetComponentInChildren<TextMeshProUGUI>().text = message.NoParse();
            messageGO.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });

            PlayerChatManager.Instance.SendChatMessageSelf(OwnerClientId, message);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendMessageServerRpc(string message)
        {
            SendMessageClientRpc(message);
        }

        [ClientRpc]
        public void SendMessageClientRpc(string message)
        {
            if (CanReceive)
            {
                ReceiveMessage(message);
            }
        }
        #endregion
    }
}