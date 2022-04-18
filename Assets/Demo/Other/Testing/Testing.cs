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




}