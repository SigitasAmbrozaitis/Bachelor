
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.SO.Events
{
    [CustomEditor(typeof(DataTypes.Events.Vector2Event))]
    public class Vetor2EventEditor : EventEditor
    {
        protected override VisualElement CreateView()
        {
            var root = base.CreateView();
            var view = base.CreateBaseView();
            root.Add(view);
            return root;
        }
    }
}

