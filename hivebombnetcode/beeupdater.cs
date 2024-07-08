using HarmonyLib;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;
using System.Collections;
using UnityEngine.XR;
using UnityEngine.PlayerLoop;

namespace hivebombnetcode
{
    public class SyncConfigs : MonoBehaviour
    {
        public static Vector3 where = new Vector3();
        public static int rand = 0;

        public void Update()
        {
            if (hivebombnetcode.Plugin.PauseUntilCoroutine)
            {
                StartCoroutine(SendOnceSynced(hivebombnetcode.Plugin.currentObj));
                hivebombnetcode.Plugin.PauseUntilCoroutine = false;
            }
        }
        public IEnumerator SendOnceSynced(GrabbableObject __instance)
        {
            if (Config.Instance.Debug.Value == true) hivebombnetcode.Plugin.mls.LogInfo("Sending ping");
            ((Component)RoundManager.Instance).GetComponent<HiveMindManager>().ConfigPingServerRpc(hivebombnetcode.Config.Instance.KnockbackEnabled.Value, hivebombnetcode.Config.Instance.VisibleExplosions.Value, hivebombnetcode.Config.Instance.Radius.Value, hivebombnetcode.Config.Instance.MaxPlayerDamage.Value);

            yield return new WaitUntil(() => ((hivebombnetcode.Plugin.knockback == Config.Instance.KnockbackEnabled.Value) & (hivebombnetcode.Plugin.visible == Config.Instance.VisibleExplosions.Value) & (hivebombnetcode.Plugin.radius == Config.Instance.Radius.Value) & (hivebombnetcode.Plugin.maxdmg == Config.Instance.MaxPlayerDamage.Value)));
            if (Config.Instance.Debug.Value == true) hivebombnetcode.Plugin.mls.LogInfo("Configs updated");
            yield return new WaitForSeconds(0.025f); // hopefully helps guarantee on clients that the config update gets sent before the explosion rpc

            rand = hivebombnetcode.Plugin.getrandom.Next(50);
            where = __instance.itemProperties.positionOffset;
            ((Component)RoundManager.Instance).GetComponent<HiveMindManager>().ExplodePingServerRpc(__instance.NetworkObject.transform.position.x + where.x, __instance.NetworkObject.transform.position.y + where.y, __instance.NetworkObject.transform.position.z + where.z, rand);
            if (Config.Instance.Debug.Value == true) hivebombnetcode.Plugin.mls.LogInfo("Exploding in coroutine");
        }
    }

    [HarmonyPatch(typeof(GrabbableObject))]
    internal class beeupdate
    {
//        public static bool first = false;
//        private static HiveMindManager bossman;
//        [HarmonyPatch(typeof(RoundManager), "Update")]
//        [HarmonyPostfix]
//        private static void UpdatePostfix(RoundManager __instance)
 //       {
 //           if (!first)
 //           {
 //               first = true;
 //               bossman = ((Component)__instance).GetComponent<HiveMindManager>();
 //           }
 //       }
        [SerializeField]
        public static bool knockback = false;
        [SerializeField]
        public static bool visible = false;
        [SerializeField]
        public static Vector3 where = new Vector3();
        [SerializeField]
        public static int rand = 0;
        [SerializeField]
        public static int dmg = 0;
        [SerializeField]
        public static float radius = 0;

        public static int Framecount = 0;

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void beehiveexplode(GrabbableObject __instance)
        {
            if (((NetworkBehaviour)RoundManager.Instance).IsHost)
            {
                if (!hivebombnetcode.Plugin.addedCoroutine)
                {
                    RoundManager.Instance.gameObject.AddComponent<SyncConfigs>();
                    hivebombnetcode.Plugin.addedCoroutine = true;
                }
                if ((Config.Instance.Enabled.Value == true) & (hivebombnetcode.Plugin.PauseUntilCoroutine == false))
                {
                    if (__instance.name == "RedLocustHive(Clone)")
                    {
                        //hivebombnetcode.Plugin.mls.LogInfo("IM SIGMA");
                        //finalhivebomb.Logger.LogInfo("found");
                        if (Framecount <= 0)
                        {
                            Framecount = Config.Instance.GlobalExplosionCooldown.Value;
                            if ((hivebombnetcode.Plugin.getrandom.Next((int)(800*Config.Instance.RandomnessMult.Value)) <= hivebombnetcode.Plugin.getrandom.Next(10)))
                            {
                                if ((hivebombnetcode.Plugin.knockback != Config.Instance.KnockbackEnabled.Value) || (hivebombnetcode.Plugin.visible != Config.Instance.VisibleExplosions.Value) || (hivebombnetcode.Plugin.radius != Config.Instance.Radius.Value) || (hivebombnetcode.Plugin.maxdmg != Config.Instance.MaxPlayerDamage.Value))
                                {
                                    if (Config.Instance.Debug.Value == true) hivebombnetcode.Plugin.mls.LogInfo("Trying to sync configs");
                                    hivebombnetcode.Plugin.currentObj = __instance;
                                    hivebombnetcode.Plugin.PauseUntilCoroutine = true;
                                }
                                else
                                {
                                    rand = hivebombnetcode.Plugin.getrandom.Next(50);
                                    where = __instance.itemProperties.positionOffset;
                                    ((Component)RoundManager.Instance).GetComponent<HiveMindManager>().ExplodePingServerRpc(__instance.NetworkObject.transform.position.x + where.x, __instance.NetworkObject.transform.position.y + where.y, __instance.NetworkObject.transform.position.z + where.z, rand);
                                    if (Config.Instance.Debug.Value == true) Plugin.mls.LogInfo("Exploding");
                                }
                            }
                        }
                        else if (Framecount > 0)
                        {
                            Framecount -= 1;
                        }
                    }
                }
            }
        }
    }
}
