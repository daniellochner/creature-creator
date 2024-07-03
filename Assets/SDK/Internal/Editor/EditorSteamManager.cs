using Steamworks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorSteamManager
{
	static AppId_t appId = (AppId_t)1990050;

	static bool connected;

	static EditorSteamManager()
	{
		if(!connected)
		{
			ConnectToSteam();
		}
	}

	public static void ConnectToSteam()
	{
		if (SteamAPI.Init())
		{
			if(!SteamUser.BLoggedOn())
			{
				Debug.Log("Steam user not logged on." + "\nMake sure you are logged into Steam and connected to the internet.");
				SteamAPI.Shutdown();
				return;
			}
		}
		else
		{
			Debug.Log("Failed to initialize Steam." + "\nMake sure you are logged into Steam and connected to the internet.");
			return;
		}

		SteamNetworkingUtils.InitRelayNetworkAccess();
		Debug.Log("Successfully connected to Steam");

		connected = true;
	}

	public static string GetInstallFolder()
	{
		if(!connected)
		{
			Debug.LogError("Steam is not connected.");
			throw new System.Exception("Steam is not connected.");
		}
		SteamApps.GetAppInstallDir(appId, out string folder, 1024 * 16);
		return folder;
	}

	public static string GetInstallLocation()
	{
		return GetInstallFolder() + "/Creature Creator.exe";
	}
}