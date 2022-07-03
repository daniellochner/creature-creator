using UnityEngine;

using DanielLochner.Assets.CreatureCreator;
using DanielLochner.Assets;
using System.Collections.Generic;
using UnityEditor;
using BodyPart = DanielLochner.Assets.CreatureCreator.BodyPart;

public class Testing : MonoBehaviour
{

    public Database bps;

    //[ContextMenu("VOLUME")]
    //public void GETVOLUME()
    //{
    //    for (int i = 0; i<2; i++)
    //    {
    //        BodyPartConstructor bpc = bp[i].GetComponent<BodyPartConstructor>();
    //        Mesh mesh;

    //        Renderer renderer = bpc.Model.GetComponentInChildren<Renderer>();
    //        if (renderer is SkinnedMeshRenderer)
    //        {
    //            mesh = (renderer as SkinnedMeshRenderer).sharedMesh;
    //        }
    //        else
    //        {
    //            mesh = bpc.Model.GetComponentInChildren<MeshFilter>().sharedMesh;
    //        }

    //        Debug.Log(MeshVolume.VolumeOfMesh(mesh));
    //    }

    //}

    //public Database bps;

    [ContextMenu("UPDATE")]
    public void UpdateScale()
    {
        foreach (object obj in bps.Objects.Values)
        {
            BodyPart bp = obj as BodyPart;
            bp.Weight = 1;
            EditorUtility.SetDirty(bp);
        }
    }


    //[ContextMenu("UPDATE")]
    //public void UpdateEditorDrag()
    //{
    //    foreach (object obj in bps.Objects.Values)
    //    {
    //        BodyPart bp = obj as BodyPart;

    //        GameObject editor = bp.GetPrefab(BodyPart.PrefabType.Editable);

    //        Drag rDrag = editor.gameObject.AddComponent<Drag>();
    //        rDrag.mouseButton = 1;
    //        rDrag.mousePlaneAlignment = Drag.MousePlaneAlignment.WithCamera;
    //        rDrag.boundsShape = Drag.BoundsShape.Sphere;
    //        rDrag.sphereRadius = 0.1f;
    //        rDrag.updatePlaneOnPress = true;

    //        Debug.Log(editor.name);
    //    }


    //}




    //public Database db;

    //[ContextMenu("Print")]
    //public void Print()
    //{
    //    string text = "";

    //    foreach (string id in db.Objects.Keys)
    //    {
    //        BodyPart bp = db.Objects[id] as BodyPart;

    //        text += id + "," + bp.name + "\n";
    //    }


    //    Debug.Log(text);
    //}


    //[ContextMenu("Realign")]
    //public void Realign()
    //{
    //    foreach (UnlockableBodyPart ubp in GetComponentsInChildren<UnlockableBodyPart>())
    //    {
    //        if (Physics.Raycast(ubp.transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hitInfo, 2f))
    //        {
    //            ubp.transform.position = hitInfo.point;
    //        }
    //        else
    //        {
    //            Debug.Log(ubp.name, ubp);
    //        }
    //    }
    //}

    //[ContextMenu("RemoveBPC")]
    //public void RemoveBPC()
    //{
    //    foreach (UnlockableBodyPart ubp in GetComponentsInChildren<UnlockableBodyPart>())
    //    {
    //        BodyPartConstructor bpc = ubp.GetComponentInChildren<BodyPartConstructor>();
    //        DestroyImmediate(bpc);
    //    }
    //}


    //[ContextMenu("Assign")]
    //public void Assign()
    //{
    //    List<string> patterns = new List<string>(db.Objects.Keys);


    //    patterns.Remove("9aba0c43");
    //    patterns.Remove("f070ab8f");
    //    patterns.Remove("ab1607c4");
    //    patterns.Remove("777bc7d0");

    //    patterns.Remove("99138f48");
    //    patterns.Remove("9d6e0e71");
    //    patterns.Remove("442d0d0e");
    //    patterns.Remove("0b06a02d");


    //    foreach (UnlockablePattern up in GetComponentsInChildren<UnlockablePattern>())
    //    {
    //        if (!string.IsNullOrEmpty(up.patternID))
    //        {
    //            patterns.Remove(up.patternID);
    //        }
    //    }


    //    patterns.Shuffle();


    //    //foreach (UnlockablePattern up in GetComponentsInChildren<UnlockablePattern>())
    //    //{
    //    //    if (string.IsNullOrEmpty(up.patternID))
    //    //    {
    //    //        up.patternID = patterns[0];
    //    //        patterns.RemoveAt(0);
    //    //    }
    //    //}


    //    foreach (string p in patterns)
    //    {
    //        Debug.Log(p);
    //    }

    //}

    //public Material patternMat;
    //public Material bodyPartMat;

    //public UnlockableBodyPart ubp;
    //public UnlockablePattern up;
    //public UnlockableCollection uc;

    //public Transform bodyPartsT;
    //public Transform patternsT;
    //public Transform collectionsT;

    //public Database bodyPartsDB;

    //[ContextMenu("MOVE")]
    //public void Move()
    //{
    //    foreach (UnlockableItem item in GetComponentsInChildren<UnlockableItem>())
    //    {
    //        if (item is UnlockableBodyPart)
    //        {
    //            UnlockableBodyPart t1 = PrefabUtility.InstantiatePrefab(ubp, bodyPartsT) as UnlockableBodyPart;
    //            t1.transform.SetPositionAndRotation(item.transform.position, item.transform.rotation);
    //            t1.bodyPartID = (item as UnlockableBodyPart).bodyPartID;

    //            DanielLochner.Assets.CreatureCreator.BodyPart bp = bodyPartsDB.Objects[t1.bodyPartID] as DanielLochner.Assets.CreatureCreator.BodyPart;

    //            GameObject go = PrefabUtility.InstantiatePrefab(bp.GetPrefab(DanielLochner.Assets.CreatureCreator.BodyPart.PrefabType.Constructible), t1.transform.GetChild(1)) as GameObject;

    //            Transform PREV = item.transform.GetChild(0).GetChild(0); // change this when go to FARM!
    //            go.transform.SetPositionAndRotation(PREV.position, PREV.rotation);
    //            go.transform.localScale = PREV.localScale;

    //            BodyPartConstructor bpc = go.GetComponent<BodyPartConstructor>();
    //            Material mat = new Material(bodyPartMat);

    //            Renderer r = bpc.GetComponentInChildren<Renderer>();
    //            Material[] mats = new Material[r.materials.Length];
    //            for (int i = 0; i < r.materials.Length; i++)
    //            {
    //                mats[i] = mat;
    //            }
    //            r.materials = mats;

    //            DestroyImmediate(bpc);
    //        }
    //        else 
    //        if (item is UnlockablePattern)
    //        {
    //            UnlockablePattern t2 = PrefabUtility.InstantiatePrefab(up, patternsT) as UnlockablePattern;
    //            t2.transform.SetPositionAndRotation(item.transform.position, item.transform.rotation);
    //            t2.patternID = (item as UnlockablePattern).patternID;

    //            Material mat = new Material(patternMat);
    //            mat.mainTexture = item.GetComponentInChildren<MeshRenderer>().material.mainTexture;
    //            t2.GetComponentInChildren<MeshRenderer>().material = mat;
    //        }
    //        else
    //        {
    //            UnlockableCollection t3 = PrefabUtility.InstantiatePrefab(uc, collectionsT) as UnlockableCollection;
    //            t3.transform.SetPositionAndRotation(item.transform.position, item.transform.rotation);
    //            t3.items = (item as UnlockableCollection).items;
    //        }
    //    }
    //}

    //public Database db;

    //[ContextMenu("SEARCH")]
    //public void SearchPatterns()
    //{
    //    List<string> existing = new List<string>()
    //    {
    //        "99138f48",
    //        "9d6e0e71",
    //        "442d0d0e",
    //        "0b06a02d",
    //        "b27472b2",
    //        "d578a721",
    //        "783ddfbf",
    //        "9aba0c43",
    //        "f070ab8f",
    //        "ab1607c4",
    //        "777bc7d0",
    //        "d699da29",
    //        "21e3dff8",
    //        "13207240",
    //        "ffb2d8ba"
    //    };


    //    List<string> patternIDs = new List<string>(db.Objects.Keys);

    //    foreach (string e in existing)
    //    {
    //        patternIDs.Remove(e);
    //    }


    //    foreach (UnlockablePattern p in GetComponentsInChildren<UnlockablePattern>())
    //    {
    //        if (p.patternID != "")
    //        {
    //            patternIDs.Remove(p.patternID);
    //        }
    //    }

    //    foreach (string s in patternIDs)
    //    {
    //        Debug.Log(s);
    //    }


    //}


    //public Material mat;
    //[ContextMenu("MATERIAL")]
    //public void MATERIAL()
    //{
    //    foreach (UnlockableBodyPart ubp in GetComponentsInChildren<UnlockableBodyPart>())
    //    {
    //        Renderer r = ubp.transform.GetChild(1).GetChild(2).GetComponentInChildren<Renderer>();

    //        Material[] mats = new Material[r.materials.Length];
    //        for (int i = 0; i < mats.Length; i++)
    //        {
    //            mats[i] = mat;
    //        }
    //        r.materials = mats;
    //    }
    //}


    //[ContextMenu("MOVE UP")]
    //public void MoveUp()
    //{
    //    foreach (UnlockableBodyPart ubp in GetComponentsInChildren<UnlockableBodyPart>())
    //    {
    //        Transform model = ubp.transform.GetChild(1).GetChild(2).GetChild(0);

    //        Mesh mesh = null;

    //        SkinnedMeshRenderer smr = model.GetComponentInChildren<SkinnedMeshRenderer>();
    //        if (smr == null)
    //        {
    //            mesh = model.GetComponentInChildren<MeshFilter>().sharedMesh;
    //        }
    //        else
    //        {
    //            mesh = smr.sharedMesh;
    //        }

    //        float minY = Mathf.Infinity;
    //        float maxY = Mathf.NegativeInfinity;

    //        foreach (Vector3 vertex in mesh.vertices)
    //        {
    //            Vector3 worldV = model.L2WSpace(vertex);

    //            if (worldV.y > maxY)
    //            {
    //                maxY = worldV.y;
    //            }
    //            else if (worldV.y < minY)
    //            {
    //                minY = worldV.y;
    //            }
    //        }


    //        float avgY = (maxY + minY) / 2f;


    //        Transform effect1 = ubp.transform.GetChild(1).GetChild(0);
    //        Transform effect2 = ubp.transform.GetChild(1).GetChild(1);

    //        effect1.position = effect2.position = new Vector3(effect1.position.x, avgY, effect1.position.z);
    //    }
    //}
}