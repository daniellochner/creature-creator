namespace DanielLochner.Assets
{
    public class DrawIfAttribute : DrawIfAttributeBase
    {
        public DrawIfAttribute(string propertyName, object propertyValue, bool indent = false, bool readOnly = false) : base(propertyName, propertyValue, indent, readOnly) { }
    }
}