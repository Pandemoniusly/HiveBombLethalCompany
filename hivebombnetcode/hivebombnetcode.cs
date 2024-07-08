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
using System.Collections.Generic;
using Random = System.Random;

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

        private NetcodeValidator Validator;

        public static ConfigFile BepInExConfig()
        {
            return instance.Config;
        }

        //public static AssetBundle bundle;

        //public GameObject HiveMindPrefabobj;

        public GameObject TheHiveMindIsReal;
        public static bool islobbyHost = false;
        public static bool checkedClient = false;
        public static bool knockback = true;
        public static bool visible = true;
        public static float radius = 0f;
        public static int maxdmg = 0;
        public float updatetimer = 0;
        public static bool addedCoroutine = false;
        public static readonly Random getrandom = new Random();
        public static bool PauseUntilCoroutine = false;
        public static GrabbableObject currentObj;
        public void Awake()
        {
            // entry point when mod load
            instance = this;

            mls = BepInEx.Logging.Logger.CreateLogSource("Pandemonius.BeehiveBomb");
            hivebombnetcode.Config.Instance.Setup();
            harmony.PatchAll(typeof(beeupdate));
            harmony.PatchAll(typeof(CheckOwnership));
            harmony.PatchAll(typeof(ResetInfo));

            Validator = new NetcodeValidator("Pandemonius.BeehiveBomb");
            Validator.PatchAll();

            Validator.BindToPreExistingObjectByBehaviour<HiveMindManager, RoundManager>();
            mls.LogMessage("Welcome to the HiveMind");
        }

        //public void Update()
        //{
        //    if ((RoundManager.Instance == null) || (islobbyHost == false) || (checkedClient == false)) return;
        //    updatetimer += Time.deltaTime;
        //    if (updatetimer >= 10)
        //    {
        //        hivebombnetcode.Plugin.mls.LogInfo("Updating");
        //        ((Component)RoundManager.Instance).GetComponent<HiveMindManager>().ConfigPingServerRpc(hivebombnetcode.Config.Instance.KnockbackEnabled.Value, hivebombnetcode.Config.Instance.VisibleExplosions.Value, hivebombnetcode.Config.Instance.Radius.Value, hivebombnetcode.Config.Instance.MaxPlayerDamage.Value);
        //        updatetimer = 0;
        //    }
        //}
    }
}
