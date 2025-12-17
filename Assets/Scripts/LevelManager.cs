using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Impostazioni Livello")]
    public int iDLivello = 1;
    public float punteggioPerTreStelle = 15f; 

    [Header("Impostazioni Tempo")]
    public float tempoTotale = 60f;
    [HideInInspector] public float tempoRimanente; 
    private bool partitaInCorso = false;

    [Header("Intro Livello")]
    public GameObject introPanel;
    public GameObject gameHUD;

    [Header("Script da Bloccare")]
    public MonoBehaviour[] scriptsPlayer; 

    [Header("Interfaccia (UI)")]
    public TextMeshProUGUI testoTimer;
    public GameObject pannelloGameOver;
    public TextMeshProUGUI testoPuntiFinale;

    [Header("Stelle Game Over (Trascinale Qui)")]
    // --- NUOVE VARIABILI PER LE STELLE DEL GAME OVER ---
    public GameObject stellaGameover1;
    public GameObject stellaGameover2;
    public GameObject stellaGameover3;
    // ---------------------------------------------------

    [Header("Oggetti da Controllare")]
    public GameObject spawnerRifiuti;

    void Start()
    {
        Time.timeScale = 0f; 
        partitaInCorso = false;

        foreach (var script in scriptsPlayer) if(script != null) script.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (introPanel != null) introPanel.SetActive(true);
        if (gameHUD != null) gameHUD.SetActive(false);
        if (pannelloGameOver != null) pannelloGameOver.SetActive(false);
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(false);

        tempoRimanente = tempoTotale;
    }

    public void IniziaPartita()
    {
        if (introPanel != null) introPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(true);

        foreach (var script in scriptsPlayer) if(script != null) script.enabled = true;

        Time.timeScale = 1f;
        partitaInCorso = true;
        
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
                testoTimer.text = string.Format("Tempo: {0:00}:{1:00}", minuti, secondi);
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
        
        float puntiFinali = 0;
        if (ScoreManager.instance != null)
        {
            puntiFinali = ScoreManager.instance.GetPunteggio();
        }

        Debug.Log("🛑 TEMPO SCADUTO! Punti: " + puntiFinali);

        // --- ATTIVA IL PANNELLO ---
        if (pannelloGameOver != null) pannelloGameOver.SetActive(true);

        // --- ORA CALCOLIAMO LE STELLE E AGGIORNIAMO LA GRAFICA ---
        CalcolaESalvaStelle(puntiFinali);
        // -----------------------------------------------------------

        if (testoPuntiFinale != null)
        {
            testoPuntiFinale.text = "Punteggio finale: " + puntiFinali.ToString("F1");
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(false);
        
        foreach (var script in scriptsPlayer) if(script != null) script.enabled = false;
    }

    void CalcolaESalvaStelle(float punteggio)
    {
        float step = punteggioPerTreStelle / 3f;
        int stelleGuadagnate = 0;

        if (punteggio >= punteggioPerTreStelle) stelleGuadagnate = 3;
        else if (punteggio >= step * 2) stelleGuadagnate = 2;
        else if (punteggio >= step) stelleGuadagnate = 1;
        else stelleGuadagnate = 0;

        // 1. SALVATAGGIO DATI (Per il Menu)
        string chiaveSalvataggio = "Livello_" + iDLivello + "_Stelle";
        int recordPrecedente = PlayerPrefs.GetInt(chiaveSalvataggio, 0);

        if (stelleGuadagnate > recordPrecedente)
        {
            PlayerPrefs.SetInt(chiaveSalvataggio, stelleGuadagnate);
            PlayerPrefs.Save();
        }
        
        // 2. AGGIORNAMENTO GRAFICO (Per il Game Over attuale)
        // Prima le spegniamo tutte per sicurezza
        if(stellaGameover1 != null) stellaGameover1.SetActive(false);
        if(stellaGameover2 != null) stellaGameover2.SetActive(false);
        if(stellaGameover3 != null) stellaGameover3.SetActive(false);

        // Poi accendiamo quelle giuste
        if (stelleGuadagnate >= 1 && stellaGameover1 != null) stellaGameover1.SetActive(true);
        if (stelleGuadagnate >= 2 && stellaGameover2 != null) stellaGameover2.SetActive(true);
        if (stelleGuadagnate >= 3 && stellaGameover3 != null) stellaGameover3.SetActive(true);
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
    // --- NUOVA FUNZIONE PER IL LIVELLO SUCCESSIVO ---
    public void CaricaProssimoLivello()
    {
        // 1. Scongela il tempo (fondamentale, altrimenti il prossimo livello parte bloccato)
        Time.timeScale = 1f;

        // 2. Calcola il nome della prossima scena
        // Se siamo al livello 1, cercherà "Scena 2"
        // Nota: Assicurati che ci sia lo spazio dopo "Scena" se i tuoi file si chiamano "Scena 1"
        string nomeProssimaScena = "Scena " + (iDLivello + 1);

        Debug.Log("Caricamento in corso: " + nomeProssimaScena);

        // 3. Controlla se la scena esiste nel Build Settings (Opzionale ma utile per evitare crash)
        if (Application.CanStreamedLevelBeLoaded(nomeProssimaScena))
        {
            SceneManager.LoadScene(nomeProssimaScena);
        }
        else
        {
            Debug.LogError("ERRORE: Non trovo la scena chiamata '" + nomeProssimaScena + "'. Controlla i Build Settings!");
            // Opzionale: Se non esiste il livello successivo, torna al menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}