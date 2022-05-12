
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class  MenuHandler : MonoBehaviour
{
    public static bool isGamePaused = false;
    [SerializeField] Player player;
    [SerializeField] Inventory inventory;
    [SerializeField] CraftingBench craftingBench;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deadMenu;
    [SerializeField] TextMeshProUGUI surviveTimerText;
  

    private void Start()
    {
       
    }
    void Update()
    {
       
        if (player.PressedPause) // just make a reference to the player, use setter getter function, do not use static, disable controls as well and it should be perfect 
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (player.IsDead())
        {
            EndGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        player.EnableInputs();
        Time.timeScale = 1f;
        isGamePaused = false;

    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        inventory.ToggleInventoryMenu(false);
        craftingBench.ToggleCraftingMenu(false);
        player.DisableInputs();
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void EndGame()
    {
        

        player.DisableInputs();
        deadMenu.SetActive(true);
        surviveTimerText.text = "You Survived For: " + player.GetTimeAlive();
        inventory.ToggleInventoryMenu(false);
        craftingBench.ToggleCraftingMenu(false);
        Time.timeScale = 0f;

    }

    public void QuitToMenu()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(0);
        player.EnableInputs();
        player.SetPlayerDead(false);
        deadMenu.SetActive(false);
        Time.timeScale = 1f;
        
    }
}
