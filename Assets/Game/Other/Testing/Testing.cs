using DanielLochner.Assets;
using DanielLochner.Assets.CreatureCreator;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using static DanielLochner.Assets.CreatureCreator.Settings;

public class Testing : MonoBehaviour
{
    public Database bodyparts;

    [ContextMenu("UPDATE")]
    public void UpdateSMR()
    {
        foreach (var bp in bodyparts.Objects.Values)
        {
            DanielLochner.Assets.CreatureCreator.BodyPart bodypart = bp as DanielLochner.Assets.CreatureCreator.BodyPart;

            foreach (SkinnedMeshRenderer r in bodypart.GetPrefab(DanielLochner.Assets.CreatureCreator.BodyPart.PrefabType.Animatable).GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                r.allowOcclusionWhenDynamic = false;
                r.skinnedMotionVectors = false;
            }
            foreach (SkinnedMeshRenderer r in bodypart.GetPrefab(DanielLochner.Assets.CreatureCreator.BodyPart.PrefabType.Constructible).GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                r.allowOcclusionWhenDynamic = false;
                r.skinnedMotionVectors = false;

            }
            foreach (SkinnedMeshRenderer r in bodypart.GetPrefab(DanielLochner.Assets.CreatureCreator.BodyPart.PrefabType.Editable).GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                r.allowOcclusionWhenDynamic = false;
                r.skinnedMotionVectors = false;

            }
        }
    }
}