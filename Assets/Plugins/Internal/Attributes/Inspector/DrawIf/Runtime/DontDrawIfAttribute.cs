namespace DanielLochner.Assets
{
    public class DontDrawIfAttribute : DrawIfAttributeBase
    {
        public DontDrawIfAttribute(string propertyName, object propertyValue, bool indent = false, bool readOnly = false) : base(propertyName, propertyValue, indent, readOnly) { }
    }
}