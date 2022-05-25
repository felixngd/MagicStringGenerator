using System;
using System.IO;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.Addressables
{
    [Serializable]
    public class KeyGeneratorConfig
    {
        //namespace
        [SerializeField]
        private string @namespace;
        public string Namespace { get { return @namespace; } set { @namespace = value; } }

        //class name
        [SerializeField]
        private string className;
        public string ClassName { get { return className; } set { className = value; } }

        //output path
        [SerializeField]
        private string outputPath;
        public string OutputPath { get { return outputPath; } set { outputPath = value; } }

        public string GetFullOutputPath()
        {
            if(OutputPath.StartsWith("Assets/"))
            {
                OutputPath = OutputPath.Substring(7);
                return Path.Combine(Application.dataPath, $"{OutputPath}/{ClassName}.cs");
            }
            else
            {
                 return Path.Combine(Application.dataPath, $"{OutputPath}/{ClassName}.cs");
            }
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Namespace) && !string.IsNullOrEmpty(ClassName) && !string.IsNullOrEmpty(OutputPath);
        }
    }
}