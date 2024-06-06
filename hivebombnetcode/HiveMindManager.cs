using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

namespace hivebombnetcode
{
    public class HiveMindManager : NetworkBehaviour
    {

        public void servertime(float x, float y, float z, float rand, bool knockback, bool visible, int dmg, float rad)
        {
            ExplodeAtClientRpc(x,y,z, rand, knockback, visible, dmg, rad);
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
    }
}

