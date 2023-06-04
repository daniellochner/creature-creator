using Pinwheel.Poseidon;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Pool : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Ability swimAbility;

        [SerializeField] private PWater water;
        [SerializeField] private PWaterEmissionSetter waterEmission;

        [SerializeField] private GameObject toxic;
        [SerializeField] private GameObject healPrefab;
        [SerializeField] private GameObject splashPrefab;

        [SerializeField] private Color healColour;
        [SerializeField] private Color acidColour;
        [SerializeField] private float damageCooldown;
        [SerializeField] private float damageAmount;
        [SerializeField] private float acidifyTime;

        private float damageTimeLeft;
        #endregion

        #region Properties
        public NetworkVariable<bool> IsAcidic { get; set; } = new NetworkVariable<bool>(false);
        #endregion

        #region Methods
        private void Start()
        {
            SetColour(IsAcidic.Value ? acidColour : healColour);
            toxic.SetActive(IsAcidic.Value);
        }
        public override void OnDestroy()
        {
            SetColour(healColour);
            base.OnDestroy();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local"))
            {
                CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
                if (player.Abilities.Abilities.Contains(swimAbility))
                {
                    if (!IsAcidic.Value)
                    {
                        Instantiate(healPrefab, player.Constructor.Body, false);
                        player.Health.HealthPercentage = 1f;
                    }
                }
            }
            else
            if (other.CompareTag("Toxic"))
            {
                Acidify();
                Instantiate(splashPrefab, other.transform.position, Quaternion.identity);
            }
        }
        public void OnTriggerStay(Collider other)
        {
            if (IsAcidic.Value && other.CompareTag("Player/Local"))
            {
                TimerUtility.OnTimer(ref damageTimeLeft, damageCooldown, Time.fixedDeltaTime, delegate
                {
                    other.GetComponent<CreaturePlayerLocal>().Health.TakeDamage(damageAmount, reason: DamageReason.Acid);
                });
            }
        }

        private void Acidify()
        {
            if (!IsAcidic.Value)
            {
                AcidifyClientRpc();
                IsAcidic.Value = true;
            }
        }
        [ClientRpc]
        private void AcidifyClientRpc()
        {
            toxic.SetActive(true);

            this.InvokeOverTime(delegate (float p)
            {
                SetColour(Color.Lerp(healColour, acidColour, p));
            },
            acidifyTime);
        }

        private void SetColour(Color colour)
        {
            water.Profile.Color = water.Profile.DepthColor = colour;
            waterEmission.emission = colour;
        }
        #endregion
    }
}