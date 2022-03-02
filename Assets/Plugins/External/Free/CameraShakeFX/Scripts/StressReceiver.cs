using UnityEngine;

public class StressReceiver : MonoBehaviour 
{
    private float _trauma;
    private Vector3 _lastPosition;
    private Vector3 _lastRotation;
    [Tooltip("Exponent for calculating the shake factor. Useful for creating different effect fade outs")]
    public float TraumaExponent = 1;
    [Tooltip("Maximum angle that the gameobject can shake. In euler angles.")]
    public Vector3 MaximumAngularShake = Vector3.one * 5;
    [Tooltip("Maximum translation that the gameobject can receive when applying the shake effect.")]
    public Vector3 MaximumTranslationShake = Vector3.one * .75f;

    private void Update()
    {
        float shake = Mathf.Pow(_trauma, TraumaExponent);
        /* Only apply this when there is active trauma */
        if(shake > 0)
        {
            var previousRotation = _lastRotation;
            var previousPosition = _lastPosition;
            /* In order to avoid affecting the transform current position and rotation each frame we substract the previous translation and rotation */
            _lastPosition = new Vector3(
                MaximumTranslationShake.x * (Mathf.PerlinNoise(0, Time.time * 25) * 2 - 1),
                MaximumTranslationShake.y * (Mathf.PerlinNoise(1, Time.time * 25) * 2 - 1),
                MaximumTranslationShake.z * (Mathf.PerlinNoise(2, Time.time * 25) * 2 - 1)
            ) * shake;

            _lastRotation = new Vector3(
                MaximumAngularShake.x * (Mathf.PerlinNoise(3, Time.time * 25) * 2 - 1),
                MaximumAngularShake.y * (Mathf.PerlinNoise(4, Time.time * 25) * 2 - 1),
                MaximumAngularShake.z * (Mathf.PerlinNoise(5, Time.time * 25) * 2 - 1)
            ) * shake;

            transform.localPosition += _lastPosition - previousPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + _lastRotation - previousRotation);
            _trauma = Mathf.Clamp01(_trauma - Time.deltaTime);
        }
        else
        {
            if (_lastPosition == Vector3.zero && _lastRotation == Vector3.zero) return;
            /* Clear the transform of any left over translation and rotations */
            transform.localPosition -= _lastPosition;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - _lastRotation);
            _lastPosition = Vector3.zero;
            _lastRotation = Vector3.zero;
        }
    }

    /// <summary>
    ///  Applies a stress value to the current object.
    /// </summary>
    /// <param name="Stress">[0,1] Amount of stress to apply to the object</param>
    public void InduceStress(float Stress)
    {
        _trauma = Mathf.Clamp01(_trauma + Stress);
    }
}