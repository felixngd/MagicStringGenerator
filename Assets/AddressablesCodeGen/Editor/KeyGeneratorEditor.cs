#if ODIN_INSPECTOR || ODIN_INSPECTOR_3_0_OR_NEWER
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif
using System;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.Addressables
{
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3_0_OR_NEWER
    public class KeyGeneratorEditor : OdinEditorWindow
    {
#else
    public class KeyGeneratorEditor : EditorWindow
    {
#endif
        [MenuItem("Tools/CodeGen/Addressable Key Generator")]
        public static void ShowWindow()
        {
           GetWindow(typeof(KeyGeneratorEditor));
        }
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3_0_OR_NEWER

        [LabelText("Namespace")] public string nameSpace = "Wolffun.CodeGen.Addressables";
        [LabelText("Class Name")] public string className = "AddressableKey";
        [FolderPath] public string outputPath = "Assets/Scripts/Addressables";
        
        private ConfigAsset _config;
        
        protected override void OnEnable()
        {
            _config = Resources.Load<ConfigAsset>("AddressablesCodeGenConfig");
            if (_config.keyGeneratorConfig != null)
            {
                nameSpace = _config.keyGeneratorConfig.Namespace;
                className = _config.keyGeneratorConfig.ClassName;
                outputPath = _config.keyGeneratorConfig.OutputPath;
            }
        }
        
        [Button]
        public void Generate()
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Namespace cannot be empty", "Ok");
                return;
            }
        
            if (string.IsNullOrEmpty(outputPath))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Output path cannot be empty", "Ok");
                return;
            }
        
            if (string.IsNullOrEmpty(className))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Class name cannot be empty", "Ok");
                return;
            }
        
            _config.keyGeneratorConfig = new KeyGeneratorConfig()
            {
                Namespace = nameSpace,
                ClassName = className,
                OutputPath = outputPath
            };
            try
            {
                AddressableKeyGenerator.GenerateAddressableKeys(_config.keyGeneratorConfig);
                //dialog
                EditorUtility.DisplayDialog("Success", "Addressable keys generated successfully", "Ok");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            finally
            {
                EditorUtility.SetDirty(_config);
            }
        }
#else
       private ConfigAsset _config;
         void OnEnable()
        {
            _config = Resources.Load<ConfigAsset>("AddressablesCodeGenConfig");
        }
        private void OnGUI()
        {

            GUILayout.Label("Addressables Key Generator", EditorStyles.boldLabel);
            GUILayout.Label("Generate keys for all addressables group in the project", EditorStyles.label);
            //field for namespace
            GUILayout.Label("Namespace", EditorStyles.boldLabel);
            var namespaceField = GUILayout.TextField(_config.keyGeneratorConfig.Namespace);
        
            //field for output path
            GUILayout.Label("Output Path", EditorStyles.boldLabel);
            var outputPathField = GUILayout.TextField(_config.keyGeneratorConfig.OutputPath);
        
            // field for class name
            GUILayout.Label("Class Name", EditorStyles.boldLabel);
            var classNameField = GUILayout.TextField(_config.keyGeneratorConfig.ClassName);
            
            if(!_config.keyGeneratorConfig.IsValid())
            {
                _config.keyGeneratorConfig = new KeyGeneratorConfig()
                {
                    Namespace = "Wolffun.CodeGen.Addressables",
                    ClassName = "AddressableKey",
                    OutputPath = "Assets/Scripts/Addressables"
                };
            }

            if (GUILayout.Button("Generate Keys"))
            {
                if (string.IsNullOrEmpty(namespaceField))
                {
                    //dialog
                    EditorUtility.DisplayDialog("Error", "Namespace cannot be empty", "Ok");
                    return;
                }
                if (string.IsNullOrEmpty(outputPathField))
                {
                    //dialog
                    EditorUtility.DisplayDialog("Error", "Output path cannot be empty", "Ok");
                    return;
                }
                if (string.IsNullOrEmpty(classNameField))
                {
                    //dialog
                    EditorUtility.DisplayDialog("Error", "Class name cannot be empty", "Ok");
                    return;
                }
                
                _config.keyGeneratorConfig = new KeyGeneratorConfig()
                {
                    Namespace = namespaceField,
                    ClassName = classNameField,
                    OutputPath = outputPathField
                };
                try
                {
                    AddressableKeyGenerator.GenerateAddressableKeys(_config.keyGeneratorConfig);
                    //dialog
                    EditorUtility.DisplayDialog("Success", "Addressable keys generated successfully", "Ok");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
                finally
                {
                    EditorUtility.SetDirty(_config);
                }
            }
        }
#endif
    }
}