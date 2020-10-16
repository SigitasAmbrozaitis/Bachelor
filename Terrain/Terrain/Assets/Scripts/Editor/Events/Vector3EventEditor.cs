
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.SO.Events
{
    [CustomEditor(typeof(DataTypes.Events.Vector3Event))]
    public class Vector3EventEditor : EventEditor
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
