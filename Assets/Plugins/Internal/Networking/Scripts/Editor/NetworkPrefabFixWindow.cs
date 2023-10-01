using UnityEngine;
using UnityEditor;
using Unity.Netcode;

namespace DanielLochner.Assets
{
    public class NetworkPrefabFixWindow : EditorWindow
    {
        #region Fields
        [SerializeField] private NetworkPrefabsList prefabs;

        private SerializedProperty _prefabs;
        private SerializedObject target;
        #endregion

        #region Properties
        public static NetworkPrefabFixWindow Window => GetWindow<NetworkPrefabFixWindow>("Network Prefab Fix", false);
        #endregion

        #region Methods
        private void OnEnable()
        {
            target = new SerializedObject(this);

            _prefabs = target.FindProperty("prefabs");
        }
        private void OnGUI()
        {
            target.Update();

            EditorGUILayout.PropertyField(_prefabs);
            if (GUILayout.Button("Fix"))
            {
                Fix();
            }
            target.ApplyModifiedProperties();
        }

        public void Fix()
        {
            foreach (NetworkPrefab prefab in prefabs.PrefabList)
            {
                NetworkObject obj = prefab.Prefab.GetComponent<NetworkObject>();
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