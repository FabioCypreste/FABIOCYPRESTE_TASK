using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private GameObject visualPanel;
    [SerializeField] private float displayTime = 3f;

    private Coroutine currentCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (visualPanel != null) visualPanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();

        if (visualPanel != null && notificationText != null)
        {
            notificationText.text = message;

            visualPanel.SetActive(true);
            StartCoroutine(HideAfterDelay());
        }
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        if (visualPanel != null)
        {
            visualPanel.SetActive(false);
        }
    }
}