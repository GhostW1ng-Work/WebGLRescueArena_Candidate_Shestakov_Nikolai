#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace WebGLRescueArena.Editor
{
    public static class TestContentGenerator
    {
        [MenuItem("Tools/WebGL Rescue Arena/Generate Test Assets")]
        public static void GenerateTestAssets()
        {
            const string materialFolder = "Assets/_Project/Art/Materials";
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Art")) AssetDatabase.CreateFolder("Assets/_Project", "Art");
            if (!AssetDatabase.IsValidFolder(materialFolder)) AssetDatabase.CreateFolder("Assets/_Project/Art", "Materials");
            CreateMaterial(materialFolder + "/Player.mat", new Color(0.12f, 0.65f, 1f));
            CreateMaterial(materialFolder + "/Enemy.mat", new Color(0.9f, 0.2f, 0.2f));
            CreateMaterial(materialFolder + "/Floor.mat", new Color(0.12f, 0.15f, 0.18f));
            CreateMaterial(materialFolder + "/Wall.mat", new Color(0.35f, 0.38f, 0.42f));
            CreateMaterial(materialFolder + "/Projectile.mat", new Color(1f, 0.75f, 0.1f));
            AssetDatabase.SaveAssets();
        }
        private static void CreateMaterial(string path, Color color)
        {
            if (AssetDatabase.LoadAssetAtPath<Material>(path) != null) return;
            Material material = new Material(Shader.Find("Standard")) { color = color };
            AssetDatabase.CreateAsset(material, path);
        }
    }
}
#endif
