using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace DanielLochner.Assets
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(LocalizeStringEvent))]
    public class LocalizedText : TextMeshProUGUI
    {
        private LocalizeStringEvent localizedStringEvent;

        private static readonly object[] ARG_BUFFER = new object[] { null, null, null, null, null };

        protected override void Awake()
        {
            base.Awake();
            localizedStringEvent = GetComponent<LocalizeStringEvent>();
            SetArguments(ARG_BUFFER);
        }

        public void SetArguments(params object[] arguments)
        {
            localizedStringEvent.StringReference.Arguments = arguments;
            localizedStringEvent.StringReference.RefreshString();
        }
    }
}