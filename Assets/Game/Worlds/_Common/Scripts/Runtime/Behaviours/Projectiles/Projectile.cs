using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Projectile : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private MinMax minMaxDamage;
        private SelfDestructor selfDestructor;
        #endregion

        #region Methods
        private void Awake()
        {
            selfDestructor = GetComponent<SelfDestructor>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (IsServer)
            {
                foreach (var contactPoint in collision.contacts)
                {
                    CreatureBase creature = contactPoint.otherCollider.GetComponent<CreatureBase>();
                    if (creature != null && creature != Player.Instance)
                    {
                        if (!((creature is CreaturePlayerRemote) && !(WorldManager.Instance.World as WorldMP).EnablePVP))
                        {
                            float damage = minMaxDamage.Random;
                            creature.Health.TakeDamage(damage);

                            if (creature.Health.Health - damage <= 0)
                            {
                                KillClientRpc(NetworkUtils.SendTo(OwnerClientId));
                            }

                            break;
                        }
                    }
                }
                if (NetworkObject.IsSpawned)
                {
                    NetworkObject.Despawn();
                }
            }
        }

        [ClientRpc]
        private void KillClientRpc(ClientRpcParams clientRpcParams = default)
        {
#if USE_STATS
            StatsManager.Instance.Kills++;
#endif
        }
        #endregion
    }
}