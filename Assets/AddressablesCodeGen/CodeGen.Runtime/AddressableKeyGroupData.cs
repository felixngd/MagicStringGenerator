using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Wolffun.CodeGen.Addressables
{
    /// <summary>
    /// Group all key by its Asset group or a label
    /// </summary>
    [CreateAssetMenu(fileName = "AssetKeyGroup", menuName = "AddressablesCodeGen/AssetKeyGroup", order = 1)]
    public class AddressableKeyGroupData : UnityEngine.ScriptableObject
    {
        public string GroupOrLabelName;
        public string[] Keys;
        
        public UniTask PreloadAsync<T>() where T : UnityEngine.Object
        {
            var tasks = new List<UniTask>();
            for (var i = 0; i < Keys.Length; i++)
            {
                var task = AddressablesManager.LoadAssetAsync<T>(Keys[i]);
                tasks.Add(task);
            }
            return UniTask.WhenAll(tasks);
        }
    }
}