using DanielLochner.Assets;
using System.Text.RegularExpressions;
using UnityEngine;

public class PrefabNameTrimmer : MonoBehaviour
{
    [SerializeField, Button("Trim")] private bool trim;

    public void Trim()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (Regex.IsMatch(t.gameObject.name, ".* ([0-9]*)"))
            {
                t.gameObject.name = t.gameObject.name.Substring(0, t.name.LastIndexOf('('));
            }
        }
    }
}
