using UnityEditor;
using UnityEngine;

namespace UnityEditor {
    public class LPVegetation_MaterialInspector : ShaderGUI {

        MaterialProperty metallic = null;
        MaterialProperty smoothness = null;

        MaterialProperty mainTex = null;
        MaterialProperty noiseTexture = null;
        MaterialProperty noiseTextureTiling = null;
        MaterialProperty noisePannerSpeed = null;

        MaterialProperty mbDefaultBending = null;
        MaterialProperty mbAmplitude = null;
        MaterialProperty mbAmplitudeOffset = null;
        MaterialProperty mbFrequency = null;
        MaterialProperty mbFrequencyOffset = null;
        MaterialProperty mbPhase = null;
        MaterialProperty mbWindDirection = null;
        MaterialProperty mbWindDirectionOffset = null;
        MaterialProperty mbMaxHeight = null;

        MaterialEditor matEditor;

        public void FindProperties(MaterialProperty[] mProps)
        {
            metallic                    = FindProperty("_Metallic", mProps);
            smoothness                  = FindProperty("_Smoothness", mProps);

            mainTex                     = FindProperty("_MainTex", mProps);
            noiseTexture                = FindProperty("_NoiseTexture", mProps);
            noiseTextureTiling          = FindProperty("_NoiseTextureTilling", mProps);
            noisePannerSpeed            = FindProperty("_NoisePannerSpeed", mProps);

            mbDefaultBending            = FindProperty("_MBDefaultBending", mProps);
            mbAmplitude                 = FindProperty("_MBAmplitude", mProps);
            mbAmplitudeOffset           = FindProperty("_MBAmplitudeOffset", mProps);
            mbFrequency                 = FindProperty("_MBFrequency", mProps);
            mbFrequencyOffset           = FindProperty("_MBFrequencyOffset", mProps);
            mbPhase                     = FindProperty("_MBPhase", mProps);
            mbWindDirection             = FindProperty("_MBWindDir", mProps);
            mbWindDirectionOffset       = FindProperty("_MBWindDirOffset", mProps);
            mbMaxHeight                 = FindProperty("_MBMaxHeight", mProps);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] mProps)
        {
            matEditor = materialEditor;
            Material material = materialEditor.target as Material;

            FindProperties(mProps);
            ShaderPropertiesGUI(material);
        }

        public void ShaderPropertiesGUI(Material material)
        {
            EditorGUI.BeginChangeCheck();
            {
                EditorGUIUtility.fieldWidth = 64f;

                EditorGUILayout.Separator();
                InspectorBox(10, () =>
                {
                    EditorGUILayout.LabelField(new GUIContent("Surface"), EditorStyles.boldLabel);
                    GUILayout.Space(5);
                    matEditor.TexturePropertySingleLine(new GUIContent("Albedo"), mainTex);
                    GUILayout.Space(5);
                    matEditor.ShaderProperty(metallic, "Metallic");
                    matEditor.ShaderProperty(smoothness, "Smoothness");
                });

                EditorGUILayout.Separator();
                InspectorBox(10, () =>
                {
                    //EditorGUILayout.LabelField(new GUIContent("Main Bending"), EditorStyles.boldLabel);
       
                    //GUILayout.Space(5);
                    matEditor.ShaderProperty(mbDefaultBending, new GUIContent("Default Bending", "The base bending applied to the model."));

                    GUILayout.Space(5);
                    matEditor.ShaderProperty(mbAmplitude, new GUIContent("Amplitude", "The amplitude of the main bending."));
                    matEditor.ShaderProperty(mbAmplitudeOffset, new GUIContent("Amplitude Offset", "The amplitude offset of the main bending. The value of this field is multiplied with a static noise value and added to the main bending amplitude."));

                    GUILayout.Space(5);
                    matEditor.ShaderProperty(mbFrequency, new GUIContent("Frequency", "The frequency of the main bending."));
                    matEditor.ShaderProperty(mbFrequencyOffset, new GUIContent("Frequency Offset", "The frequency offset of the main bending. The value of this field is multiplied with a static noise value and added to the main bending frequency."));

                    GUILayout.Space(5);
                    matEditor.ShaderProperty(mbPhase, new GUIContent("Phase", "The phase of the main bending. A phase shift is applied based on the position the game object has on the XZ axis. "
                        + "If the main bending of the models that are close to each other is synchronous, try to increase the value of this field."));

                    GUILayout.Space(5);
                    matEditor.ShaderProperty(mbWindDirection, new GUIContent("Wind Dir", "The direction of the wind."));
                    matEditor.ShaderProperty(mbWindDirectionOffset, new GUIContent("Wind Dir Offset", "The wind direction offset. " 
                        + "This value is multiplied with an animated noise value and added to the wind direction to create wind direction variation over time."));

                    GUILayout.Space(5);
                    matEditor.ShaderProperty(mbMaxHeight, new GUIContent("Max Height", "The height of the tallest model that uses this material. " 
                        + "This value is used to calculate the final main bending amplitude of a vertex."));
                });

                EditorGUILayout.Separator();
                InspectorBox(10, () =>
                {
                    EditorGUILayout.LabelField(new GUIContent("World Space Noise"), EditorStyles.boldLabel);

                    GUILayout.Space(5);
                    matEditor.TexturePropertySingleLine(new GUIContent("Noise Texture"), noiseTexture);
                    matEditor.ShaderProperty(noiseTextureTiling, new GUIContent("Noise Tiling: Static (XY), Animated (ZW)", "Noise texture tiling. "
                        + "The XY values are used for static noise tiling and ZW for animated noise tiling."));
                    matEditor.ShaderProperty(noisePannerSpeed, new GUIContent("Noise Panning Speed", "The panning speed of the noise texture."));
                });

                EditorGUILayout.Separator();
                InspectorBox(10, () =>
                {
                    GUILayout.Label("Advanced Options", EditorStyles.boldLabel);

                    matEditor.EnableInstancingField();
                });
            }
        }

        public void InspectorBox(int aBorder, System.Action inside)
        {
            Rect r = EditorGUILayout.BeginHorizontal();

            GUI.Box(r, GUIContent.none);
            GUILayout.Space(aBorder);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(aBorder);
            inside();
            GUILayout.Space(aBorder);
            EditorGUILayout.EndVertical();
            GUILayout.Space(aBorder);
            EditorGUILayout.EndHorizontal();
        }
    }
}
