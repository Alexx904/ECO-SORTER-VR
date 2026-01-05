using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Gestisce il Menu di Pausa durante la partita.
/// Si occupa di fermare il tempo (TimeScale), mostrare/nascondere l'interfaccia e gestire la navigazione nei sottomenu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Variabile statica globale per sapere se il gioco è attualmente in pausa.
    /// Utile per bloccare input o altri eventi in altri script.
    /// </summary>
    public static bool GameIsPaused = false;

    [Header("Riferimenti Pannelli UI")]
    [Tooltip("Il pannello principale del menu di pausa.")]
    public GameObject pauseMenuUI;
    [Tooltip("Il pannello delle impostazioni.")]
    public GameObject settingsUI; 
    [Tooltip("L'interfaccia di gioco (HUD) da nascondere quando si apre la pausa.")]
    public GameObject gameHUD;    

    [Header("Statistiche nel Menu")]
    public TextMeshProUGUI testoPunteggio; 
    public TextMeshProUGUI testoTempo;     

    [Header("Riferimenti Esterni")]
    [Tooltip("Riferimento al LevelManager per leggere il tempo rimanente.")]
    public LevelManager levelManager; 

    /// <summary>
    /// Alterna lo stato del gioco tra Pausa e Ripresa.
    /// Questa funzione viene solitamente chiamata dal PlayerInteractionController alla pressione del tasto ESC/Start.
    /// </summary>
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

    /// <summary>
    /// Riprende la partita: nasconde il menu, riattiva l'HUD, sblocca il tempo e blocca il cursore.
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if(settingsUI != null) settingsUI.SetActive(false);
        if(gameHUD != null) gameHUD.SetActive(true);

        Time.timeScale = 1f; 
        GameIsPaused = false;
        
        // Blocca il cursore al centro per il gameplay FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Mette in pausa: ferma il tempo, mostra il menu, sblocca il cursore e aggiorna i testi informativi.
    /// </summary>
    public void Pause()
    {
        AggiornaDatiPausa(); 

        pauseMenuUI.SetActive(true);
        
        // Imposta il focus del controller sul tasto "Riprendi"
        if (GameMenuController.instance != null) 
        {
            GameMenuController.instance.FocusPausa();
        }
        
        if(gameHUD != null) gameHUD.SetActive(false);

        // Congela il tempo di gioco
        Time.timeScale = 0f; 
        GameIsPaused = true;
        
        // Mostra il cursore per navigare nei menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Recupera i dati aggiornati (Punteggio e Tempo) dai manager e li mostra nei testi del menu di pausa.
    /// </summary>
    void AggiornaDatiPausa()
    {
        if (ScoreManager.instance != null && testoPunteggio != null)
        {
            // Usa i tag Rich Text <size> per ingrandire solo il numero
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

    /// <summary>
    /// Ricarica la scena corrente per ricominciare il livello da capo.
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Importante: ripristinare il tempo prima di ricaricare
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Torna al Menu Principale.
    /// </summary>
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }

    /// <summary>
    /// Chiude il pannello di pausa principale e apre quello delle impostazioni.
    /// </summary>
    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        if(settingsUI != null) settingsUI.SetActive(true);
        
        // Sposta il focus del controller sulle impostazioni
        if (GameMenuController.instance != null) GameMenuController.instance.FocusSettings();
    }

    /// <summary>
    /// Chiude le impostazioni e torna al menu di pausa.
    /// </summary>
    public void CloseSettings()
    {
        if(settingsUI != null) settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        
        // Riporta il focus sul menu di pausa
        if (GameMenuController.instance != null) GameMenuController.instance.FocusPausa();
    }

    /// <summary>
    /// Chiude l'applicazione (Funziona solo nella Build finale).
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}