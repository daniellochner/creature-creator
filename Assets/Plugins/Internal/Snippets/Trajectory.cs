using UnityEngine;

namespace DanielLochner.Assets
{
    public class Trajectory : MonoBehaviour
    {
        #region Fields
        [SerializeField] private LayerMask mask;
        [SerializeField, Range(10, 100)] private int linePoints = 25;
        [SerializeField, Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;

        private LineRenderer lr;
        private float speed, mass;
        #endregion

        #region Methods
        private void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }
        private void OnEnable()
        {
            DrawProjection(speed, mass);
            lr.enabled = true;
        }
        private void OnDisable()
        {
            lr.enabled = false;
        }
        private void Update()
        {
            DrawProjection(speed, mass);
        }

        public void Setup(float speed, float mass)
        {
            this.speed = speed;
            this.mass = mass;
        }
        private void DrawProjection(float speed, float mass)
        {
            lr.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;

            Vector3 startPos = transform.position;
            Vector3 startVel = transform.forward * speed / mass;

            int i = 0;
            lr.SetPosition(i, startPos);
            for (float time = 0; time < linePoints; time += timeBetweenPoints)
            {
                i++;

                Vector3 point = startPos + time * startVel;
                point.y = startPos.y + startVel.y * time + (Physics.gravity.y / 2f * time * time);

                lr.SetPosition(i, point);

                Vector3 lastPos = lr.GetPosition(i - 1);

                if (Physics.Raycast(lastPos, (point - lastPos).normalized, out RaycastHit hitInfo, (point - lastPos).magnitude, mask))
                {
                    lr.SetPosition(i, hitInfo.point);
                    lr.positionCount = i + 1;
                    break;
                }
            }
        }
        #endregion
    }
}