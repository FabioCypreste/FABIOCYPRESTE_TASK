using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void NewGame()
    {
        if (InventoryManager.InventoryManagerInstance != null)
        {
            InventoryManager.InventoryManagerInstance.ClearInventory();
        }
        else
        {
            PlayerPrefs.DeleteKey("InventorySaveData");
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}