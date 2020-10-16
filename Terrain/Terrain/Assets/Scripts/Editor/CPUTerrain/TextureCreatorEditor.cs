//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//using UnityEngine.UIElements;
//using UnityEditor.UIElements;

//using CPU;

//namespace Editor.CPU
//{
//    [CustomEditor(typeof(TextureCreator))]
//    public class TextureCreatorEditor : UnityEditor.Editor
//    {
//        private VisualTreeAsset m_Tree = default;
//        private TextureCreator m_Target = default; 

//        private void OnEnable()
//        {
//            var path = "Assets/Scripts/Editor/CPUTerrain/TextureCreatorEditor.uxml";
//            m_Tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
//        }


//        public override VisualElement CreateInspectorGUI()
//        {
//            var root = new VisualElement();
//            m_Tree.CloneTree(root);

//            root.Bind(serializedObject);


//            m_Target = target as TextureCreator;

//            return root;

//        }
//    }

//}
