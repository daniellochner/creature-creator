using DanielLochner.Assets.CreatureCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomMapLoadingPass_MiniMap : CustomMapLoadingPass_Base
{
	public override bool Load(Scene scene)
	{
		/*EditorManager.Instance.MiniMap.texture = MapCon
		foreach(var go in scene.GetRootGameObjects())
		{
			if(go.activeSelf)
			{
				foreach(var proxy in go.GetComponentsInChildren<UpgradeCabinetProxy>())
				{
					var instantiatedCabinet = Instantiate(upgradeCabinetPrefab, proxy.transform.position, proxy.transform.rotation, proxy.transform.parent);
					instantiatedCabinet.GetComponent<MyceliumIdentity>().Initialize(MyceliumNetwork.LocalPlayer, proxy.id);
					instantiatedCabinet.SetActive(true);

					Destroy(proxy.gameObject);
				}
			}
		}*/

		return true;
	}
}