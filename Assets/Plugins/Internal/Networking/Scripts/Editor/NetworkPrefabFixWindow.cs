using UnityEngine;
using UnityEditor;
using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class NetworkPrefabFixWindow : EditorWindow
    {
        #region Fields
        [SerializeField] private NetworkObject[] objects;

        private SerializedProperty _objects;
        private SerializedObject target;
        #endregion

        #region Properties
        public static NetworkPrefabFixWindow Window => GetWindow<NetworkPrefabFixWindow>("Network Prefab Fix", false);
        #endregion

        #region Methods
        private void OnEnable()
        {
            target = new SerializedObject(this);

            _objects = target.FindProperty("objects");
        }
        private void OnGUI()
        {
            target.Update();

            EditorGUILayout.PropertyField(_objects);
            if (GUILayout.Button("Fix"))
            {
                Fix();
            }
            target.ApplyModifiedProperties();
        }

        public void Fix()
        {
            foreach (NetworkObject obj in objects)
            {
                obj.AlwaysReplicateAsRoot = !obj.AlwaysReplicateAsRoot;
                obj.AlwaysReplicateAsRoot = !obj.AlwaysReplicateAsRoot;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void FixOnLoad()
        {
            Window.Fix();
        }

        [MenuItem("Window/Networking/Network Prefab Fix")]
        public static void ShowWindow()
        {
            Window.Show();
        }
        #endregion
    }
}