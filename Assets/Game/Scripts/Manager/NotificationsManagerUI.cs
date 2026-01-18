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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (visualPanel != null) visualPanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (!gameObject.activeInHierarchy)
        {
            visualPanel.SetActive(true);
        }
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(DisplayRoutine(message));
    }

    private IEnumerator DisplayRoutine(string message)
    {
        notificationText.text = message;
        yield return new WaitForSeconds(displayTime);
        visualPanel.SetActive(false);
    }
}