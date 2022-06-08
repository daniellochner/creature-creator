using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace DanielLochner.Assets
{
    public class AddStateWindow : EditorWindow
    {
        #region Fields
        [SerializeField] private string id;
        [SerializeField] private string[] types;

        private SerializedProperty _id, _types;
        private SerializedObject target;

        private int selected;
        private string[] qualifiedTypes;
        #endregion

        #region Properties
        public static StateMachine TargetStateMachine { get; set; }
        #endregion

        #region Methods
        private void OnEnable()
        {
            target = new SerializedObject(this);

            _id = target.FindProperty("id");
            _types = target.FindProperty("types");


            List<Type> t = new List<Type>();
            Type type = TargetStateMachine.GetType();
            while (type != typeof(StateMachine))
            {
                t.AddRange(type.GetNestedTypes());
                type = type.BaseType;
            }

            types = new string[t.Count];
            qualifiedTypes = new string[t.Count];
            for (int i = 0; i < t.Count; i++)
            {
                types[i] = t[i].Name;
                qualifiedTypes[i] = t[i].AssemblyQualifiedName;
            }
        }
        private void OnGUI()
        {
            target.Update();

            EditorGUILayout.PropertyField(_id);
            selected = EditorGUILayout.Popup("Type", selected, types);

            if (GUILayout.Button("Add"))
            {
                Type stateType = Type.GetType(qualifiedTypes[selected]);
                BaseState state = (BaseState)Activator.CreateInstance(stateType);
                state.StateMachine = TargetStateMachine;
                state.ID = id;

                TargetStateMachine.States.Add(state);

                Close();
            }
            else
            {
                target.ApplyModifiedProperties();
            }
        }

        [MenuItem("Window/State Machine/Add State")]
        public static void ShowWindow()
        {
            bool selected = false;

            if (Selection.activeGameObject != null)
            {
                StateMachine stateMachine = Selection.activeGameObject.GetComponent<StateMachine>();
                if (stateMachine != null)
                {
                    TargetStateMachine = stateMachine;

                    AddStateWindow window = GetWindow<AddStateWindow>("Add State");
                    window.position = new Rect((Screen.width / 2f) - 150f, (Screen.height / 2f) - 50f, 300f, 100f);
                    window.ShowModal();

                    selected = true;
                }
            }

            if (!selected)
            {
                Debug.LogError("A valid state machine must be selected.");
            }
        }
        #endregion
    }
}