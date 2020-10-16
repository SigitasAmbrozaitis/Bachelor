
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.SO.Events
{
    [CustomEditor(typeof(DataTypes.Events.StringEvent))]
    public class StringEventEditor : EventEditor
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
