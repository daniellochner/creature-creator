using UnityEngine;
using DanielLochner.Assets.CreatureCreator;
using DanielLochner.Assets;
using UnityEditor;

public class Testing : MonoBehaviour
{

    //[ContextMenu("SET ACTIVE")]
    //public void SetActive()
    //{
    //    foreach (Transform t in GetComponentsInChildren<Transform>(true))
    //    {
    //        t.gameObject.SetActive(true);
    //    }
    //}


    //public string data;

    //public CreatureConstructor cPrefab;

    //[ContextMenu("CONSTRUCT")]
    //public void Construct()
    //{
    //    var cc = Instantiate(cPrefab);

    //    CreatureData creature = JsonUtility.FromJson<CreatureData>(data);

    //    cc.Construct(creature);
    //}


    //public Database bps;


    //[ContextMenu("UPDATE")]
    //public void UpdateBodyParts()
    //{
    //    foreach (var b in bps.Objects)
    //    {
    //        BodyPart bodyPart = b.Value as BodyPart;

    //        Renderer r = bodyPart.GetPrefab(BodyPart.PrefabType.Constructible).GetComponentInChildren<Renderer>();

    //        Material[] mats = r.sharedMaterials;


    //        if (bodyPart.DefaultColours.primary.a != 0f || bodyPart.DefaultColours.secondary.a != 0f)
    //        {
    //            Debug.Log(bodyPart.name + "COLOURS", bodyPart);
    //        }


    //        foreach (Material mat in mats)
    //        {
    //            if (mat.name == "BodyPart_Primary")
    //            {
    //                Debug.Log(bodyPart.name + " - PRIMARY", bodyPart);
    //            }
    //            else if (mat.name == "BodyPart_Secondary")
    //            {
    //                Debug.Log(bodyPart.name + " - Secondary", bodyPart);
    //            }
    //        }
    //    }
    //}



    //public bool isGrounded;
    //public float contactDistance;
    //public float radius;

    //public BoxCollider col;

    //private void Update()
    //{


    //    //if (Physics.BoxCast(center, halfExtents, dir, orientation, maxDistance))
    //    //{
    //    //    isGrounded = true;
    //    //}
    //    //else
    //    //{
    //    //    isGrounded = false;
    //    //}


    //    Vector3 center = transform.position + Vector3.up * (0.5f + 0.001f);
    //    Vector3 halfExtents = new Vector3(1f, 0.5f, 1f);
    //    Vector3 dir = -transform.up;
    //    Quaternion orientation = transform.rotation;
    //    float maxDistance = 0.5f;

    //    if (Physics.BoxCast(center, halfExtents, Vector3.down, orientation, maxDistance))
    //    {
    //        isGrounded = true;
    //    }
    //    else
    //    {
    //        isGrounded = false;
    //    }


    //}


    //private void Update()
    //{
    //    Vector3 origin = transform.position;
    //    Vector3 dir = -transform.up;
    //    float radius = 0.5f;

    //    if (Physics.SphereCast(origin, radius, dir * 2f, out RaycastHit hitInfo))
    //    {
    //        Debug.Log("TEST");
    //    }
    //}


    //private void OnDrawGizmos()
    //{
    //    Vector3 origin = transform.position;
    //    Vector3 dir = -transform.up;
    //    float radius = 0.5f;

    //    Gizmos.DrawSphere(origin, radius);
    //}


    //public Database bps;

    //[ContextMenu("SETUP")]
    //public void Setup()
    //{
    //    foreach (var bp in bps.Objects)
    //    {
    //        var b = bp.Value as BodyPart;

    //        if (b is Limb)
    //        {
    //            RemoveIt(b.GetPrefab(BodyPart.PrefabType.Constructible));
    //            RemoveIt(b.GetPrefab(BodyPart.PrefabType.Animatable));

    //            Debug.Log(b.name);
    //        }
    //    }
    //}

    //public void RemoveIt(GameObject go)
    //{
    //    foreach (LookAtConstraint lac in go.GetComponentsInChildren<LookAtConstraint>())
    //    {
    //        DestroyImmediate(lac, true);
    //    }
    //    foreach (RotationConstraint rc in go.GetComponentsInChildren<RotationConstraint>())
    //    {
    //        DestroyImmediate(rc, true);
    //    }
    //}

    //public GameObject[] gos;

    //[ContextMenu("Setup constriants")]
    //public void SetupC()
    //{
    //    foreach (GameObject go in gos)
    //    {
    //        Setup(go.GetComponent<LimbConstructor>());
    //    }
    //}

    //public void Setup(LimbConstructor limbConstructor)
    //{
    //    for (int i = 0; i < limbConstructor.Bones.Length; i++)
    //    {
    //        Debug.Log(i);
    //        Transform bone = limbConstructor.Bones[i];
    //        if (i < limbConstructor.Bones.Length - 1)
    //        {
    //            if (bone.gameObject.GetComponent<LookAtConstraint>() != null) continue;

    //            LookAtConstraint boneConstraint = bone.gameObject.AddComponent<LookAtConstraint>();
    //            Quaternion offset = QuaternionUtility.GetRotationOffset(limbConstructor.Bones[i + 1], limbConstructor.Bones[i]);

    //            boneConstraint.AddSource(new ConstraintSource()
    //            {
    //                sourceTransform = limbConstructor.Bones[i + 1],
    //                weight = 1f
    //            });
    //            boneConstraint.rotationAtRest = bone.localEulerAngles;
    //            boneConstraint.rotationOffset = offset.eulerAngles;
    //            boneConstraint.constraintActive = true;
    //            boneConstraint.locked = true;
    //        }
    //        else
    //        {
    //            if (bone.gameObject.GetComponent<RotationConstraint>() != null) continue;
    //            RotationConstraint extremityConstraint = bone.gameObject.AddComponent<RotationConstraint>();
    //            Quaternion offset = QuaternionUtility.GetRotationOffset(bone, limbConstructor.Bones[i - 1]);

    //            extremityConstraint.AddSource(new ConstraintSource()
    //            {
    //                sourceTransform = limbConstructor.Bones[i - 1],
    //                weight = 1f
    //            });
    //            extremityConstraint.rotationAtRest = bone.localEulerAngles;
    //            extremityConstraint.rotationOffset = offset.eulerAngles;
    //            extremityConstraint.constraintActive = true;
    //            //extremityConstraint.locked = true; // Must lock manually - bug with Unity's RotationConstraint
    //        }
    //    }
    //}

    //public Database bps;

    //[ContextMenu("TEST")]
    //void Test()
    //{
    //    foreach (var bp in bps.Objects)
    //    {
    //        BodyPart b = bp.Value as BodyPart;

    //        //CheckAll(b.GetPrefab(BodyPart.PrefabType.Constructible));
    //        //CheckAll(b.GetPrefab(BodyPart.PrefabType.Animatable));
    //        //CheckAll(b.GetPrefab(BodyPart.PrefabType.Editable));

    //        //UpdateCollider(b.GetPrefab(BodyPart.PrefabType.Constructible));
    //        //UpdateCollider(b.GetPrefab(BodyPart.PrefabType.Animatable));
    //        //UpdateCollider(b.GetPrefab(BodyPart.PrefabType.Editable));

    //        //UpdateSMRS(b.GetPrefab(BodyPart.PrefabType.Constructible));
    //        //UpdateSMRS(b.GetPrefab(BodyPart.PrefabType.Animatable));
    //        //UpdateSMRS(b.GetPrefab(BodyPart.PrefabType.Editable));
    //    }



    //    //Destroy(m);
    //}

    //[ContextMenu("UPDATE THIS COL")]
    //private void UpdateCollider()
    //{
    //    Mesh m = new Mesh();

    //    var smr = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

    //    smr.BakeMesh(m);

    //    gameObject.GetComponentInChildren<MeshCollider>().sharedMesh = m;
    //}

    //private void CheckAll(GameObject go)
    //{
    //    if (go.GetComponentInChildren<Testing>() != null)
    //    {
    //        Debug.Log(go, go);
    //    }
    //}

    //[ContextMenu("UPDATE THIS ONE")]
    //void UpdateThisOne()
    //{
    //    UpdateSMRS(gameObject);
    //}


    //void UpdateSMRS(GameObject go)
    //{
    //    Vector3 offset = Vector3.zero;
    //    Transform root = go.transform.Find("Root");

    //    if (root != null)
    //    {
    //        offset = root.localEulerAngles;
    //    }

    //    foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
    //    {
    //        UpdateBounds(smr, offset);
    //    }
    //}

    //void UpdateBounds(SkinnedMeshRenderer smr, Vector3 offset)
    //{
    //    smr.transform.localEulerAngles = offset;

    //    Mesh m = new Mesh();
    //    smr.BakeMesh(m);

    //    smr.updateWhenOffscreen = false;

    //    m.RecalculateBounds();
    //    Debug.Log(m.bounds);

    //    smr.localBounds = new UnityEngine.Bounds(m.bounds.center, m.bounds.size);

    //    smr.transform.localEulerAngles = Vector3.zero;

    //    //smr.bounds = new UnityEngine.Bounds(m.bounds.center, m.bounds.size);

    //    //Destroy(m);
    //}


    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject, collision.gameObject);
    //}


    //public GameObject source;
    //public GameObject target;

    //[ContextMenu("COMPARE")]
    //public void Compare()
    //{
    //    List<Component> sourceComps = new List<Component>(source.GetComponents<Component>());
    //    List<Component> targetComps = new List<Component>(target.GetComponents<Component>());

    //    foreach (Component comp in sourceComps)
    //    {
    //        if (!targetComps.Find(x => x.GetType() == comp.GetType()))
    //        {
    //            //Component c = target.AddComponent(comp.GetType());
    //            //comp.CopyValues();
    //            //c.PasteValues();

    //            Debug.Log(comp);
    //        }
    //    }
    //}

    //public Database bps;

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

    //[ContextMenu("UPDATE")]
    //public void UpdateScale()
    //{
    //    foreach (object obj in bps.Objects.Values)
    //    {
    //        BodyPart bp = obj as BodyPart;
    //        bp.Weight = 1;
    //        EditorUtility.SetDirty(bp);
    //    }
    //}


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