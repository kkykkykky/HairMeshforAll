using Chara;
using HarmonyLib;
using UnityEngine;
using System;

namespace HairMeshforAll
{
    public class HairMeshforAllComponent : MonoBehaviour
    {
        private static CmpHair hairComponent;
        private static bool goAhead;
        public HairMeshforAllComponent(IntPtr ptr) : base(ptr)
        {
            //BepInExLoader.log.LogMessage("[HairMeshforAll] Entered Constructor");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Chara.ChaControl), "ChangeSettingHairMeshType")]

        public static void EnableUseMesh_Patch(Chara.ChaInfo __instance, int parts)
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
            BepInExLoader.log.LogDebug($"{customHairComponent.name} mesh enabled");
        }
    }
}
