using UnityEngine;
using UnityEditor;
using Framework;

[CustomPropertyDrawer(typeof(HidePropertyAttribute))]
public class HidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        HidePropertyAttribute propertyAttribute = (HidePropertyAttribute)attribute;
        bool wasEnabled = GUI.enabled;

        if (Application.isPlaying)
        {
            GUI.enabled = propertyAttribute.m_runtimeVisibility == VisibilityMode.Editable;
            if (propertyAttribute.m_runtimeVisibility != VisibilityMode.Hidden)
                EditorGUI.PropertyField(_position, _property, _label, true);
        }
        else
        {
            GUI.enabled = propertyAttribute.m_editorVisibility == VisibilityMode.Editable;
            if (propertyAttribute.m_editorVisibility != VisibilityMode.Hidden)
                EditorGUI.PropertyField(_position, _property, _label, true);
        }
        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
    {
        HidePropertyAttribute propertyAttribute = (HidePropertyAttribute)attribute;
        if ((propertyAttribute.m_editorVisibility == VisibilityMode.Hidden && !Application.isPlaying)
            || (propertyAttribute.m_runtimeVisibility == VisibilityMode.Hidden && Application.isPlaying))
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
        else
        {
            return base.GetPropertyHeight(_property, _label);
        }
    }
}
