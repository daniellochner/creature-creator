using UnityEngine;

namespace DanielLochner.Assets
{
    public class DrawIfAttributeBase : PropertyAttribute
    {
        public string propertyName;
        public object propertyValue;
        public bool indent;
        public bool readOnly;

        public DrawIfAttributeBase(string propertyName, object propertyValue, bool indent = false, bool readOnly = false)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.indent = indent;
            this.readOnly = readOnly;
        }
    }
}