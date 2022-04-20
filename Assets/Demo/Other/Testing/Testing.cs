using UnityEngine;

using DanielLochner.Assets.CreatureCreator;
using DanielLochner.Assets;
using System.Collections.Generic;

public class Testing : MonoBehaviour
{
    public Database db;

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


}