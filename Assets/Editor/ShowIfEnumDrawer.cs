using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIfEnum = (ShowIfEnumAttribute)attribute;
        string path = GetPath(property, showIfEnum.EnumFieldName);
        SerializedProperty enumProperty = property.serializedObject.FindProperty(path);

        if (enumProperty != null && enumProperty.enumValueIndex == showIfEnum.EnumValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIfEnum = (ShowIfEnumAttribute)attribute;
        string path = GetPath(property, showIfEnum.EnumFieldName);
        SerializedProperty enumProperty = property.serializedObject.FindProperty(path);

        if (enumProperty != null && enumProperty.enumValueIndex == showIfEnum.EnumValue)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        return -EditorGUIUtility.standardVerticalSpacing; // Return negative height to collapse the property
    }

    private string GetPath(SerializedProperty property, string enumFieldName)
    {
        string path = property.propertyPath;

        if (property.propertyType == SerializedPropertyType.Generic)
        {
            path = path.Replace(property.name, enumFieldName);
        }
        else
        {
            int lastIndex = path.LastIndexOf('.');
            if (lastIndex >= 0)
            {
                path = path.Substring(0, lastIndex + 1) + enumFieldName;
            }
        }

        return path;
    }
}

