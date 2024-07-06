using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace hivebombnetcode
{
    [HarmonyPatch]
    public class NetSpawner
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Start))]
        public static void Init()
        {
            if (networkPrefab != null)
                return;

            string assetlocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hivebombing");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetlocation);
            networkPrefab = bundle.LoadAsset<GameObject>("Assets/TheHiveMind.prefab");
            networkPrefab.AddComponent<HiveMindManager>();

            NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake))]
        static void SpawnNetworkHandler()
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                var networkHandlerHost = UnityEngine.Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
                networkHandlerHost.GetComponent<NetworkObject>().Spawn();
            }
        }

        static GameObject networkPrefab;
    }
}
