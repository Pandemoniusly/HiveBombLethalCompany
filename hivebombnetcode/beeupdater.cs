using HarmonyLib;
using GameNetcodeStuff;
using RuntimeNetcodeRPCValidator;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;
using LethalNetworkAPI;
using System.Xml.Linq;
using LethalLib.Modules;

namespace hivebombnetcode
{
    [HarmonyPatch(typeof(GrabbableObject))]
    internal class beeupdate
    {
        public static bool first = false;
        private static HiveMindManager bossman;
        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void UpdatePostfix(PlayerControllerB __instance)
        {
            if (!first)
            {
                first = true;
                bossman = ((Component)__instance).GetComponent<HiveMindManager>();
            }
        }
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
            if (GameNetworkManager.Instance.localPlayerController.isHostPlayerObject)
            {
                if (Config.Instance.Enabled.Value == true)
                {

                    if (__instance.name == "RedLocustHive(Clone)")
                    {

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
                                bossman.servertime(__instance.NetworkObject.transform.position.x, __instance.NetworkObject.transform.position.y, __instance.NetworkObject.transform.position.z, rand, knockback, visible, dmg, radius);
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
