using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

[CustomEditor(typeof(TextureGenerator))]
public class TextureGeneratorEditor : UnityEditor.Editor
{
    private struct Elements
    {
        public ObjectField imageField;
        public Vector2Field dimenionsion;
        public Vector2Field step;
        public Vector2Field speed;
        public Vector2Field offset;
        public ColorField color;
        public Toggle usePerlin;
        public Toggle alpha;
        public Toggle red;
        public Toggle green;
        public Toggle blue;
        public TextField exportName;
        public Button export;
    }



    private static string FOLDER_PATH = "/Textures/";

    private VisualTreeAsset m_VisualTree;
    private TextureGenerator m_Target;
    private Elements m_Elements;

    public void OnEnable()
    {
        m_VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/TextureGenerator/TextureGeneratorEditor.uxml");
    }

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        m_VisualTree.CloneTree(root);

        m_Target = target as TextureGenerator;
        var sObj = new SerializedObject(m_Target);

        m_Elements = GetElements(root);

        InitialiseElements();
        BindElements();



        return root;
    }

    private void Export()
    {
        var path = Application.dataPath + FOLDER_PATH + m_Elements.exportName.value + ".png";
        m_Target.Initialise();

        byte[] bytes = m_Target.texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

    private Elements GetElements(VisualElement root)
    {
        Elements e = new Elements();

        e.imageField = root.Q("ImageField") as ObjectField;
        e.dimenionsion = root.Q("Dimension") as Vector2Field;
        e.step = root.Q("Step") as Vector2Field;
        e.speed = root.Q("Speed") as Vector2Field;
        e.offset = root.Q("Offset") as Vector2Field;
        e.color = root.Q("Color") as ColorField;
        e.usePerlin = root.Q("UsePerlin") as Toggle;
        e.alpha = root.Q("Alpha") as Toggle;
        e.red = root.Q("Red") as Toggle;
        e.green = root.Q("Green") as Toggle;
        e.blue = root.Q("Blue") as Toggle;
        e.exportName = root.Q("ExportName") as TextField;
        e.export = root.Q("Export") as Button;

        return e;
    }

    private void InitialiseElements()
    {
        m_Elements.imageField.objectType = typeof(UnityEngine.UI.Image);
        m_Elements.imageField.value = m_Target.image;

        m_Elements.dimenionsion.value = m_Target.dimension;
        m_Elements.step.value = m_Target.step;
        m_Elements.speed.value = m_Target.speed;
        m_Elements.offset.value = m_Target.offset;
        m_Elements.color.value = m_Target.defaultColor;
        m_Elements.usePerlin.value = m_Target.usePerlin;
        m_Elements.alpha.value = m_Target.alpha;
        m_Elements.red.value = m_Target.red;
        m_Elements.green.value = m_Target.green;
        m_Elements.blue.value = m_Target.blue;
        m_Elements.exportName.value = "";
    }

    private void BindElements()
    {
        m_Elements.imageField.RegisterCallback<ChangeEvent<UnityEngine.Object>>(e => { m_Target.image = (UnityEngine.UI.Image)(e.newValue); });
        m_Elements.dimenionsion.RegisterCallback<ChangeEvent<Vector2>>(e => { m_Target.dimension = e.newValue; });
        m_Elements.step.RegisterCallback<ChangeEvent<Vector2>>(e => { m_Target.step = e.newValue; });
        m_Elements.speed.RegisterCallback<ChangeEvent<Vector2>>(e => { m_Target.speed = e.newValue; });
        m_Elements.offset.RegisterCallback<ChangeEvent<Vector2>>(e => { m_Target.offset = e.newValue; });
        m_Elements.color.RegisterCallback<ChangeEvent<Color>>(e => { m_Target.defaultColor = e.newValue; });
        m_Elements.usePerlin.RegisterCallback<ChangeEvent<bool>>(e => { m_Target.usePerlin = e.newValue; });
        m_Elements.alpha.RegisterCallback<ChangeEvent<bool>>(e => { m_Target.alpha = e.newValue; });
        m_Elements.red.RegisterCallback<ChangeEvent<bool>>(e => { m_Target.red = e.newValue; });
        m_Elements.green.RegisterCallback<ChangeEvent<bool>>(e => { m_Target.green = e.newValue; });
        m_Elements.blue.RegisterCallback<ChangeEvent<bool>>(e => { m_Target.blue = e.newValue; });

        m_Elements.export.clickable.clicked += Export;
    }

}