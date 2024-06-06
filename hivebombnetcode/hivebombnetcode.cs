using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.IO;
using GameNetcodeStuff;
using UnityEngine;
using UnityEngine.Assertions;
using RuntimeNetcodeRPCValidator;

namespace hivebombnetcode
{
    [BepInPlugin("Pandemonius.BeehiveBomb", "BeehiveBomb", "2.0.0")]
    [BepInDependency(RuntimeNetcodeRPCValidator.MyPluginInfo.PLUGIN_GUID, RuntimeNetcodeRPCValidator.MyPluginInfo.PLUGIN_VERSION)]
    //    [BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {

        private readonly Harmony harmony = new Harmony("Pandemonius.BeehiveBomb");

        static internal Plugin instance;

        static internal ManualLogSource mls;

        private NetcodeValidator netcodebullshitgo;

        public static ConfigFile BepInExConfig()
        {
            return instance.Config;
        }

        public static AssetBundle bundle;

        public GameObject HiveMindPrefabobj;

        public GameObject TheHiveMindIsReal;

        public void Awake()
        {
            // entry point when mod load
            instance = this;

            string assetlocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hivebombing");
            bundle = AssetBundle.LoadFromFile(assetlocation);

            HiveMindPrefabobj = bundle.LoadAsset<GameObject>("Assets/TheHiveMind.prefab");
            HiveMindPrefabobj.AddComponent<HiveMindManager>();

            mls = BepInEx.Logging.Logger.CreateLogSource("Pandemonius.BeehiveBomb");
            hivebombnetcode.Config.Instance.Setup();
            mls.LogMessage("Welcome to the HiveMind");
            harmony.PatchAll(typeof(beeupdate));
            netcodebullshitgo = new NetcodeValidator("Pandemonius.BeehiveBomb");
            netcodebullshitgo.PatchAll();
            netcodebullshitgo.BindToPreExistingObjectByBehaviour<HiveMindManager, PlayerControllerB>();
            //harmony.PatchAll(typeof(HiveCreator));
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
}
