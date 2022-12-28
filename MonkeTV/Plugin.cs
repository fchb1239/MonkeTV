using BepInEx;
using HarmonyLib;
using MonkeTV.Behaviours;
using System.Reflection;

namespace MonkeTV
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public Harmony tHarmony;

        public TVClass tClass;

        public void Awake()
        {
            if (!(Instance is null)) return;
            Instance = this;

            tHarmony = new Harmony(PluginInfo.GUID);
            tHarmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
