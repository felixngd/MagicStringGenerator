using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Wolffun.CodeGen.Addressables
{
    public static class AddressableKeyGenerator
    {
        public static void GenerateAddressableKeys(KeyGeneratorConfig config)
        {
            try
            {
                Write(GetKeyGroups(), config);
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
                //regex spaces and special characters
                var regex = new Regex(@"[^a-zA-Z0-9]");
                var className = regex.Replace(keyGroup.Key, string.Empty);
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
                    var fieldName = regex.Replace(key, string.Empty);
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

        static Dictionary<string, HashSet<string>> GetKeyGroups()
        {
            var groups = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.groups;
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
    }
}