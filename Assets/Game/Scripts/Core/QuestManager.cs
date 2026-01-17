using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public List<Quest> quests = new List<Quest>();
    private QuestUI questUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        questUI = FindFirstObjectByType<QuestUI>();
    }

    public void AddQuest(Quest newQuest)
    {
        if (!quests.Exists(q => q.title == newQuest.title))
        {
            quests.Add(newQuest);
            Debug.Log("New mission received: " + newQuest.title);
            questUI?.UpdateQuestList();
        }
    }

    public void CompleteQuest(string title)
    {
        Quest quest = quests.Find(q => q.title == title);
        if (quest != null && !quest.isCompleted)
        {
            quest.CompleteQuest();
            questUI?.UpdateQuestList();
        }
    }

    public List<Quest> GetActiveQuests()
    {
        return quests.FindAll(quests => !quests.isCompleted);
    }
}

[SerializeField]
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