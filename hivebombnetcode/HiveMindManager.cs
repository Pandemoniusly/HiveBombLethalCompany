using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

namespace hivebombnetcode
{
    struct MyComplexStruct : INetworkSerializable
    {
        public Vector3 Position;

        // INetworkSerializable
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Position);
        }
        // ~INetworkSerializable
    }
    public class HiveMindManager : NetworkBehaviour
    {
        private PlayerControllerB victim;

        private void Start()
        {
            victim = ((Component)this).GetComponent<PlayerControllerB>();
        }

        public void servertime(float x, float y, float z, float rand, bool knockback, bool visible, int dmg, float rad)
        {
            ExplodeAtServerRpc(x,y,z, rand, knockback, visible, dmg, rad);
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

