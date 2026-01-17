using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public string questTitle;
    public string questDescription;
    public ItemData requiredItem;
    private bool isPlayerClose = false;
    private bool isQuestActive = false;
    private string dialogComplete = "Thank you!";

    public void Interact()
    {
        if (isQuestActive)
        {
            Debug.Log($"NPC: {dialogComplete}");
            return;
        }

        if (!isQuestActive)
        {
            GiveQuest();
        }
        else
        {
            CheckForCompletion();
        }
    }
    void GiveQuest()
    {
        Quest newQuest = new Quest(questTitle, questDescription);
        QuestManager.QuestManagerInstance.AddQuest(newQuest);
        isQuestActive = true;
        Debug.Log("NPC: Please, find the item for me");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
            Debug.Log("Press F to accept the mission");
        }
    }

    private void CheckForCompletion()
    {
        if (InventoryManager.InventoryManagerInstance.HasItem(requiredItem))
        {
            QuestManager.QuestManagerInstance.CompleteQuest(questTitle);
            isQuestActive = true;
            Debug.Log($"NPC: {dialogComplete}");
        }
        else
        {
            Debug.Log($"NPC: Have you found it yet?");
        }
    }
}