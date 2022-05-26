using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace
namespace Wolffun.CodeGen.Addressables
{
    public class ConfigAsset : ScriptableObject
    {
        public KeyGeneratorConfig keyGeneratorConfig;
        
        
        //create a new asset
        //[MenuItem("Tools/CodeGen/Create Config Asset")]
        public static void CreateAsset()
        {
            var asset = CreateInstance<ConfigAsset>();
            AssetDatabase.CreateAsset(asset, "Assets/AddressablesCodeGen/Editor/Settings/AddressablesCodeGenConfig.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}