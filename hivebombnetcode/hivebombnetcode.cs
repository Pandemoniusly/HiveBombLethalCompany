using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace hivebombnetcode
{
    [BepInPlugin(modGUID, modName, modVersion)]
//    [BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string modGUID = "Pandemonius.BeehiveBomb";
        public const string modName = "BeehiveBomb";
        public const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static Plugin instance;

        static internal ManualLogSource mls;

        static internal ExplosionHandler explosionHandler = new ExplosionHandler();
        public static ConfigFile BepInExConfig() { return instance.Config; }
        static internal AssetBundle MainAssetBundle = null;
        public void Awake()
        {
            // entry point when mod load
            if (instance == null)
                instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            hivebombnetcode.Config.Instance.Setup();
            mls.LogMessage("Plugin " + modName + " loaded!");
            harmony.PatchAll(typeof(beeupdate));
        }
    }

  //  [HarmonyPatch]
  //  public class NetworkObjectManager
  //  {
  //
  //      [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Start))]
  //      public static void Init()
  //      {
  //          if (networkPrefab != null)
   //             return;
  //          
  //          networkPrefab = (GameObject)MainAssetBundle.LoadAsset("ExampleNetworkHandler");
  //      }
  //
  //      static GameObject networkPrefab;
  //  }
}