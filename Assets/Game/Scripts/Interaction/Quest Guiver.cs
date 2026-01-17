using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public string questTitle;
    public string questDescription;

    private bool playerIsClose = false;
    private bool questGiven = false;

    private void Update()
    {
        if (playerIsClose && Input.GetKeyDown(KeyCode.F) && !questGiven)
        {
            GiveQuest();
        }
    }
    void GiveQuest()
    {
        Quest newQuest = new Quest(questTitle, questDescription);

        QuestManager.instance.AddQuest(newQuest);

        questGiven = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            Debug.Log("Press F to accept the mission");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }
}