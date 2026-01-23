using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGeneratorController))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGeneratorController controller = (LevelGeneratorController)target;



        GUILayout.Label("\nGame Levels", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("<", new GUILayoutOption[] { GUILayout.MinWidth(15f), GUILayout.MaxWidth(45f) }))
        {
            controller.SwitchSelectedLevel(false);
        }

        int GUIValue = EditorGUILayout.IntField("", controller.SelectedLevelIndex, new GUILayoutOption[] { GUILayout.MinWidth(15f), GUILayout.MaxWidth(30f) });
        if(GUIValue != controller.SelectedLevelIndex)
        {
            controller.SwitchSelectedLevel(GUIValue);
        }

        GUI.enabled = false;
        controller.SelectedLevel = EditorGUILayout.ObjectField("", controller.SelectedLevel, typeof(LevelSettings), false, GUILayout.MinWidth(30f)) as LevelSettings;
        
        GUI.enabled = controller.SelectedLevel != controller.LevelToGenerate;

        if (GUILayout.Button("Load", new GUILayoutOption[] { GUILayout.MinWidth(15f), GUILayout.MaxWidth(45f) }))
        {
            controller.SelectedLevelToGenerate();
        }
        GUI.enabled = true;

        if (GUILayout.Button(">", new GUILayoutOption[] { GUILayout.MinWidth(15f), GUILayout.MaxWidth(45f) }))
        {
            controller.SwitchSelectedLevel(true);
        }

        GUILayout.EndHorizontal();



        GUILayout.Label("\nGenerator", EditorStyles.boldLabel);

        controller.LevelToGenerate = EditorGUILayout.ObjectField("Level to Generate", controller.LevelToGenerate, typeof(LevelSettings), false) as LevelSettings;

        GUILayout.BeginHorizontal();
        
        GUI.enabled = controller.CheckForLevelToClear();
        if (GUILayout.Button("Clear"))
        {
            controller.ClearLevel();
        }

        GUI.enabled = controller.CheckForLevelToClear();
        if (GUILayout.Button("Regenerate"))
        {
            if (!controller.RegenerateLevel())
            {
                EditorUtility.DisplayDialog("Meaningless action?", "Generated Level has 'Keep Room Order' enabled.\n\nThe level was regenerated but room positions are the same.", "Ok");
            }
        }

        GUI.enabled = controller.CheckForLevelToGenerate() && (!controller.CheckGeneratedVsToGenerateLevels() || !controller.CheckForLevelToClear());
        if (GUILayout.Button("Generate"))
        {
            controller.GenerateLevel();
        }
        
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }
}
