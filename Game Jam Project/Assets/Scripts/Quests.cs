using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Create Quests")]
public class Quests : ScriptableObject
{
    public List<QuestList> quests = new List<QuestList>();

    [System.Serializable]
    public struct QuestList
    {
        [Range(1, 5)]
        public int difficulty;
        public List<Quest> questsList;
    }

    [System.Serializable]
    public struct Quest
    {
        public BuildingSO buildingType;
        public float numberOfBuildings;
    }
}
