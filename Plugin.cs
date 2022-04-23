using HarmonyLib;
using BepInEx;
using Bepinject;
using System.Reflection;
using Utilla;
using System.ComponentModel;

namespace MonkeTV
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = new Harmony(PluginInfo.GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
