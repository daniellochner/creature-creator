using UnityEngine;

namespace DanielLochner.Assets
{
    public class ButtonAttribute : PropertyAttribute
    {
        public string methodName;
        public object[] methodArguments;
        public float buttonHeight;

        public ButtonAttribute(string methodName, float buttonHeight = 25f, params object[] methodArguments)
        {
            this.methodName = methodName;
            this.buttonHeight = buttonHeight;
            this.methodArguments = methodArguments;
        }
    }
}