using UnityEngine;

namespace DanielLochner.Assets
{
    public class HasFlagAttribute : PropertyAttribute
    {
        public string enumPropertyName;
        public object enumFlagValue;

        public HasFlagAttribute(string enumPropertyName, object enumFlagValue)
        {
            this.enumPropertyName = enumPropertyName;
            this.enumFlagValue = enumFlagValue;
        }
    }
}