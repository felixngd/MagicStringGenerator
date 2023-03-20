#if ODIN_INSPECTOR || ODIN_INSPECTOR_3_0_OR_NEWER
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor.AddressableAssets.Settings;
#endif
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.MagicString.Editor
{
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3_0_OR_NEWER
    public class KeyGeneratorEditor : OdinEditorWindow
    {
#else
    public class KeyGeneratorEditor : EditorWindow
    {
#endif
        [MenuItem("Tools/CodeGen/Magic String Generator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(KeyGeneratorEditor));
        }
#if ODIN_INSPECTOR || ODIN_INSPECTOR_3_0_OR_NEWER
        [LabelText("Namespace"), TabGroup("Addressable Magic Strings")]
        public string addressableStringNameSpace = "Wolffun.CodeGen.Addressables";

        [LabelText("Class Name"), TabGroup("Addressable Magic Strings")]
        public string addressableStringClassName = "AddressableKey";

        [LabelText("Output Path"), FolderPath, TabGroup("Addressable Magic Strings")]
        public string addressableStringOutputPath = "Assets/Scripts/Generated";

        [TabGroup("Addressable Magic Strings")]
        public AddressableAssetGroup[] includeAddressableGroups;

        [LabelText("Namespace"), TabGroup("Localization Magic Strings")]
        public string localizationStringNameSpace = "Wolffun.CodeGen.Localization";

        [LabelText("Class Name"), TabGroup("Localization Magic Strings")]
        public string localizationStringClassName = "L";

        [LabelText("Output Path"), FolderPath, TabGroup("Localization Magic Strings")]
        public string localizationStringOutputPath = "Assets/Scripts/Generated";


        private ConfigAsset _addressableStringConfig;
        private ConfigAsset _localizationStringConfig;

        protected override void OnEnable()
        {
            CreateConfigFolderIfNotAvailable();
            _addressableStringConfig =
                Resources.Load<ConfigAsset>("AddressablesCodeGenConfig");
            if (_addressableStringConfig == null)
            {
                //create new one
                _addressableStringConfig = CreateInstance<ConfigAsset>();
                //save scriptable object to Resources folder
                AssetDatabase.CreateAsset(_addressableStringConfig, Path.Combine(ConfigPath, $"AddressablesCodeGenConfig.asset"));
                AssetDatabase.SaveAssets();
            }

            if (_addressableStringConfig.keyGeneratorConfig != null)
            {
                addressableStringNameSpace = _addressableStringConfig.keyGeneratorConfig.Namespace;
                addressableStringClassName = _addressableStringConfig.keyGeneratorConfig.ClassName;
                addressableStringOutputPath = _addressableStringConfig.keyGeneratorConfig.StaticClassOutputPath;
            }

            _localizationStringConfig =
                Resources.Load<ConfigAsset>("LocalizationCodeGenConfig");
            if(_localizationStringConfig == null)
            {
                //create new one
                _localizationStringConfig = CreateInstance<ConfigAsset>();
                //save it to Resources folder
                AssetDatabase.CreateAsset(_localizationStringConfig,
                    Path.Combine(ConfigPath, "LocalizationCodeGenConfig.asset"));
                AssetDatabase.SaveAssets();
            }

            if (_localizationStringConfig.keyGeneratorConfig != null)
            {
                localizationStringNameSpace = _localizationStringConfig.keyGeneratorConfig.Namespace;
                localizationStringClassName = _localizationStringConfig.keyGeneratorConfig.ClassName;
                localizationStringOutputPath = _localizationStringConfig.keyGeneratorConfig.StaticClassOutputPath;
            }
        }
        
        [Button, TabGroup("Addressable Magic Strings")]
        public void GenerateAddressableStrings()
        {
            if (string.IsNullOrEmpty(addressableStringNameSpace))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Namespace cannot be empty", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(addressableStringOutputPath))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Output path cannot be empty", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(addressableStringClassName))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Class name cannot be empty", "Ok");
                return;
            }

            _addressableStringConfig.keyGeneratorConfig = new KeyGeneratorConfig()
            {
                Namespace = addressableStringNameSpace,
                ClassName = addressableStringClassName,
                StaticClassOutputPath = addressableStringOutputPath
            };
            try
            {
                MagicStringGenerator.GenerateAddressableKeys(_addressableStringConfig.keyGeneratorConfig,
                    includeAddressableGroups);
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
                EditorUtility.SetDirty(_addressableStringConfig);
            }
        }

        [Button, TabGroup("Localization Magic Strings")]
        public void GenerateLocalizationStrings()
        {
            if (string.IsNullOrEmpty(addressableStringNameSpace))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Namespace cannot be empty", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(localizationStringOutputPath))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Output path cannot be empty", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(localizationStringClassName))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Class name cannot be empty", "Ok");
                return;
            }

            _localizationStringConfig.keyGeneratorConfig = new KeyGeneratorConfig()
            {
                Namespace = localizationStringNameSpace,
                ClassName = localizationStringClassName,
                StaticClassOutputPath = localizationStringOutputPath
            };

            try
            {
                MagicStringGenerator.GenerateLocalizationKeys(_localizationStringConfig.keyGeneratorConfig);
                //dialog
                EditorUtility.DisplayDialog("Success", "Localization keys generated successfully", "Ok");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            finally
            {
                EditorUtility.SetDirty(_localizationStringConfig);
            }
        }

        [FolderPath, LabelText("Generate scriptable objects of key groups", true),
         TabGroup("Addressable Magic Strings")]
        public string scriptableObjectOutputPath = "";

        [Button, LabelText("Generate scriptable objects of key groups"), TabGroup("Addressable Magic Strings")]
        public void CreateScriptableObjects()
        {
            if (string.IsNullOrEmpty(scriptableObjectOutputPath))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Scriptable object output path cannot be empty", "Ok");
                return;
            }

            _addressableStringConfig.keyGeneratorConfig.ScriptableObjectPath = scriptableObjectOutputPath;

            try
            {
                MagicStringGenerator.CreateScriptableObjects(_addressableStringConfig.keyGeneratorConfig);
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
                EditorUtility.SetDirty(_addressableStringConfig);
            }
        }

        [Button, LabelText("Update scriptable objects of key groups"), TabGroup("Addressable Magic Strings")]
        public void UpdateScriptableObjects()
        {
            if (string.IsNullOrEmpty(scriptableObjectOutputPath))
            {
                //dialog
                EditorUtility.DisplayDialog("Error", "Scriptable object output path cannot be empty", "Ok");
                return;
            }

            _addressableStringConfig.keyGeneratorConfig.ScriptableObjectPath = scriptableObjectOutputPath;

            try
            {
                MagicStringGenerator.UpdateScriptableObjects();
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
                EditorUtility.SetDirty(_addressableStringConfig);
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
                    MagicStringGenerator.GenerateAddressableKeys(_config.keyGeneratorConfig);
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
                    MagicStringGenerator.CreateScriptableObjects(_config.keyGeneratorConfig);
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
                    MagicStringGenerator.UpdateScriptableObjects();
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
        private const string ConfigPath = "Assets/MagicStringGenerator/Editor/Resources";

        
        private void CreateConfigFolderIfNotAvailable()
        {
            var path = Path.Combine(Application.dataPath, "../", ConfigPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
