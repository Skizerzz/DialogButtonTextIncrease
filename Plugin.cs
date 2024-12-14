using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace DialogButtonTextIncrease {
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        internal static new ManualLogSource Logger;

        private void Awake() {
            // Plugin startup logic
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(DialogManager), "Create_DialogSelectionButton")]
        public static class DialogManager_Create_DialogSelectionButton {
            public static void Postfix(ref DialogManager __instance) {
                FieldInfo _selectionContainerFieldInfo = AccessTools.Field(typeof(DialogManager), "_selectionContainer");
                if(_selectionContainerFieldInfo == null) {
                    Logger.LogWarning("Failed to get _selectionContainer");
                    return;
                }
                
                try {
                    var _selectionFieldObj = _selectionContainerFieldInfo.GetValue(__instance);
                    Transform _selectionContainer = (Transform)_selectionFieldObj;

                    UnityEngine.UI.Text[] texts = _selectionContainer.GetComponentsInChildren<UnityEngine.UI.Text>();

                    if(texts == null) {
                        Logger.LogWarning("Text components were null.");
                        return;
                    }

                    foreach (UnityEngine.UI.Text text in texts) {
                        text.resizeTextForBestFit = true;
                    }
                } catch (Exception e) {
                    Logger.LogWarning("Failed to cast _selectionContainer");
                    return;
                }
            }
        }
    }
}
