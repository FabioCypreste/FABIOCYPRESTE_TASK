using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI questListText;
    private QuestManager questManager;

    void Start()
    {
        questManager = Object.FindFirstObjectByType<QuestManager>();
        UpdateQuestList();
    }

    public void UpdateQuestList()
    {
        questListText.text = "Quests:\n";
        foreach (Quest quest in questManager.GetActiveQuests())
        {
            questListText.text += "- " + quest.title + ": " + quest.description + "\n";
        }
    }
}