using UnityEngine;
using UnityEditor;
using Framework.Input;

[CustomEditor(typeof(InputHandler))]
public class InputEditor : Editor
{
    InputHandler t;
    SerializedObject GetTarget;
    SerializedProperty singleList;
    SerializedProperty doubleList;
    SerializedProperty axisList;
    int singleListSize;
    int doubleListSize;
    int axisListSize;

    private void OnEnable()
    {
        t = (InputHandler)target;
        GetTarget = new SerializedObject(t);
        singleList = GetTarget.FindProperty("m_singleInputActions");
        doubleList = GetTarget.FindProperty("m_doubleInputActions");
        axisList = GetTarget.FindProperty("m_axisInputActions");
    }

    public override void OnInspectorGUI()
    {
        GetTarget.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Single Input Action"))
        {
            t.AddSingleInputAction();
        }

        if (GUILayout.Button("Add Double Input Action"))
        {
            t.AddDoubleInputAction();
        }

        if (GUILayout.Button("Add Axis Input Action"))
        {
            t.AddAxisInputAction();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        SerializedProperty input = GetTarget.FindProperty("OnInput");
        EditorGUILayout.PropertyField(input);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < singleList.arraySize; i++)
        {

            SerializedProperty MyListRef = singleList.GetArrayElementAtIndex(i);

            SerializedProperty actionID = MyListRef.FindPropertyRelative("m_actionID");
            SerializedProperty buttonPress = MyListRef.FindPropertyRelative("OnButtonPressed");

            EditorGUILayout.PropertyField(actionID);
            EditorGUILayout.PropertyField(buttonPress);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove"))
            {
                singleList.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }

        for (int i = 0; i < doubleList.arraySize; i++)
        {

            SerializedProperty MyListRef = doubleList.GetArrayElementAtIndex(i);

            SerializedProperty actionID = MyListRef.FindPropertyRelative("m_actionID");
            SerializedProperty buttonPress = MyListRef.FindPropertyRelative("OnButtonPressed");
            SerializedProperty buttonRelease = MyListRef.FindPropertyRelative("OnButtonReleased");

            EditorGUILayout.PropertyField(actionID);
            EditorGUILayout.PropertyField(buttonPress);
            EditorGUILayout.PropertyField(buttonRelease);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove"))
            {
                doubleList.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }

        for (int i = 0; i < axisList.arraySize; i++)
        {

            SerializedProperty MyListRef = axisList.GetArrayElementAtIndex(i);

            SerializedProperty actionID = MyListRef.FindPropertyRelative("m_actionID");
            SerializedProperty inputAxis = MyListRef.FindPropertyRelative("m_inputAxis");

            EditorGUILayout.PropertyField(actionID);
            EditorGUILayout.PropertyField(inputAxis);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove"))
            {
                axisList.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }

        GetTarget.ApplyModifiedProperties();
    }
}
