using UnityEngine;

namespace Framework
{
    public enum VisibilityMode
    {
        Editable,
        ReadOnly,
        Hidden
    }

    public class HidePropertyAttribute : PropertyAttribute
    {
        public readonly VisibilityMode m_editorVisibility;
        public readonly VisibilityMode m_runtimeVisibility;

        public HidePropertyAttribute(VisibilityMode _editorVisibility = VisibilityMode.Hidden, VisibilityMode _runtimeVisisbility = VisibilityMode.Hidden)
        {
            m_editorVisibility = _editorVisibility;
            m_runtimeVisibility = _runtimeVisisbility;
        }
    }
}