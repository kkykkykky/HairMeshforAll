using Chara;
using BepInEx;
using UnhollowerRuntimeLib;
using HarmonyLib;
using UnityEngine;

namespace HairMeshforAll
{
    [BepInPlugin(GUID, "RG Hair Mesh for All", Version)]
    [BepInProcess("RoomGirlTrial")]
    [BepInProcess("RoomGirl")]
    public class BepInExLoader : BepInEx.IL2CPP.BasePlugin
    {
        public const string GUID = "kky.RG.HairMeshforAll";
        public const string Version = "1.0.1";

        public static BepInEx.Logging.ManualLogSource log;

        public BepInExLoader()
        {
            log = Log;
        }

        public override void Load()
        {
            try
            {
                // Register custom Types in Il2Cpp
                ClassInjector.RegisterTypeInIl2Cpp<HairMeshforAllComponent>();

                var go = new GameObject("HairMeshforAllObject");
                go.AddComponent<HairMeshforAllComponent>();
                Object.DontDestroyOnLoad(go);
            }
            catch
            {
                log.LogError("[HairMeshforAll] FAILED to Register Il2Cpp Type: HairMeshforAllComponent!");
            }

            try
            {
                var harmony = new Harmony(GUID);

                // Hooks 

                var originalChangeSettingHairMeshType = AccessTools.Method(typeof(Chara.ChaControl), "ChangeSettingHairMeshType");
                var postChangeSettingHairMeshType = AccessTools.Method(typeof(HairMeshforAllComponent), "EnableUseMesh_Patch");
                harmony.Patch(originalChangeSettingHairMeshType, prefix: new HarmonyMethod(postChangeSettingHairMeshType));
            }
            catch
            {
                log.LogError("[HairMeshforAll] Harmony - FAILED to Apply Patch's!");
            }
        }
    }
}
