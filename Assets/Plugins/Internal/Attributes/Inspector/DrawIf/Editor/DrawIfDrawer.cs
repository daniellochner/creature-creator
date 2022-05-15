using UnityEditor;

namespace DanielLochner.Assets
{
    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfDrawer : DrawIfDrawerBase<DrawIfAttribute>
    {
    }
}