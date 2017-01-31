using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Quest myQuest = (Quest)target;
        if (GUILayout.Button("Build Object"))
        {
            //QuestObjective objective = ScriptableObject.CreateInstance<QuestObjective>();
            //AssetDatabase.CreateAsset(objective, "Assets/Quest/NewScripableObject.asset");
            //
            //AssetDatabase.AddObjectToAsset(objective, myQuest);

            var newItem = CreateInstance<QuestObjective>();

            AssetDatabase.AddObjectToAsset(newItem, target);
            AssetDatabase.SaveAssets();

            myQuest.objectives.Add(newItem);

        }
    }
}

[CustomEditor(typeof(QuestObjective))]
public class QuestObjectiveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestObjective myQuest = (QuestObjective)target;
        if (GUILayout.Button("Build Object"))
        {
            //QuestObjective objective = ScriptableObject.CreateInstance<QuestObjective>();
            //AssetDatabase.CreateAsset(objective, "Assets/Quest/NewScripableObject.asset");
            //
            //AssetDatabase.AddObjectToAsset(objective, myQuest);

            var newItem = CreateInstance<QuestReward>();

            AssetDatabase.AddObjectToAsset(newItem, target);
            AssetDatabase.SaveAssets();

            myQuest.rewards.Add(newItem);

        }
    }
}


[CustomEditor(typeof(QuestReward))]
public class QuestRewardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}