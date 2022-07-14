using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestNB : NetworkBehaviour
{
    public bool test;

    private void Start()
    {
        Debug.Log(IsOwner);
    }

    [ContextMenu("TEST")]
    public void Test()
    {
        TestServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]//
    public void TestServerRpc()
    {
        TestClientRpc();
    }
    [ClientRpc]
    public void TestClientRpc()
    {
        Debug.Log("TEST from " + gameObject.name, gameObject);
    }
}