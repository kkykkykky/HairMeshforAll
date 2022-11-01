using Chara;
using HarmonyLib;
using UnityEngine;
using System;

namespace HairMeshforAll
{
    public class HairMeshforAllComponent : MonoBehaviour
    {
        private static readonly int meshMaskID = Shader.PropertyToID("_MeshColorMask");
        public HairMeshforAllComponent(IntPtr ptr) : base(ptr)
        {
            //BepInExLoader.log.LogMessage("[HairMeshforAll] Entered Constructor");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Chara.ChaControl), "ChangeSettingHairMeshType")]

        public static void EnableUseMesh_Patch(Chara.ChaInfo __instance, int parts)
        {
            bool goAhead = false;
            CmpHair hairComponent = __instance.GetCustomHairComponent(parts);
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
            foreach (Renderer ren in customHairComponent.rendHair)
            {
                Material material = ren.material;
                for (int i = 0; i < material.shader.GetPropertyCount(); i++)
                {
                    if (material.shader.GetPropertyName(i) == "_MeshColorMask" && material.GetTexture(meshMaskID) == null)
                    {
                        material.SetTexture(meshMaskID, Texture2D.redTexture);
                        break;
                    }
                }
            }
        }
    }
}
