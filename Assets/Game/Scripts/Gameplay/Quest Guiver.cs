using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public string questTitle;
    [TextArea] public string questDescription;
    public ItemData requiredItem;

    private bool isQuestActive = false;
    private bool isQuestCompleted = false;
    private string dialogComplete = "Thank You!";

    public void Interact()
    {
        if (isQuestCompleted)
        {
            NotificationUI.Instance.ShowMessage($"NPC: {dialogComplete}");
            return;
        }

        if (!isQuestActive)
        {
            GiveQuest();
            return;
        }

        CheckForCompletion();
    }

    private void GiveQuest()
    {
        if (QuestManager.QuestManagerInstance == null) return;

        Quest newQuest = new Quest(questTitle, questDescription);
        QuestManager.QuestManagerInstance.AddQuest(newQuest);
        isQuestActive = true;
        NotificationUI.Instance.ShowMessage($"NPC: Please, find {requiredItem.ItemName} for me.");
    }

    private void CheckForCompletion()
    {
        if (InventoryManager.InventoryManagerInstance.HasItem(requiredItem))
        {
            InventoryManager.InventoryManagerInstance.RemoveItem(requiredItem, 1);
            QuestManager.QuestManagerInstance?.CompleteQuest(questTitle);

            isQuestCompleted = true;
            NotificationUI.Instance.ShowMessage($"NPC: {dialogComplete}");
        }
        else
        {
            NotificationUI.Instance.ShowMessage($"NPC: You didn't find {requiredItem.ItemName}...");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationUI.Instance.ShowMessage("Press F to interact");
        }
    }
}