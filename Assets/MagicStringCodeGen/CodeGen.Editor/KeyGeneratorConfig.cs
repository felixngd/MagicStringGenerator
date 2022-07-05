using System;
using System.IO;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.MagicString.Editor
{
    [Serializable]
    public class KeyGeneratorConfig
    {
        //namespace
        [SerializeField] private string @namespace;

        public string Namespace
        {
            get { return @namespace; }
            set { @namespace = value; }
        }

        //class name
        [SerializeField] private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        //output path
        [SerializeField] private string staticClassOutputPath;

        public string StaticClassOutputPath
        {
            get { return staticClassOutputPath; }
            set { staticClassOutputPath = value; }
        }

        //scriptable object path
        [SerializeField] private string scriptableObjectPath;

        public string ScriptableObjectPath
        {
            get { return scriptableObjectPath; }
            set { scriptableObjectPath = value; }
        }

        //path for generated scriptable object
        public string GetFullPathScriptableObject(string assetName)
        {
            if (ScriptableObjectPath.StartsWith("Assets/"))
            {
                return $"{ScriptableObjectPath}/{assetName}.asset";
            }
            else
            {
                return $"Assets/{ScriptableObjectPath}/{assetName}.asset";
            }
        }

        /// <summary>
        /// Path for generated static class
        /// </summary>
        /// <returns></returns>
        public string GetFullOutputPath()
        {
            if (StaticClassOutputPath.StartsWith("Assets/"))
            {
                StaticClassOutputPath = StaticClassOutputPath.Substring(7);
                return Path.Combine(Application.dataPath, $"{StaticClassOutputPath}/{ClassName}.cs");
            }
            else
            {
                return Path.Combine(Application.dataPath, $"{StaticClassOutputPath}/{ClassName}.cs");
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Namespace) && !string.IsNullOrEmpty(ClassName) &&
                   !string.IsNullOrEmpty(StaticClassOutputPath);
        }
    }
}