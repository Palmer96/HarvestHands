using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Quest myQuest = (Quest)target;
        //Add button to inspector
        if (GUILayout.Button("Build Object"))
        {
            //Create new instance of object
            var newItem = CreateInstance<QuestObjective>();
            //Adds new object as child
            AssetDatabase.AddObjectToAsset(newItem, myQuest);

            myQuest.objectives.Add(newItem);
            //Rename object (asset)
            myQuest.objectives[myQuest.objectives.Count - 1].name = myQuest.name + "_" + "Objective" + (myQuest.objectives.Count - 1).ToString() + "_" + "test";
            //Save changes
            AssetDatabase.SaveAssets();
        }

        //Add button to inspector
        if (GUILayout.Button("Add Talk To Objective"))
        {
            //Create new instance of object
            var newItem = CreateInstance<TalkObjective>();
            //Adds new object as child
            AssetDatabase.AddObjectToAsset(newItem, myQuest);

            myQuest.objectives.Add(newItem);
            //Rename object (asset)
            myQuest.objectives[myQuest.objectives.Count - 1].name = myQuest.name + "_" + "Objective" + (myQuest.objectives.Count - 1).ToString() + "_" + "Talk";
            //Save changes
            AssetDatabase.SaveAssets();
        }
        //Add button to inspector
        if (GUILayout.Button("Add Harvest Objective"))
        {
            //Create new instance of object
            var newItem = CreateInstance<HarvestObjective>();
            //Adds new object as child
            AssetDatabase.AddObjectToAsset(newItem, myQuest);

            myQuest.objectives.Add(newItem);
            //Rename object (asset)
            myQuest.objectives[myQuest.objectives.Count - 1].name = myQuest.name + "_" + "Objective" + (myQuest.objectives.Count - 1).ToString() + "_" + "Harvest";
            //Save changes
            AssetDatabase.SaveAssets();
        }

        //Add button to inspector
        if (GUILayout.Button("Add Sell Objective"))
        {
            //Create new instance of object
            var newItem = CreateInstance<SellObjective>();
            //Adds new object as child
            AssetDatabase.AddObjectToAsset(newItem, myQuest);

            myQuest.objectives.Add(newItem);
            //Rename object (asset)
            myQuest.objectives[myQuest.objectives.Count - 1].name = myQuest.name + "_" + "Objective" + (myQuest.objectives.Count - 1).ToString() + "_" + "Sell";
            //Save changes
            AssetDatabase.SaveAssets();
        }

        //Add button to inspector
        if (GUILayout.Button("Add Plant Objective"))
        {
            //Create new instance of object
            var newItem = CreateInstance<PlantObjective>();
            //Adds new object as child
            AssetDatabase.AddObjectToAsset(newItem, myQuest);

            myQuest.objectives.Add(newItem);
            //Rename object (asset)
            myQuest.objectives[myQuest.objectives.Count - 1].name = myQuest.name + "_" + "Plant" + (myQuest.objectives.Count - 1).ToString() + "_" + "Sell";
            //Save changes
            AssetDatabase.SaveAssets();
        }

        //Add button to inspector
        if (GUILayout.Button("Add Money Reward"))
        {
            //Create new instance of object
            var newItem = CreateInstance<MoneyReward>();
            //Adds new object as child
            AssetDatabase.AddObjectToAsset(newItem, myQuest);

            myQuest.rewards.Add(newItem);
            //Rename object (asset)
            myQuest.rewards[myQuest.rewards.Count - 1].name = myQuest.name + "_" + "Reward" + (myQuest.rewards.Count - 1).ToString() + "_" + "Money";
            //Save changes
            AssetDatabase.SaveAssets();
        }
        //Add button to inspector
        if (GUILayout.Button("Add Item Reward"))
        {
            //Create new instance of object
            var newItem = CreateInstance<ItemReward>();
            //Adds new object as child
            AssetDatabase.AddObjectToAsset(newItem, myQuest);

            myQuest.rewards.Add(newItem);
            //Rename object (asset)
            myQuest.rewards[myQuest.rewards.Count - 1].name = myQuest.name + "_" + "Reward" + (myQuest.rewards.Count - 1).ToString() + "_" + "Item";
            //Save changes
            AssetDatabase.SaveAssets();
        }

    }
}

[CustomEditor(typeof(TalkObjective))]
public class TalkObjectiveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        QuestObjective myQuest = (QuestObjective)target;
        if (GUILayout.Button("Delete This Objective"))
        {
            string questPath = AssetDatabase.GetAssetPath(myQuest.GetInstanceID());
            Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            quest.objectives.Remove(myQuest);
            UnityEditor.Selection.activeObject = quest;
            //Reselect Quest
            Selection.objects = new Object[] { quest };
            //Delete object
            UnityEngine.Object.DestroyImmediate(myQuest, true);
            AssetDatabase.SaveAssets();
        }
    }
}

[CustomEditor(typeof(HarvestObjective))]
public class HarvestObjectiveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        QuestObjective myQuest = (QuestObjective)target;
        if (GUILayout.Button("Delete This Objective"))
        {
            string questPath = AssetDatabase.GetAssetPath(myQuest.GetInstanceID());
            Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            quest.objectives.Remove(myQuest);
            UnityEditor.Selection.activeObject = quest;
            //Reselect Quest
            Selection.objects = new Object[] { quest };
            //Delete object
            UnityEngine.Object.DestroyImmediate(myQuest, true);
            AssetDatabase.SaveAssets();
        }
    }
}

[CustomEditor(typeof(SellObjective))]
public class SellObjectiveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        QuestObjective myQuest = (QuestObjective)target;
        if (GUILayout.Button("Delete This Objective"))
        {
            string questPath = AssetDatabase.GetAssetPath(myQuest.GetInstanceID());
            Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            quest.objectives.Remove(myQuest);
            UnityEditor.Selection.activeObject = quest;
            //Reselect Quest
            Selection.objects = new Object[] { quest };
            //Delete object
            UnityEngine.Object.DestroyImmediate(myQuest, true);
            AssetDatabase.SaveAssets();
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
        if (GUILayout.Button("Delete This Objective"))
        {
            string questPath = AssetDatabase.GetAssetPath(myQuest.GetInstanceID());
            Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            quest.objectives.Remove(myQuest);
            UnityEditor.Selection.activeObject = quest;            
            //Reselect Quest
            Selection.objects = new Object[] { quest };
            //Delete object
            UnityEngine.Object.DestroyImmediate(myQuest, true);
            AssetDatabase.SaveAssets();
        }
    }
}


[CustomEditor(typeof(QuestReward))]
public class QuestRewardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        QuestObjective myQuest = (QuestObjective)target;
        if (GUILayout.Button("Delete This Reward"))
        {
            string questPath = AssetDatabase.GetAssetPath(myQuest.GetInstanceID());
            Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            quest.objectives.Remove(myQuest);
            UnityEditor.Selection.activeObject = quest;
            //Reselect Quest
            Selection.objects = new Object[] { quest };
            //Delete object
            UnityEngine.Object.DestroyImmediate(myQuest, true);
            AssetDatabase.SaveAssets();
        }
    }
}

[CustomEditor(typeof(MoneyReward))]
public class MoneyRewardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        QuestReward myQuest = (QuestReward)target;
        if (GUILayout.Button("Delete This Reward"))
        {
            string questPath = AssetDatabase.GetAssetPath(myQuest.GetInstanceID());
            Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            quest.rewards.Remove(myQuest);
            UnityEditor.Selection.activeObject = quest;
            //Reselect Quest
            Selection.objects = new Object[] { quest };
            //Delete object
            UnityEngine.Object.DestroyImmediate(myQuest, true);
            AssetDatabase.SaveAssets();
        }
    }
}

[CustomEditor(typeof(ItemReward))]
public class ItemRewardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        QuestReward myQuest = (QuestReward)target;
        if (GUILayout.Button("Delete This Reward"))
        {
            string questPath = AssetDatabase.GetAssetPath(myQuest.GetInstanceID());
            Quest quest = AssetDatabase.LoadAssetAtPath<Quest>(questPath);
            quest.rewards.Remove(myQuest);
            UnityEditor.Selection.activeObject = quest;
            //Reselect Quest
            Selection.objects = new Object[] { quest };
            //Delete object
            UnityEngine.Object.DestroyImmediate(myQuest, true);
            AssetDatabase.SaveAssets();
        }
    }
}

#endif