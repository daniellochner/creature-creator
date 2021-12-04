using DanielLochner.Assets;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private async void Start()
    {
        Debug.Log("Authenticate");
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Debug.Log("Allocate relay");
        Allocation allocation = await Relay.Instance.CreateAllocationAsync(10);
        FindObjectOfType<UnityTransport>().SetRelayServerData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

        Debug.Log("Generate join code");
        string joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);

        Debug.Log("Create lobby");
        CreateLobbyOptions options = new CreateLobbyOptions()
        {
            IsPrivate = false,
            Player = new Unity.Services.Lobbies.Models.Player(AuthenticationService.Instance.PlayerId, joinCode, null, allocation.AllocationId.ToString())
        };
        await Lobbies.Instance.CreateLobbyAsync("name", 10, options);

        Debug.Log("Start host");
        NetworkManager.Singleton.StartHost();

        StartCoroutine(CountRoutine());
    }

    private IEnumerator CountRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            Task<QueryResponse> task = Lobbies.Instance.QueryLobbiesAsync();
            yield return new WaitUntil(() => task.IsCompleted);
            Debug.Log("Lobbies: " + task.Result.Results.Count);
        }
    }
}