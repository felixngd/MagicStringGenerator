using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.MagicString.Editor
{
    public class KeyGeneratorEditor : OdinEditorWindow
    {
        [MenuItem("Tools/CodeGen/Magic String Generator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(KeyGeneratorEditor));
        }

        [LabelText("Namespace"), TabGroup("Addressable Magic Strings")]
        public string addressableStringNameSpace = "Wolffun.CodeGen.Addressables";

        [LabelText("Class Name"), TabGroup("Addressable Magic Strings")]
        public string addressableStringClassName = "AddressableKey";

        [LabelText("Output Path"), FolderPath, TabGroup("Addressable Magic Strings")]
        public string addressableStringOutputPath = "Assets/Scripts/Generated";

        [TabGroup("Addressable Magic Strings")]
        public AddressableGroupsCodeGenConfig includeAddressableGroups;

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

            if (includeAddressableGroups == null)
            {
                //find
                includeAddressableGroups = AssetDatabase.LoadAssetAtPath<AddressableGroupsCodeGenConfig>(Path.Combine(ConfigPath, $"AddressableGroupsCodeGenConfig.asset"));
                // if not found
                if (includeAddressableGroups == null)
                {
                    //create new one
                    includeAddressableGroups = CreateInstance<AddressableGroupsCodeGenConfig>();
                    //save scriptable object to Resources folder
                    AssetDatabase.CreateAsset(includeAddressableGroups, Path.Combine(ConfigPath, $"AddressableGroupsCodeGenConfig.asset"));
                    AssetDatabase.SaveAssets();
                }
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
                    includeAddressableGroups.includeAddressableGroups);
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
