using Pinwheel.Poseidon;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MobileOptimizations : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Material[] standardMaterials;
        [SerializeField] private PWaterProfile[] waterProfiles;
        #endregion

        #region Methods
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
            foreach (Material material in standardMaterials)
            {
                material.shader = Shader.Find("Mobile/Diffuse");
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
        #endregion
    }
}