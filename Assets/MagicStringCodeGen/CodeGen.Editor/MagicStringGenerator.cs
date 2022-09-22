using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Localization;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.MagicString.Editor
{
    public static class MagicStringGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="groups">groups = null if you want to generate all addressables group</param>
        public static void GenerateAddressableKeys(KeyGeneratorConfig config, AddressableAssetGroup[] groups = null)
        {
            try
            {
                Write(GetAddressableKeyGroups(groups), config);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public static void GenerateLocalizationKeys(KeyGeneratorConfig config)
        {
            try
            {
                Write(GetLocalizationTableGroups(), config);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private static void GenerateCSharpCode(CodeCompileUnit targetUnit, string fileName)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = true;
            options.VerbatimOrder = true;

            using (var sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }

            AssetDatabase.Refresh();
        }

        static void Write(Dictionary<string, HashSet<string>> keyGroups, KeyGeneratorConfig config)
        {
            var targetUnit = new CodeCompileUnit();
            var codeNamespace = new CodeNamespace(config.Namespace);
            var targetClass = new CodeTypeDeclaration(config.ClassName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            codeNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(codeNamespace);

            foreach (var keyGroup in keyGroups)
            {
                var keys = keyGroup.Value;
                //regex spaces and special characters excepts underscore
                var regex = new Regex(@"[^a-zA-Z0-9_ -]");
                var className = regex.Replace(keyGroup.Key, string.Empty).Replace(" ", "_").Replace("-", "_");
                //if keyName start with number, add _
                if (char.IsDigit(className[0]))
                {
                    className = "_" + className;
                }

                //add a local class
                var localClass = new CodeTypeDeclaration(className)
                {
                    IsClass = true,
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                };
                targetClass.Members.Add(localClass);
                foreach (var key in keys)
                {
                    var fieldName = regex.Replace(key, string.Empty).Replace(" ", "_").Replace("-", "_");
                    if (string.IsNullOrEmpty(fieldName))
                        continue;
                    //if keyName start with number, add _
                    if (char.IsDigit(fieldName[0]))
                    {
                        fieldName = "_" + fieldName;
                    }

                    var idField = new CodeMemberField(typeof(string), fieldName)
                    {
                        Attributes = MemberAttributes.Public | MemberAttributes.Const,
                        InitExpression = new CodePrimitiveExpression(key)
                    };

                    localClass.Members.Add(idField);
                }
            }

            GenerateCSharpCode(targetUnit, config.GetFullOutputPath());
        }

        static Dictionary<string, HashSet<string>> GetLocalizationTableGroups()
        {
            var collections = LocalizationEditorSettings.GetStringTableCollections();
            var keyGroups = new Dictionary<string, HashSet<string>>();
            foreach (var tableCollection in collections)
            {
                var stringTables = tableCollection.StringTables;
                foreach (var table in stringTables)
                {
                    var stringTableEntries = table.Values;
                    var keys = new HashSet<string>();
                    foreach (var stringTableEntry in stringTableEntries)
                    {
                        var key = stringTableEntry.Key;
                        if (string.IsNullOrEmpty(key))
                        {
                            continue;
                        }

                        keys.Add(key);
                    }

                    if (keyGroups.ContainsKey(table.TableCollectionName))
                    {
                        keyGroups[table.TableCollectionName].UnionWith(keys);
                    }
                    else
                    {
                        keyGroups.Add(table.TableCollectionName, keys);
                    }
                }
            }

            return keyGroups;
        }

        static Dictionary<string, HashSet<string>> GetAddressableKeyGroups(AddressableAssetGroup[] groups = null)
        {
            if(groups == null || groups.Length == 0)
                groups = AddressableAssetSettingsDefaultObject.Settings.groups.ToArray();
            //var groups = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.groups;
            var keyGroups = new Dictionary<string, HashSet<string>>();
            foreach (var group in groups)
            {
                var schema = group.Schemas[0];
                var keys = new HashSet<string>();
                var entries = schema.Group.entries;
                foreach (var addressableAssetEntry in entries)
                {
                    var added = keys.Add(addressableAssetEntry.address);
                    if (!added)
                    {
                        Debug.LogWarning($"Duplicate key {addressableAssetEntry.address} in group {group.Name}");
                    }
                }

                if (keyGroups.ContainsKey(group.Name))
                {
                    Debug.LogWarning($"Duplicate group {group.Name}");
                }
                else
                {
                    keyGroups.Add(group.Name, keys);
                }
            }

            return keyGroups;
        }

        //get all key by label
        static Dictionary<string, HashSet<string>> GetLabelGroups()
        {
            var groups = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.groups;
            var labelGroups = new Dictionary<string, HashSet<string>>();
            foreach (var group in groups)
            {
                var schema = group.Schemas[0];
                var entries = schema.Group.entries;
                foreach (var addressableAssetEntry in entries)
                {
                    var labels = addressableAssetEntry.labels;
                    var key = addressableAssetEntry.address;
                    //add label as key, addresses as value to labelGroups
                    foreach (var label in labels)
                    {
                        if (labelGroups.ContainsKey(label))
                        {
                            var added = labelGroups[label].Add(key);
                            if (!added)
                            {
                                Debug.LogWarning($"Duplicate key {key} in label {label}");
                            }
                        }
                        else
                        {
                            labelGroups.Add(label, new HashSet<string> {key});
                        }
                    }
                }
            }

            return labelGroups;
        }

        public static void CreateScriptableObjects(KeyGeneratorConfig config)
        {
            DeleteScriptableObjects<AddressableKeyGroupData>();

            var labelGroups = GetLabelGroups();
            foreach (var group in labelGroups)
            {
                var path = config.GetFullPathScriptableObject(group.Key + "_key_group");
                var assetPath = AssetDatabase.GenerateUniqueAssetPath(path);
                var keyGroupData = ScriptableObject.CreateInstance<AddressableKeyGroupData>();
                keyGroupData.GroupOrLabelName = group.Key;
                keyGroupData.Keys = group.Value.ToArray();
                AssetDatabase.CreateAsset(keyGroupData, assetPath);
            }

            var keyGroups = GetAddressableKeyGroups();
            foreach (var group in keyGroups)
            {
                var path = config.GetFullPathScriptableObject(group.Key + "_key_group");
                var assetPath = AssetDatabase.GenerateUniqueAssetPath(path);
                var keyGroupData = ScriptableObject.CreateInstance<AddressableKeyGroupData>();
                keyGroupData.GroupOrLabelName = group.Key;
                keyGroupData.Keys = group.Value.ToArray();
                AssetDatabase.CreateAsset(keyGroupData, assetPath);
            }

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
        }

        public static void UpdateScriptableObjects()
        {
            var labelGroups = GetLabelGroups();
            UpdateScriptableObjects(labelGroups);
            var keyGroups = GetAddressableKeyGroups();
            UpdateScriptableObjects(keyGroups);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
        }

        private static void UpdateScriptableObjects(Dictionary<string, HashSet<string>> groups)
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(AddressableKeyGroupData)}");
            if (guids.Length == 0)
            {
                Debug.LogWarning("No AddressableKeyGroupData found");
                return;
            }

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<AddressableKeyGroupData>(path);
                if (asset == null)
                {
                    Debug.LogWarning($"{path} is not a AddressableKeyGroupData");
                    continue;
                }

                if (groups.ContainsKey(asset.GroupOrLabelName) &&
                    groups[asset.GroupOrLabelName].ToArray() == asset.Keys)
                {
                    continue;
                }

                if (groups.ContainsKey(asset.GroupOrLabelName))
                {
                    asset.Keys = groups[asset.GroupOrLabelName].ToArray();
                    Debug.Log($"{path} updated");
                }
            }
        }

        private static void DeleteScriptableObjects<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.DeleteAsset(path);
            }
        }

        public static void SetScriptableObject(AddressableKeyGroupData asset, string labelOrGroupName)
        {
            var labelGroups = GetLabelGroups();
            if (labelGroups.ContainsKey(labelOrGroupName))
            {
                asset.GroupOrLabelName = labelOrGroupName;
                asset.Keys = labelGroups[labelOrGroupName].ToArray();
            }
            else
            {
                var keyGroups = GetAddressableKeyGroups();
                if (keyGroups.ContainsKey(labelOrGroupName))
                {
                    asset.GroupOrLabelName = labelOrGroupName;
                    asset.Keys = keyGroups[labelOrGroupName].ToArray();
                }
                else
                {
                    Debug.LogError($"{labelOrGroupName} is not a valid label or group name");
                }
            }
        }
    }
}