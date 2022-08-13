using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureVelocity : KinematicVelocity
    {
        [SerializeField] private float baseMovementSpeed;
        [SerializeField] private float baseTurnSpeed;

        public CreatureConstructor Constructor { get; private set; }
        public Animator Animator { get; private set; }

        public float LSpeedPercentage => Animator.GetFloat("%LSpeed");
        public float ASpeedPercentage => Animator.GetFloat("%ASpeed");

        private float MoveSpeed
        {
            get => baseMovementSpeed * Constructor.Statistics.Speed;
        }
        private float TurnSpeed
        {
            get => baseTurnSpeed;
        }

        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
            Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            OnLinearChanged += delegate (Vector3 linear)
            {
                float l = Mathf.Clamp01(Vector3.ProjectOnPlane(Linear, transform.up).magnitude / MoveSpeed);
                Animator.SetFloat("%LSpeed", l);
            };
            OnAngularChanged += delegate (Vector3 angular)
            {
                float a = Mathf.Clamp01(Mathf.Abs(Angular.y) / TurnSpeed);
                Animator.SetFloat("%ASpeed", a);
            };
        }
    }
}