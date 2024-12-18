// using AjaxNguyen.Core.Manager;
// using UnityEditor;
// using UnityEngine;

// namespace AjaxNguyen {
//     [CustomEditor(typeof(ResourceManager))]
//     public class TestEditor : UnityEditor.Editor {
//         public override void OnInspectorGUI() {
//             ResourceManager resourceManager = (ResourceManager)target;
//             DrawDefaultInspector();
            
//             if (GUILayout.Button("Add 10 diamond")) {
//                 ResourceManager.Instance.AddResource(ResourceType.Diamond, 10);
//             }

//             if (GUILayout.Button("Spend 3 diamond")) {
//                 ResourceManager.Instance.SpendResource(ResourceType.Diamond, 3);
//             }
//         }
//     }
// }