using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Framework;

[CustomPropertyDrawer(typeof(HideMultiPropertyAttribute))]
public class HideMultiPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        HideMultiPropertyAttribute propertyAttribute = (HideMultiPropertyAttribute)attribute;
        bool enabled = GetHidePropertyResult(propertyAttribute, _property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!propertyAttribute.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(_position, _property, _label, true);
        }

        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
    {
        HideMultiPropertyAttribute propertyAttribute = (HideMultiPropertyAttribute)attribute;
        bool enabled = GetHidePropertyResult(propertyAttribute, _property);

        if (!propertyAttribute.HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(_property, _label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetHidePropertyResult(HideMultiPropertyAttribute _propertyAttribute, SerializedProperty _property)
    {
        bool? enabled = true;
        string propertyPath = _property.propertyPath; //returns the property path of the property the attribute will be applied to
        string conditionPath = propertyPath.Replace(_property.name, _propertyAttribute.ConditionalSourceField); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = _property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {

            string[] enumNames = sourcePropertyValue.enumNames;
            if (enumNames.Length > 0)
            {
                if (enumNames[sourcePropertyValue.enumValueIndex] != _propertyAttribute.EnumValue)
                {
                    enabled = false; 
                }   
            }
            else
            {
                enabled = sourcePropertyValue.boolValue;
            }
        }
        else
        {
            Debug.LogWarning("Attempting to use a HidePropertyAttribute but no matching SourcePropertyValue found in object: " + _propertyAttribute.ConditionalSourceField);
        }

        return enabled.Value;
    }
}
