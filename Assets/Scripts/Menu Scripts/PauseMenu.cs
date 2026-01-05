using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("Riferimenti Pannelli")]
    public GameObject pauseMenuUI;
    public GameObject settingsUI; 
    public GameObject gameHUD;    

    [Header("Riferimenti Testi Pausa")]
    public TextMeshProUGUI testoPunteggio; 
    public TextMeshProUGUI testoTempo;     

    [Header("Riferimento Logic")]
    public LevelManager levelManager; 

    // --- NUOVA FUNZIONE: Questa verrà chiamata dal PlayerInteractionController ---
    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    // --------------------------------------------------------------------------

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if(settingsUI != null) settingsUI.SetActive(false);
        if(gameHUD != null) gameHUD.SetActive(true);

        Time.timeScale = 1f; 
        GameIsPaused = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Ora è PUBLIC così possiamo chiamarla se serve
    public void Pause()
    {
        AggiornaDatiPausa(); 

        pauseMenuUI.SetActive(true);
        
        // Questo serve per evidenziare il bottone col controller
        if (GameMenuController.instance != null) 
        {
            GameMenuController.instance.FocusPausa();
        }
        
        if(gameHUD != null) gameHUD.SetActive(false);

        Time.timeScale = 0f; 
        GameIsPaused = true;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void AggiornaDatiPausa()
    {
        if (ScoreManager.instance != null && testoPunteggio != null)
        {
            testoPunteggio.text = "Punteggio: <size=120%>" + ScoreManager.instance.GetPunteggio() + "</size>";
        }

        if (levelManager != null && testoTempo != null)
        {
            float tempo = levelManager.tempoRimanente;
            int minuti = Mathf.FloorToInt(tempo / 60);
            int secondi = Mathf.FloorToInt(tempo % 60);
            testoTempo.text = string.Format("Tempo rimanente: <size=120%>{0:00}:{1:00}</size>", minuti, secondi);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        if(settingsUI != null) settingsUI.SetActive(true);
        if (GameMenuController.instance != null) GameMenuController.instance.FocusSettings();
    }

    public void CloseSettings()
    {
        if(settingsUI != null) settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        if (GameMenuController.instance != null) GameMenuController.instance.FocusPausa();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}