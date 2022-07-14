using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnMe : MonoBehaviour
{
    [ContextMenu("SPAWN ME")]
    public void SpawnMeLOL()
    {
        GameObject ob = Instantiate(gameObject);
        ob.GetComponent<NetworkObject>().Spawn();
    }
}
