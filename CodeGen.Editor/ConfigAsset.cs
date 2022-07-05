using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace
namespace Wolffun.CodeGen.MagicString.Editor
{
    [CreateAssetMenu(fileName = "CodeGenConfig.asset", menuName = "Tools/CodeGen/Magic String Generator/Create Config", order = 0)]
    public class ConfigAsset : ScriptableObject
    {
        public KeyGeneratorConfig keyGeneratorConfig;
    }
}