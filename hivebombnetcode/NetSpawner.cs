using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using Unity;

namespace hivebombnetcode
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class CheckOwnership
    {
        [HarmonyPatch("ConnectClientToPlayerObject")]
        [HarmonyPostfix]
        static void SpawnNetworkHandler()
        {
            try
            {
                Plugin.checkedClient = true;
                if (((NetworkBehaviour)StartOfRound.Instance).IsHost)
                {
                    Plugin.islobbyHost = true;
                    if (Config.Instance.Debug.Value == true) Plugin.mls.LogInfo("Im hosting");
                }
            }
            catch { }
        }
    }

    [HarmonyPatch(typeof(StartOfRound))]
    public class ResetInfo
    {
        // Name this whatever you like. It needs to be called exactly once, so 
        [HarmonyPatch("OnLocalDisconnect")]
        [HarmonyPostfix]
        public static void ResetKnownInfo()
        {
            hivebombnetcode.Plugin.addedCoroutine = false;
            hivebombnetcode.Plugin.islobbyHost = false;
            hivebombnetcode.Plugin.checkedClient = false;
            if (((Component)RoundManager.Instance).GetComponent<HiveMindManager>()) UnityEngine.Object.Destroy(((Component)RoundManager.Instance).GetComponent<HiveMindManager>());
            if (Config.Instance.Debug.Value == true) Plugin.mls.LogInfo("Reset");
        }
    }
}
