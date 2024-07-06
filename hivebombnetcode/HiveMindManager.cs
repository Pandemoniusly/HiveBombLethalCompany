using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using System.Collections;

namespace hivebombnetcode
{
    public class HiveMindManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {

            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
                Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
            Instance = this;

            base.OnNetworkSpawn();
        }

        [ServerRpc]
        public void ExplodeAtServerRpc(float x, float y, float z, float rand, bool knockback, bool visible, int dmg, float rad, ServerRpcParams rpcParams = default)
        {
                ExplodeAtClientRpc(x,y,z, rand, knockback, visible, dmg, rad);
        }

        [ClientRpc]
        public void ExplodeAtClientRpc(float x, float y, float z, float rand, bool knockback, bool visible, int dmg, float rad)
        {
                Landmine.SpawnExplosion(new Vector3(x,y,z), visible, 0, rad, dmg, knockback ? rand : 0);
        }
        public static HiveMindManager Instance { get; private set; }
    }
}

