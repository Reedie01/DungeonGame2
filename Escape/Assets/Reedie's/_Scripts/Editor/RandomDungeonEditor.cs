using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractGenerator), true)]

public class RandomDungeonEditor : Editor
{
    AbstractGenerator generator;

    private void Awake()
    {
        generator = (AbstractGenerator)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        // Section with Button for the first generator
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("----", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }

        EditorGUILayout.EndHorizontal();
    }
}
