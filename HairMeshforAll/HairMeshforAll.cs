using AIChara;
using BepInEx;
using HarmonyLib;

namespace HairMeshforAll
{
    [BepInPlugin(GUID, "Hair Mesh for All", Version)]
    public partial class HairMeshforAll : BaseUnityPlugin
    {
        public const string GUID = "kky.HairMeshforAll";
        public const string Version = "1.0.0";

        private static CmpHair hairComponent;
        private static bool goAhead;

        public static void Logging(BepInEx.Logging.LogLevel level, string _text)
        {
            BepInEx.Logging.Logger.CreateLogSource(nameof(HairMeshforAll)).Log(level, _text);
        }

        private void Main()
        {
            Harmony.CreateAndPatchAll(typeof(HairMeshforAll));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AIChara.ChaControl), "ChangeSettingHairMeshType")]

        public static void EnableUseMesh_Patch(AIChara.ChaControl __instance, int parts)
        {
            goAhead = false;
            hairComponent = __instance.GetCustomHairComponent(parts);
            if (hairComponent != null)
            {
                goAhead = hairComponent.rendHair != null && hairComponent.rendHair.Length > 0 && !hairComponent.useMesh;
                if (goAhead) DoEnableUseMesh(hairComponent);
            }
        }

        private static void DoEnableUseMesh(CmpHair customHairComponent)
        {
            customHairComponent.useMesh = true;
            Logging(BepInEx.Logging.LogLevel.Debug, $"{customHairComponent.name} mesh enabled");
        }
    }
}
