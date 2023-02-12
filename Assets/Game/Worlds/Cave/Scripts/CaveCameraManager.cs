using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CaveCameraManager : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(GameSetup.Instance);
            yield return new WaitUntilSetup(Player.Instance.Camera);

            Player.Instance.Camera.MainCamera.clearFlags = CameraClearFlags.Color;
        }
    }
}