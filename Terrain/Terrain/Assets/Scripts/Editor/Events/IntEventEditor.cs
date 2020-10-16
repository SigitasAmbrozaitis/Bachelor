
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.SO.Events
{
    [CustomEditor(typeof(DataTypes.Events.IntEvent))]
    public class IntEventEditor : EventEditor
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
