using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CustomMapLoadingPass_Base : MonoBehaviour
{
	public abstract bool Load(Scene scene);
}
