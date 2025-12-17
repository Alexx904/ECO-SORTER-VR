using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Impostazioni Tempo")]
    public float tempoTotale = 60f;
    [HideInInspector] public float tempoRimanente; 
    private bool partitaInCorso = false;

    [Header("Intro Livello")]
    public GameObject introPanel; // Il pannello nero con il tasto "Inizia"
    public GameObject gameHUD;    // L'interfaccia di gioco (Timer/Punti) da nascondere all'inizio

    [Header("Script da Bloccare (IMPORTANTE)")]
    // Qui devi trascinare lo script "PlayerInteractionController" o "FirstPersonController"
    // Così blocchiamo il mouse senza spegnere la telecamera!
    public MonoBehaviour[] scriptsPlayer; 

    [Header("Interfaccia (UI)")]
    public TextMeshProUGUI testoTimer;
    public GameObject pannelloGameOver;
    public TextMeshProUGUI testoPuntiFinale;

    [Header("Oggetti da Controllare")]
    public GameObject spawnerRifiuti;

    void Start()
    {
        // 1. STATO INIZIALE: TUTTO FERMO
        Time.timeScale = 0f; 
        partitaInCorso = false;

        // 2. DISABILITA IL CONTROLLO DEL PLAYER (Ma lascia la Camera accesa!)
        foreach (var script in scriptsPlayer)
        {
            if(script != null) script.enabled = false;
        }

        // 3. MOSTRA IL CURSORE (Così puoi cliccare "Inizia")
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 4. Gestione Pannelli UI
        if (introPanel != null) introPanel.SetActive(true);
        if (gameHUD != null) gameHUD.SetActive(false); // Nascondi timer e punti all'inizio
        if (pannelloGameOver != null) pannelloGameOver.SetActive(false);
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(false);

        tempoRimanente = tempoTotale;
    }

    // --- QUESTA FUNZIONE VA SUL BOTTONE "INIZIA" ---
    public void IniziaPartita()
    {
        // 1. Chiudi Intro e mostra HUD
        if (introPanel != null) introPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);
        
        // 2. Avvia Spawner
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(true);

        // 3. RIATTIVA IL CONTROLLO DEL PLAYER
        foreach (var script in scriptsPlayer)
        {
            if(script != null) script.enabled = true;
        }

        // 4. Fai partire il tempo
        Time.timeScale = 1f;
        partitaInCorso = true;
        
        // 5. NASCONDI IL CURSORE (Per giocare)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (partitaInCorso)
        {
            tempoRimanente -= Time.deltaTime;

            if (testoTimer != null)
            {
                int minuti = Mathf.FloorToInt(tempoRimanente / 60);
                int secondi = Mathf.FloorToInt(tempoRimanente % 60);
                testoTimer.text = string.Format("Tempo rimanente: {0:00}:{1:00}", minuti, secondi);
            }

            if (tempoRimanente <= 0)
            {
                AttivaGameOver();
            }
        }
    }

    void AttivaGameOver()
    {
        partitaInCorso = false;
        tempoRimanente = 0;
        Debug.Log("🛑 TEMPO SCADUTO!");

        if (pannelloGameOver != null) pannelloGameOver.SetActive(true);

        if (testoPuntiFinale != null && ScoreManager.instance != null)
        {
            float puntiFinali = ScoreManager.instance.GetPunteggio();
            testoPuntiFinale.text = "Hai totalizzato: " + puntiFinali + " punti!";
        }

        Time.timeScale = 0f;
        
        // Sblocca il cursore per cliccare i menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(false);
        
        // Disabilita di nuovo i controlli player per evitare movimenti strani nel menu
        foreach (var script in scriptsPlayer)
        {
            if(script != null) script.enabled = false;
        }
    }
    
    public void TornaAlMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
    
    public void RicominciaLivello()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}