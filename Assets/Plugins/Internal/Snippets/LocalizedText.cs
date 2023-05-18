using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(LocalizeStringEvent))]
    public class LocalizedText : TextMeshProUGUI
    {
        [SerializeField] private string[] defaultArgs;
        private LocalizeStringEvent localizedStringEvent;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            localizedStringEvent = GetComponent<LocalizeStringEvent>();
            if (localizedStringEvent.StringReference.Arguments == null)
            {
                SetArguments(defaultArgs);
            }
        }
        public void SetArguments(params object[] arguments)
        {
            if (!localizedStringEvent)
            {
                Initialize();
            }
            localizedStringEvent.StringReference.Arguments = arguments;
            localizedStringEvent.StringReference.RefreshString();
        }
    }
}