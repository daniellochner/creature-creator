using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class IronSourceConfig
{
	private const string unsupportedPlatformStr = "Unsupported Platform";
	private static IronSourceConfig _instance;

	public static IronSourceConfig Instance {
		get {
			if (_instance == null) {
				_instance = new IronSourceConfig ();
			}
			return _instance;
		}
	}


	#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaObject _androidBridge;
	private readonly static string AndroidBridge = "com.ironsource.unity.androidbridge.AndroidBridge";
	
	public IronSourceConfig ()
	{
		using (var pluginClass = new AndroidJavaClass( AndroidBridge ))
			_androidBridge = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
	}

	//Setters	
	public void setLanguage (string language)
	{
		_androidBridge.Call ("setLanguage", language);
	}
	
	public void setClientSideCallbacks (bool status)
	{
		_androidBridge.Call ("setClientSideCallbacks", status);
	}
	
	public void setRewardedVideoCustomParams (Dictionary<string,string> rewardedVideoCustomParams)
	{ 
		string json = IronSourceJSON.Json.Serialize (rewardedVideoCustomParams);
		_androidBridge.Call ("setRewardedVideoCustomParams", json);
	}
	
	public void setOfferwallCustomParams (Dictionary<string,string> offerwallCustomParams)
	{
		string json = IronSourceJSON.Json.Serialize (offerwallCustomParams);
		_androidBridge.Call ("setOfferwallCustomParams", json);
	}


	#elif (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void CFSetLanguage (string language);

	[DllImport("__Internal")]
	private static extern void CFSetClientSideCallbacks (bool useClientSideCallbacks);

	[DllImport("__Internal")]
	private static extern void CFSetRewardedVideoCustomParams (string rewardedVideoCustomParams);

	[DllImport("__Internal")]
	private static extern void CFSetOfferwallCustomParams (string offerwallCustomParams);


	public void setLanguage (string language)
	{
		CFSetLanguage (language);
	}
	
	public void setClientSideCallbacks (bool status)
	{
		CFSetClientSideCallbacks (status);
	}
	
	public void setRewardedVideoCustomParams (Dictionary<string,string> rewardedVideoCustomParams)
	{ 
		string json = IronSourceJSON.Json.Serialize (rewardedVideoCustomParams);
		CFSetRewardedVideoCustomParams (json);
	}
	
	public void setOfferwallCustomParams (Dictionary<string,string> offerwallCustomParams)
	{
		string json = IronSourceJSON.Json.Serialize (offerwallCustomParams);
		CFSetOfferwallCustomParams (json);
	}

	public IronSourceConfig ()
	{
		
	}


	#else		
	public void setLanguage (string language)
	{
		Debug.Log (unsupportedPlatformStr);
	}
	
	public void setClientSideCallbacks (bool status)
	{
		Debug.Log (unsupportedPlatformStr);
	}
	
	public void setRewardedVideoCustomParams (Dictionary<string,string> rewardedVideoCustomParams)
	{ 
		Debug.Log (unsupportedPlatformStr);
	}
	
	public void setOfferwallCustomParams (Dictionary<string,string> offerwallCustomParams)
	{
		Debug.Log (unsupportedPlatformStr);
	}

	public IronSourceConfig ()
	{
		Debug.Log (unsupportedPlatformStr);
	}
	
	#endif
}


