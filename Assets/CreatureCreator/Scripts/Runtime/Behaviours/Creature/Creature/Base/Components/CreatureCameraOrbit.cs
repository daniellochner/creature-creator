using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureCameraOrbit : CameraOrbit
    {
        public override bool CanInput => !CinematicManager.Instance.IsInCinematic && !PauseMenu.Instance.IsOpen && !InputDialog.Instance.IsOpen && !ConfirmationDialog.Instance.IsOpen && !InformationDialog.Instance.IsOpen;
    }
}