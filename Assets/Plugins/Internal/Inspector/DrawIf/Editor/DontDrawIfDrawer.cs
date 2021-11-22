using UnityEditor;

namespace DanielLochner.Assets
{
    [CustomPropertyDrawer(typeof(DontDrawIfAttribute))]
    public class DontDrawIfDrawer : DrawIfDrawerBase<DontDrawIfAttribute>
    {
        protected override bool CanDraw(SerializedProperty property) => !base.CanDraw(property);
    }
}