using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(Benchmark))]
public class BenchmarkEditor : UnityEditor.Editor
{
    private VisualTreeAsset m_VisualTree;
    private Benchmark m_Target;

    public void OnEnable()
    {
        m_VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/BenchmarkEditor/BenchmarkEditor.uxml");
    }


    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        m_VisualTree.CloneTree(root);

        m_Target = target as Benchmark;
        var sObj = new SerializedObject(m_Target);
        

        var generator = root.Q("TextureGenerator") as ObjectField;
        generator.objectType = typeof(TextureGenerator);

        var perlinData = root.Q("PerlinBenchmark") as ObjectField;
        perlinData.objectType = typeof(BenchmarkData);

        var perlinDataByte = root.Q("PerlinByteBenchmark") as ObjectField;
        perlinDataByte.objectType = typeof(BenchmarkData);

        var simplexData = root.Q("SimplexBenchmark") as ObjectField;
        simplexData.objectType = typeof(BenchmarkData);

        var noiseButton = root.Q("NoiseButton") as Button;
        noiseButton.clickable.clicked += ()=>{ m_Target.BenchmarkNoise(); };

        var textureButton = root.Q("TextureButton") as Button;
        textureButton.clickable.clicked += () => { m_Target.BenchamarkTextureGenerator(); };
        root.Bind(sObj);
        return root;
    }
}