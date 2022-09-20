using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Pinwheel.Poseidon
{
    [ExecuteInEditMode]
    public class LightController : MonoBehaviour
    {
        public Light lightComponent;
        public Rect moveArea;
        public float moveSpeed = 10;
        public float hueSpeed = 10;
        public float saturationSpeed = 20;
        public float valueSpeed = 30;
        public float intensitySpeed = 10;
        public float baseIntensity = 3;
        public float yPos = 3;

        private Vector3 actualTarget;
        private Vector3 lookAtTarget;

        private float hue;
        private float saturation;
        private float value;
        private float intensity;

        private int frameCount = 0;

        private const float DELTA_TIME = 0.0167f;

        private void OnEnable()
        {
            hue = Random.value * 360;
            saturation = Random.value * 100;
            value = Random.value * 100;
            intensity = Random.value;

            frameCount = 0;
            SampleActualTarget();

            Camera.onPreCull += OnPreCullCamera;

#if POSEIDON_URP
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRenderingSRP;
#endif
        }

        private void OnDisable()
        {
            Camera.onPreCull -= OnPreCullCamera;

#if POSEIDON_URP
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRenderingSRP;
#endif
        }

        public void OnPreCullCamera(Camera cam)
        {
            Animate();
        }

        private void OnBeginCameraRenderingSRP(ScriptableRenderContext context, Camera cam)
        {
            Animate();
        }

        private void Animate()
        {
            if (!moveArea.Contains(new Vector2(actualTarget.x, actualTarget.y)) ||
                IsCloseEnoughToTarget())
            {
                SampleActualTarget();
            }

            frameCount += 1;
            if (frameCount >= 2)
            {
                SampleLookatTarget();
                frameCount = 0;
            }

            Vector3 dir = (lookAtTarget - transform.position).normalized;
            Vector3 forward = Vector3.MoveTowards(transform.forward, dir, 3 * DELTA_TIME);
            transform.forward = forward;
            transform.position += forward * moveSpeed * DELTA_TIME;

            if (lightComponent != null)
            {
                hue += hueSpeed * DELTA_TIME;
                saturation += saturationSpeed * DELTA_TIME;
                value += valueSpeed * DELTA_TIME;
                intensity += intensitySpeed * DELTA_TIME;

                Color c = Color.HSVToRGB(
                    Mathf.Sin(hue * Mathf.Deg2Rad) * 0.5f + 0.5f,
                    Mathf.Sin(saturation * Mathf.Deg2Rad) * 0.5f + 0.5f,
                    Mathf.Sin(value * Mathf.Deg2Rad) * 0.5f + 0.5f);
                lightComponent.color = c;
                lightComponent.intensity = baseIntensity * (Mathf.Sin(intensity * Mathf.Deg2Rad) * 0.5f + 0.5f);
            }
        }

        private bool IsCloseEnoughToTarget()
        {
            float threshold = 10;
            return Vector3.Distance(transform.position, actualTarget) <= threshold;
        }

        private void SampleActualTarget()
        {
            float randX = Random.value;
            float randY = Random.value;

            actualTarget = new Vector3(
                Mathf.Lerp(moveArea.min.x, moveArea.max.x, randX),
                yPos,
                Mathf.Lerp(moveArea.min.y, moveArea.max.y, randY));
        }

        private void SampleLookatTarget()
        {
            Vector3 p = Random.insideUnitSphere * 50;
            p.y = 0;
            lookAtTarget = actualTarget + p;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(actualTarget, Vector3.one);
            Gizmos.DrawRay(transform.position, transform.forward);
        }
    }
}
