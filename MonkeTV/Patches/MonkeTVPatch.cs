using HarmonyLib;
using MonkeTV.Behaviours;

namespace MonkeTV.Patches
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("Awake")]
    internal class MonkeTVPatch
    {
        public static void Postfix(GorillaLocomotion.Player __instance)
        {
            Plugin.Instance.tClass = __instance.gameObject.AddComponent<TVClass>();
        }
    }
}
