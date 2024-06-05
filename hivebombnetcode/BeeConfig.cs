using BepInEx.Configuration;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Collections;
using Unity.Netcode;
using static Unity.Netcode.CustomMessagingManager;

namespace hivebombnetcode
{
    public sealed class Config
    {
        #region Properties

        public new ConfigEntry<bool> Enabled { get; set; }
        public new ConfigEntry<bool> KnockbackEnabled { get; set; }
        public new ConfigEntry<bool> VisibleExplosions { get; set; }
        public ConfigEntry<float> Radius { get; set; }

        public ConfigEntry<float> RandomnessMult { get; set; }
        public ConfigEntry<int> MaxPlayerDamage { get; set; }
        public ConfigEntry<int> GlobalExplosionCooldown { get; set; }

        private static Config instance = null;
        public static Config Instance
        {
            get
            {
                if (instance == null)
                    instance = new Config();

                return instance;
            }
        }
        #endregion

        public void Setup()
        {
            Enabled = hivebombnetcode.Plugin.BepInExConfig().Bind("General", "Enabled", true, "Enables access to track your social security, banking info, home address and DNA samples, Default: On");
            KnockbackEnabled = hivebombnetcode.Plugin.BepInExConfig().Bind("General", "Masochist Mode", true, "Enables Knockback, Default: On");
            VisibleExplosions = hivebombnetcode.Plugin.BepInExConfig().Bind("General", "Explosion Visibility", true, "Turns the explosion particles On or Off, Default: On");
            MaxPlayerDamage = hivebombnetcode.Plugin.BepInExConfig().Bind("General", "Player Damage", 0, new ConfigDescription("Changes how many treats are gonna be delivered to your house for being such a good kid!!!", new AcceptableValueRange<int>(0, 100)));
            GlobalExplosionCooldown = hivebombnetcode.Plugin.BepInExConfig().Bind("General", "Global Explosion Cooldown", 10, new ConfigDescription("Changes the minimum frames between a beehive exploding after another one already has", new AcceptableValueRange<int>(1, 1000000000)));
            Radius = hivebombnetcode.Plugin.BepInExConfig().Bind("General", "Radius", 2.4f, new ConfigDescription("Explosion radius, Default is 2.4 and is as big as a lightning strikes kill range", new AcceptableValueRange<float>(0.1f, 10f)));
            RandomnessMult = hivebombnetcode.Plugin.BepInExConfig().Bind("General", "Randomness Multiplier", 1f, new ConfigDescription("Markiplies the chance it has to explode each frame, 10 is extremely unlikely and 0.1 is extremely likely", new AcceptableValueRange<float>(0.1f, 10f)));
        }
    }
}
