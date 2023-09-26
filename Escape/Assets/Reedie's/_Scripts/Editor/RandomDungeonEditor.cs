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
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
