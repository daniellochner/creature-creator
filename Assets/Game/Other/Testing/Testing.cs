using DanielLochner.Assets;
using DanielLochner.Assets.CreatureCreator;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class Testing : MonoBehaviour
{
    [ContextMenu("ADD OPTIONS")]
    public void AddOptions()
    {
        var table = LocalizationSettings.StringDatabase.GetTable("ui-static");

        AddAll<Map>(table);

        EditorUtility.SetDirty(table);

        EditorUtility.SetDirty(table.SharedData);
    }

    public void AddAll<T>(StringTable table)
    {
        foreach (var type in Enum.GetValues(typeof(T)))
        {
            string id = $"option-{typeof(T).Name.ToLower()}-{type.ToString().ToLower()}";
            table.AddEntry(id, type.ToString());
        }
    }
}