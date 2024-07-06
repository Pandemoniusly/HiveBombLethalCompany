using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.IO;
using GameNetcodeStuff;
using UnityEngine;
using UnityEngine.Assertions;

namespace hivebombnetcode
{
    [BepInPlugin("Pandemonius.BeehiveBomb", "BeehiveBomb", "2.0.0")]
    //[BepInDependency(RuntimeNetcodeRPCValidator.MyPluginInfo.PLUGIN_GUID, RuntimeNetcodeRPCValidator.MyPluginInfo.PLUGIN_VERSION)]
    //    [BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {

        private readonly Harmony harmony = new Harmony("Pandemonius.BeehiveBomb");

        static internal Plugin instance;

        static internal ManualLogSource mls;

        //private NetcodeValidator netcodebullshitgo;

        public static ConfigFile BepInExConfig()
        {
            return instance.Config;
        }

        //public static AssetBundle bundle;

        //public GameObject HiveMindPrefabobj;

        public GameObject TheHiveMindIsReal;
        public bool isClient = false;
        public bool checkedClient = false;
        public bool enabled = true;
        public bool knockback = true;
        public bool visible = true;
        public float radius = 0f;
        public float randomness = 0f;
        public int maxdmg = 0;
        public int cooldown = 0;

        public void Awake()
        {
            // entry point when mod load
            instance = this;

            //string assetlocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hivebombing");
            //bundle = AssetBundle.LoadFromFile(assetlocation);
            //HiveMindPrefabobj = bundle.LoadAsset<GameObject>("Assets/TheHiveMind.prefab");

            mls = BepInEx.Logging.Logger.CreateLogSource("Pandemonius.BeehiveBomb");
            hivebombnetcode.Config.Instance.Setup();
            mls.LogMessage("Welcome to the HiveMind");
            harmony.PatchAll(typeof(beeupdate));
            harmony.PatchAll(typeof(NetSpawner));

            //netcodebullshitgo = new NetcodeValidator("Pandemonius.BeehiveBomb");
            //netcodebullshitgo.PatchAll();
            //netcodebullshitgo.BindToPreExistingObjectByBehaviour<HiveMindManager, Terminal>();
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
