using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : UnityEditor.Editor
{
    private VisualTreeAsset m_VisualTree;
    private Generator m_Target;

    public void OnEnable()
    {
        m_VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Generator/GeneratorEditor.uxml");
    }
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        m_VisualTree.CloneTree(root);

        m_Target = target as Generator;
        var sObj = new SerializedObject(m_Target);

        var button = root.Q("GenerateButton") as Button;
        button.clickable.clicked += OnGenerate;

        var objectField = root.Q("PrefabField") as ObjectField;
        objectField.objectType = typeof(GameObject);


        root.Bind(sObj);

        return root;
    }

    public void OnGenerate()
    {
        m_Target.Generate();
    }
}