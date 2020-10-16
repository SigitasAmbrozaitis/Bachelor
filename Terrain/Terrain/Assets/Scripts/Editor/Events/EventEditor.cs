using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.UIElements;
using UnityEditor.UIElements;

using DataTypes.Events;
using UnityEditor.MemoryProfiler;

namespace Editor.SO.Events
{
    [CustomEditor(typeof(DataTypes.Events.Event))]
    public class EventEditor : UnityEditor.Editor
    {
        protected DataTypes.Events.Event m_Target = default;

        private VisualTreeAsset m_Tree = default;
        private VisualTreeAsset m_BaseTree = default;
        private StyleSheet m_Style = default;

        public void OnEnable()
        {
            Initialize();
        }

        public void OnDisable()
        {

        }

        public override VisualElement CreateInspectorGUI()
        {
            return CreateView();
        }

        protected void Initialize()
        {
            var uxmlPath = "Assets/Scripts/Editor/Events/EventEditor.uxml";
            m_Tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);

            var basePAth = "Assets/Scripts/Editor/Events/ExtendedEventEditor.uxml";
            m_BaseTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(basePAth);

            var stylePath = "Assets/Scripts/Editor/Events/EventStyle.uss";
            m_Style = AssetDatabase.LoadAssetAtPath<StyleSheet>(stylePath);

            m_Target = target as DataTypes.Events.Event;
        }

        protected virtual VisualElement CreateView()
        {
            var root = new VisualElement();
            m_Tree.CloneTree(root);

            root.styleSheets.Add(m_Style);

            root.Bind(serializedObject);
            root.Q<Button>("Raise").clickable.clicked += m_Target.Raise;

            return root;
        }

        protected VisualElement CreateBaseView()
        {
            var root = new VisualElement();
            m_BaseTree.CloneTree(root);

            root.Unbind();
            root.Bind(serializedObject);

            return root;
        }
    }
}
