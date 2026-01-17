using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    private List<Quest> quests = new List<Quest>();

    private QuestUI questUI;

    private void Awake()
    {
        instance = this;
        questUI = FindFirstObjectByType<QuestUI>();
    }

    public void AddQuest(Quest newQuest)
    {
        if (!quests.Exists(q => q.title == newQuest.title))
        {
            quests.Add(newQuest);
            Debug.Log("New mission received: " + newQuest.title);

            if (questUI != null) questUI.UpdateQuestList();
        }
    }

    public void CompleteQuest(string title)
    {
        Quest quest = quests.Find(q => q.title == title);
        if (quest != null && !quest.isCompleted)
        {
            quest.CompleteQuest();
            if (questUI != null) questUI.UpdateQuestList();
        }
    }

    public List<Quest> GetActiveQuests()
    {
        return quests.FindAll(quests => !quests.isCompleted);
    }
}
