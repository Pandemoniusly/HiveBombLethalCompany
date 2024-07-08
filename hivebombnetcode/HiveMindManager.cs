using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using System.Collections;

namespace hivebombnetcode
{
    public class HiveMindManager : NetworkBehaviour
    {
        [ServerRpc(RequireOwnership = false)]
        public void PingServerRpc(ServerRpcParams rpcParams = default)
        {
            PongClientRpc();
        }

        [ClientRpc]
        public void PongClientRpc()
        {
            if (Plugin.islobbyHost) ConfigPingServerRpc(Config.Instance.KnockbackEnabled.Value, Config.Instance.VisibleExplosions.Value, Config.Instance.Radius.Value, Config.Instance.MaxPlayerDamage.Value);
        }

        [ServerRpc]
        public void ConfigPingServerRpc(bool knock, bool visible, float radius, int dmg, ServerRpcParams rpcParams = default)
        {
            ConfigPongClientRpc(knock, visible, radius, dmg);
        }

        [ClientRpc]
        public void ConfigPongClientRpc(bool knock, bool visible, float radius, int dmg)
        {
            hivebombnetcode.Plugin.knockback = knock;
            hivebombnetcode.Plugin.visible = visible;
            hivebombnetcode.Plugin.radius = radius;
            hivebombnetcode.Plugin.maxdmg = dmg;
        }

        [ServerRpc]
        public void ExplodePingServerRpc(float x, float y, float z, int rand, ServerRpcParams rpcParams = default)
        {
                ExplodePongClientRpc(x,y,z,rand);
        }

        [ClientRpc]
        public void ExplodePongClientRpc(float x, float y, float z, int rand)
        {
            try
            {
                Landmine.SpawnExplosion(new Vector3(x, y, z), hivebombnetcode.Plugin.visible, 0, hivebombnetcode.Plugin.radius, hivebombnetcode.Plugin.maxdmg, hivebombnetcode.Plugin.knockback ? rand : 0);
            }
            catch { }
        }
        private void Awake()
        {
            if (!IsHost)
                StartCoroutine(WaitForSomeTime());
        }

        private IEnumerator WaitForSomeTime()
        {
            yield return new WaitUntil(() => NetworkObject.IsSpawned);

            PingServerRpc();
        }

        public static HiveMindManager Instance { get; private set; }
    }
}

