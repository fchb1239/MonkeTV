using HarmonyLib;

namespace MonkeTV.Patches
{
    class MonkeTVPatch
    {
        [HarmonyPatch(typeof(GorillaLocomotion.Player))]
        [HarmonyPatch("Awake")]
        public class Patch
        {
            private static void Postfix(GorillaLocomotion.Player __instance)
            {
                __instance.gameObject.AddComponent<Behaviours.TVClass>();
            }
        }
    }
}
