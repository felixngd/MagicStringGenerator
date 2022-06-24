#if ODIN_INSPECTOR || ODIN_INSPECTOR_3_0_OR_NEWER
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif
using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.Addressables.Editor
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
        [LabelText("Namespace"), VerticalGroup("Generate keys in static class")] public string nameSpace = "Wolffun.CodeGen.Addressables";
        [LabelText("Class Name"), VerticalGroup("Generate keys in static class")] public string className = "AddressableKey";
        [FolderPath, VerticalGroup("Generate keys in static class")] public string outputPath = "Assets/Scripts/Addressables";

        private ConfigAsset _config;

        protected override void OnEnable()
        {
            _config = Resources.Load<ConfigAsset>("AddressablesCodeGenConfig");
            if (_config.keyGeneratorConfig != null)
            {
                nameSpace = _config.keyGeneratorConfig.Namespace;
                className = _config.keyGeneratorConfig.ClassName;
                outputPath = _config.keyGeneratorConfig.StaticClassOutputPath;
            }
        }

        [Button, VerticalGroup("Generate keys in static class")]
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
                StaticClassOutputPath = outputPath
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

        [FolderPath, VerticalGroup("Generate scriptable objects of key groups")] public string scriptableObjectOutputPath = "";

        [Button, VerticalGroup("Generate scriptable objects of key groups")]
        public void CreateScriptableObjects()
        {
            if (string.IsNullOrEmpty(scriptableObjectOutputPath))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Scriptable object output path cannot be empty", "Ok");
                return;
            }

            _config.keyGeneratorConfig.ScriptableObjectPath = scriptableObjectOutputPath;

            try
            {
                AddressableKeyGenerator.CreateScriptableObjects(_config.keyGeneratorConfig);
                //dialog
                EditorUtility.DisplayDialog("Success", "Scriptable objects created successfully", "Ok");
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
        
        [Button, VerticalGroup("Update scriptable objects of key groups")]
        public void UpdateScriptableObjects()
        {
            if (string.IsNullOrEmpty(scriptableObjectOutputPath))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Scriptable object output path cannot be empty", "Ok");
                return;
            }

            _config.keyGeneratorConfig.ScriptableObjectPath = scriptableObjectOutputPath;

            try
            {
                AddressableKeyGenerator.UpdateScriptableObjects();
                //dialog
                EditorUtility.DisplayDialog("Success", "Scriptable objects updated successfully", "Ok");
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
            GUILayout.Label(
                "Generate keys for all addressables group in the project as a <color=green>static class</color>",
                EditorStyles.boldLabel);
            //field for namespace
            GUILayout.Label("Namespace", EditorStyles.boldLabel);

            var namespaceField = GUILayout.TextField(_config.keyGeneratorConfig.Namespace);
            if (namespaceField != _config.keyGeneratorConfig.Namespace)
            {
                _config.keyGeneratorConfig.Namespace = namespaceField;
                EditorUtility.SetDirty(_config);
            }

            //field for output path
            GUILayout.Label("Output Path", EditorStyles.boldLabel);
            var outputPathField = GUILayout.TextField(_config.keyGeneratorConfig.StaticClassOutputPath);
            if (outputPathField != _config.keyGeneratorConfig.StaticClassOutputPath)
            {
                _config.keyGeneratorConfig.StaticClassOutputPath = outputPathField;
                EditorUtility.SetDirty(_config);
            }

            // field for class name
            GUILayout.Label("Class Name", EditorStyles.boldLabel);
            var classNameField = GUILayout.TextField(_config.keyGeneratorConfig.ClassName);
            if (classNameField != _config.keyGeneratorConfig.ClassName)
            {
                _config.keyGeneratorConfig.ClassName = classNameField;
                EditorUtility.SetDirty(_config);
            }

            if (!_config.keyGeneratorConfig.IsValid())
            {
                _config.keyGeneratorConfig = new KeyGeneratorConfig()
                {
                    Namespace = "Wolffun.CodeGen.Addressables",
                    ClassName = "AddressableKey",
                    StaticClassOutputPath = "Assets/Scripts/Addressables"
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
                    StaticClassOutputPath = outputPathField
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

            GUILayout.Space(30);
            GUILayout.Label("Create scriptable objects for key groups", EditorStyles.boldLabel);

            //field for scriptable objects out put
            GUILayout.Label("Scriptable Object Output Path", EditorStyles.boldLabel);
            var scriptableObjectOutputPathField = GUILayout.TextField(_config.keyGeneratorConfig.ScriptableObjectPath);
            if (scriptableObjectOutputPathField != _config.keyGeneratorConfig.ScriptableObjectPath)
            {
                _config.keyGeneratorConfig.ScriptableObjectPath = scriptableObjectOutputPathField;
                EditorUtility.SetDirty(_config);
            }

            if (GUILayout.Button("Create Scriptable Objects"))
            {
                if (string.IsNullOrEmpty(scriptableObjectOutputPathField))
                {
                    //dialog
                    EditorUtility.DisplayDialog("Error", "Scriptable object output path cannot be empty", "Ok");
                    return;
                }

                _config.keyGeneratorConfig.ScriptableObjectPath = scriptableObjectOutputPathField;

                try
                {
                    AddressableKeyGenerator.CreateScriptableObjects(_config.keyGeneratorConfig);
                    //dialog
                    EditorUtility.DisplayDialog("Success", "Scriptable objects created successfully", "Ok");
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
            
            if(GUILayout.Button("Update Scriptable Objects"))
            {
                if (string.IsNullOrEmpty(scriptableObjectOutputPathField))
                {
                    //dialog
                    EditorUtility.DisplayDialog("Error", "Scriptable object output path cannot be empty", "Ok");
                    return;
                }

                _config.keyGeneratorConfig.ScriptableObjectPath = scriptableObjectOutputPathField;

                try
                {
                    AddressableKeyGenerator.UpdateScriptableObjects();
                    //dialog
                    EditorUtility.DisplayDialog("Success", "Scriptable objects updated successfully", "Ok");
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