using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager QuestManagerInstance { get; private set; }
    public List<Quest> quests = new List<Quest>();
    private QuestUI questUI;

    private void Awake()
    {
        if (QuestManagerInstance != null && QuestManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        QuestManagerInstance = this;
        DontDestroyOnLoad(gameObject);

        questUI = FindFirstObjectByType<QuestUI>();
    }

    public void AddQuest(Quest newQuest)
    {
        if (!quests.Exists(q => q.title == newQuest.title))
        {
            quests.Add(newQuest);
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
        return quests.FindAll(q => !q.isCompleted);
    }
}

[Serializable]
public class Quest
{
    public string title;
    public string description;
    public bool isCompleted;

    public Quest(string title, string description)
    {
        this.title = title;
        this.description = description;
        this.isCompleted = false;
    }

    public void CompleteQuest()
    {
        isCompleted = true;
        Debug.Log($"Quest Completed: {title}");
    }
}