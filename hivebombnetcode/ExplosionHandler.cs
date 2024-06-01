using UnityEngine;

namespace hivebombnetcode
{
    internal class ExplosionHandler
    {
        public static void ExplodeAt(Transform __transform,int rand)
        {
            if (Config.Instance.Enabled.Value == true)
            {
                if (Config.Instance.KnockbackEnabled.Value == true)
                {
                    Landmine.SpawnExplosion(__transform.position, Config.Instance.VisibleExplosions.Value, 0f, Config.Instance.Radius.Value, Config.Instance.MaxPlayerDamage.Value, rand);
                }
                else
                {
                    Landmine.SpawnExplosion(__transform.position, Config.Instance.VisibleExplosions.Value, 0f, Config.Instance.Radius.Value, Config.Instance.MaxPlayerDamage.Value, 0);
                }
            }
        }
    }
}