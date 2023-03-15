using DanielLochner.Assets;
using Pinwheel.Poseidon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileOptimizations : MonoBehaviour
{
    [SerializeField] private PWaterProfile[] waterProfiles;

    [SerializeField] private Material[] basicMaterials;

    private void Start()
    {
        if (SystemUtility.IsDevice(DeviceType.Handheld))
        {
            OptimizeStandardMaterials();
            OptimizeWater();

        }

       


    }

    private void OptimizeStandardMaterials()
    {
        foreach (Material material in basicMaterials)
        {
            material.shader = (SystemUtility.DeviceType == DeviceType.Handheld) ? Shader.Find("Mobile/Diffuse") : Shader.Find("Standard");
        }
    }
    private void OptimizeWater()
    {
        foreach (PWaterProfile waterProfile in waterProfiles)
        {
            waterProfile.LightingModel = PLightingModel.BlinnPhong;
            waterProfile.EnableFoamHQ = false;
        }
    }

}
