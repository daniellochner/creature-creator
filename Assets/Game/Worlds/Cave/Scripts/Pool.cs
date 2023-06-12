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
        [Space]
        [SerializeField] private PWater water;
        [SerializeField] private PWaterEmissionSetter waterEmission;
        [Space]
        [SerializeField] private GameObject fumes;
        [SerializeField] private GameObject healPrefab;
        [SerializeField] private GameObject splashPrefab;
        [Space]
        [SerializeField] private bool isHealing;
        [SerializeField] private Color defaultColour;
        [SerializeField] private Color toxicColour;
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
            SetColour(IsAcidic.Value ? toxicColour : defaultColour);
            fumes.SetActive(IsAcidic.Value);
        }
        public override void OnDestroy()
        {
            SetColour(defaultColour);
            base.OnDestroy();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player/Local") && isHealing && !IsAcidic.Value)
            {
                CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
                if (player.Abilities.Abilities.Contains(swimAbility))
                {
                    Instantiate(healPrefab, player.Constructor.Body, false);
                    player.Health.HealthPercentage = 1f;
                }
            }
            else
            if (other.CompareTag("Toxic"))
            {
                if (IsServer)
                {
                    Acidify();
                }
                Instantiate(splashPrefab, other.transform.position, Quaternion.identity);
            }
        }
        public void OnTriggerStay(Collider other)
        {
            if (IsServer)
            {
                CreatureBase creature = other.GetComponent<CreatureBase>();
                if (creature != null && IsAcidic.Value)
                {
                    TimerUtility.OnTimer(ref damageTimeLeft, damageCooldown, Time.fixedDeltaTime, delegate
                    {
                        creature.Health.TakeDamage(damageAmount, reason: DamageReason.Acid);
                    });
                }
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
            fumes.SetActive(true);

            this.InvokeOverTime(delegate (float p)
            {
                SetColour(Color.Lerp(defaultColour, toxicColour, p));
            },
            acidifyTime);
        }

        private void SetColour(Color colour)
        {
            water.Profile.Color = water.Profile.DepthColor = colour;
            if (waterEmission != null)
            {
                waterEmission.emission = colour;
            }
        }
        #endregion
    }
}