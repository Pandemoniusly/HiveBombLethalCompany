using HarmonyLib;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

namespace hivebombnetcode
{
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
        public static readonly Random getrandom = new Random();

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void beehiveexplode(GrabbableObject __instance)
        {
            if ((GameNetworkManager.Instance.localPlayerController == RoundManager.Instance.playersManager.allPlayerObjects[0].GetComponentInChildren<PlayerControllerB>()) & ((NetworkBehaviour)RoundManager.Instance).IsHost)
            {
                if (Config.Instance.Enabled.Value == true)
                {
                    if (__instance.name == "RedLocustHive(Clone)")
                    {
                        //hivebombnetcode.Plugin.mls.LogInfo("IM SIGMA");
                        //finalhivebomb.Logger.LogInfo("found");
                        if (Framecount <= 0)
                        {
                            Framecount = Config.Instance.GlobalExplosionCooldown.Value;
                            if ((getrandom.Next((int)(800*Config.Instance.RandomnessMult.Value)) <= getrandom.Next(10)))
                            {
                                knockback = Config.Instance.KnockbackEnabled.Value;
                                visible = Config.Instance.VisibleExplosions.Value;
                                dmg = Config.Instance.MaxPlayerDamage.Value;
                                radius = Config.Instance.Radius.Value;
                                rand = getrandom.Next(50);
                                where = __instance.itemProperties.positionOffset;
                                HiveMindManager.Instance.ExplodeAtServerRpc(__instance.NetworkObject.transform.position.x, __instance.NetworkObject.transform.position.y, __instance.NetworkObject.transform.position.z, rand, knockback, visible, dmg, radius);
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
