using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public Text mainQuestTitle;
    public Text mainQuestDescription;

    [System.Serializable]
    public struct Quest
    {
        public string title;
        public string description;
        public QuestRewards questRewards;
        public float goal;

        [HideInInspector] public bool isCompleted;
        [HideInInspector] public float currentProgress;
    }

    [System.Serializable]
    public struct QuestRewards
    {
        public Valuables currency;
        public int amount;
    }

    public Quest[] mainQuests;
    private int currentMainQuestIndex = 0;


    private void Start()
    {
        UpdateMainQuestText();
    }


    public void ProgressMainQuest(float progress)
    {
        if (currentMainQuestIndex < mainQuests.Length)
        {
            Quest currentQuest = mainQuests[currentMainQuestIndex];
            currentQuest.currentProgress += progress;
            if (currentQuest.currentProgress >= currentQuest.goal)
            {
                CompleteQuest(ref currentQuest);
                currentMainQuestIndex++;
                UpdateMainQuestText();
            }
            mainQuests[currentMainQuestIndex] = currentQuest;
        }
    }



    private void UpdateMainQuestText()
    {
        if (currentMainQuestIndex < mainQuests.Length)
        {
            mainQuestTitle.text = mainQuests[currentMainQuestIndex].title;
            mainQuestDescription.text = mainQuests[currentMainQuestIndex].description;
        }
    }

    private void CompleteQuest(ref Quest quest)
    {
        quest.isCompleted = true;
        Debug.Log(quest.description + " completed! Reward: " + quest.questRewards.ToString());
        // Implement reward logic here
    }
}