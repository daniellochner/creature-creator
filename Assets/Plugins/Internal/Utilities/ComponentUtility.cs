using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DanielLochner.Assets
{
    public static class ComponentUtility
    {
        private static Dictionary<string, object> copy = new Dictionary<string, object>();
        private const BindingFlags FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        
        public static void CopyValues(this Component component, bool debug = false, BindingFlags flags = FLAGS, Type root = null)
        {
            copy.Clear();
            Type type = component.GetType();
            while (type != typeof(Component))
            {
                FieldInfo[] fields = type.GetFields(flags);
                foreach (FieldInfo field in fields)
                {
                    if (copy.ContainsKey(field.Name)) continue;
                    copy.Add(field.Name, field.GetValue(component));
                }

                if (type == root)
                {
                    break;
                }
                type = type.BaseType;
            }
            if (debug) Debug.Log($"Copied {copy.Count} fields from '{component.name}'.");
        }
        public static void PasteValues(this Component component, bool debug = false, BindingFlags flags = FLAGS, Type root = null)
        {
            int count = 0;
            Type type = component.GetType();
            foreach (string name in copy.Keys)
            {
                while (type != typeof(Component))
                {
                    FieldInfo field = type.GetField(name, flags);
                    if (field != null)
                    {
                        field.SetValue(component, copy[name]);
                        count++;
                        break;
                    }

                    if (type == root)
                    {
                        break;
                    }
                    type = type.BaseType;
                }
            }
            if (debug) Debug.Log($"Pasted {count} values to '{component.name}'.");
        }

#if UNITY_EDITOR
        public static void MoveToTop(this Component component)
        {
            while (UnityEditorInternal.ComponentUtility.MoveComponentUp(component)) ;
        }
#endif
    }
}