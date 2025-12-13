using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necessario per modificare le scritte UI

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("Riferimenti Pannelli")]
    public GameObject pauseMenuUI;
    public GameObject settingsUI; // Opzionale
    public GameObject gameHUD;    // L'interfaccia di gioco da nascondere (Timer/Punti in gioco)

    [Header("Riferimenti Testi Pausa")]
    public TextMeshProUGUI testoPunteggio; // La scritta "Punteggio: ..." nel menu pausa
    public TextMeshProUGUI testoTempo;     // La scritta "Tempo Rimanente: ..." nel menu pausa

    [Header("Riferimento Logic")]
    public LevelManager levelManager; // Trascina qui l'oggetto che ha lo script LevelManager

    void Start()
    {
        // Al via assicuriamoci che il menu sia chiuso e il tempo scorra
        Resume(); 
    }

    void Update()
    {
        // Tasto ESC o pulsante Menu (su controller VR è diverso, ma per test PC usa ESC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if(settingsUI != null) settingsUI.SetActive(false);
        
        // Riaccendi l'HUD di gioco
        if(gameHUD != null) gameHUD.SetActive(true);

        Time.timeScale = 1f; // Riparti il tempo
        GameIsPaused = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- QUESTA È LA FUNZIONE CHIAVE ---
    void Pause()
    {
        AggiornaDatiPausa(); // 1. Aggiorna i testi PRIMA di mostrare il menu

        pauseMenuUI.SetActive(true);
        
        // Spegni l'HUD di gioco per pulizia visiva
        if(gameHUD != null) gameHUD.SetActive(false);

        Time.timeScale = 0f; // Ferma il tempo
        GameIsPaused = true;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void AggiornaDatiPausa()
    {
        // 1. RECUPERA IL PUNTEGGIO (Usa il Singleton che hai già creato!)
        if (ScoreManager.instance != null && testoPunteggio != null)
        {
            float punti = ScoreManager.instance.GetPunteggio();
            // Usa il Rich Text per fare il numero Giallo e Grande
            testoPunteggio.text = "Punteggio: <color=yellow><size=120%>" + punti + "</size></color>";
        }

        // 2. RECUPERA IL TEMPO (Usa il riferimento a LevelManager)
        if (levelManager != null && testoTempo != null)
        {
            float tempo = levelManager.tempoRimanente;
            
            // Formattazione Minuti:Secondi
            int minuti = Mathf.FloorToInt(tempo / 60);
            int secondi = Mathf.FloorToInt(tempo % 60);
            
            testoTempo.text = string.Format("Tempo rimanente: <color=yellow><size=120%>{0:00}:{1:00}</size></color>", minuti, secondi);
        }
    }

    // --- FUNZIONI PER I BOTTONI ---

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Importante sbloccare il tempo prima di ricaricare
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Assicurati che la scena si chiami esattamente così
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        if(settingsUI != null) settingsUI.SetActive(true);
    }

    public void CloseSettings()
    {
        if(settingsUI != null) settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco...");
        Application.Quit();
    }
}