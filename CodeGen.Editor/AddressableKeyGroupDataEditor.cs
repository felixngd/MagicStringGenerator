using UnityEditor;
using UnityEngine;

namespace Wolffun.CodeGen.Addressables.Editor
{
    [CustomEditor(typeof(AddressableKeyGroupData))]
    public class AddressableKeyGroupDataEditor : UnityEditor.Editor
    {
        private AddressableKeyGroupData _target;
        
        private void OnEnable()
        {
            _target = (AddressableKeyGroupData) target;
        }

        public override void OnInspectorGUI()
        {
            //show all properties of _target
            DrawDefaultInspector();

            //label: Nhập vào một group name hoặc label name rồi bấm Set data để lấy keys
            EditorGUILayout.LabelField("Nhập vào một group name hoặc label name rồi bấm Set data để lấy keys");
            //button
            if (GUILayout.Button("Set data"))
            {
                AddressableKeyGenerator.SetScriptableObject(_target, _target.GroupOrLabelName);
            }
        }
    }
}