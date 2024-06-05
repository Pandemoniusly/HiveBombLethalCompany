using HarmonyLib;
using System;
using GameNetcodeStuff;
using UnityEngine;
using Unity.Netcode;

namespace hivebombnetcode
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class HiveCreator
    {
        public static NetworkObject TheHiveMindIsReal;
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void beehiveexplode(StartOfRound __instance)
        {
            if (__instance.IsHost)
            {
                GameObject TheHiveMindIsReal = GameObject.Instantiate(Plugin.instance.HiveMindPrefabobj);
                TheHiveMindIsReal.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
