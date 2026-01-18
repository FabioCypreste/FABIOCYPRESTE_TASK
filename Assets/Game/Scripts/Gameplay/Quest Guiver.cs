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
            Debug.Log($"NPC: {dialogComplete}");
            return;
        }

        if (!isQuestActive)
        {
            GiveQuest();

            CheckForCompletion();
            return;
        }
        CheckForCompletion();

        void GiveQuest()
        {
            if (QuestManager.QuestManagerInstance == null)
            {
                Debug.LogError("QuestManagerInstance is null");
                return;
            }
            Quest newQuest = new Quest(questTitle, questDescription);
            QuestManager.QuestManagerInstance.AddQuest(newQuest);
            isQuestActive = true;
            Debug.Log($"NPC: Please, find {requiredItem.ItemName} for me.");
        }
    }

    private void CheckForCompletion()
    {
        if (InventoryManager.InventoryManagerInstance.HasItem(requiredItem))
        {
            InventoryManager.InventoryManagerInstance.RemoveItem(requiredItem, 1);
            if (QuestManager.QuestManagerInstance != null)
            {
                QuestManager.QuestManagerInstance.CompleteQuest(questTitle);
            }

            isQuestCompleted = true;
            Debug.Log($"NPC: {dialogComplete}");
        }
        else
        {
            Debug.Log($"NPC: You didn't find {requiredItem.ItemName}...");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press F To interact");
        }
    }
}