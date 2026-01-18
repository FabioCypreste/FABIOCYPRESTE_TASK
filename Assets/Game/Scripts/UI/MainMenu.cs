using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //The code below has been created by me in another project!
    [SerializeField] private string gameSceneName = "Village";

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
        if (InventoryManager.InventoryManagerInstance != null) InventoryManager.InventoryManagerInstance.SaveInventory();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}