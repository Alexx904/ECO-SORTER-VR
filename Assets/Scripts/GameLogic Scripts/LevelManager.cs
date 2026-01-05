using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe principale per la gestione del flusso di gioco all'interno di un livello.
/// Gestisce il timer, l'avvio e la fine della partita, il calcolo del punteggio e il caricamento delle scene.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Configurazione Livello")]
    [Tooltip("L'identificativo numerico del livello corrente (es. 1, 2, 3).")]
    public int iDLivello = 1;
    [Tooltip("Il punteggio necessario per ottenere il massimo delle stelle (3 stelle).")]
    public float punteggioPerTreStelle = 15f; 

    [Header("Gestione Tempo")]
    public float tempoTotale = 60f;
    [HideInInspector] public float tempoRimanente; 
    private bool partitaInCorso = false;

    [Header("Riferimenti UI Intro")]
    public GameObject introPanel;
    public GameObject gameHUD;

    [Header("Controllo Giocatore")]
    [Tooltip("Lista degli script del giocatore da disabilitare durante i menu e abilitare in gioco.")]
    public MonoBehaviour[] scriptsPlayer; 

    [Header("Riferimenti UI Gameplay")]
    public TextMeshProUGUI testoTimer;
    public GameObject pannelloGameOver;
    public TextMeshProUGUI testoPuntiFinale;

    [Header("UI Stelle (Game Over)")]
    public GameObject stellaGameover1;
    public GameObject stellaGameover2;
    public GameObject stellaGameover3;

    [Header("Gestione Oggetti")]
    public GameObject spawnerRifiuti;

    /// <summary>
    /// Inizializza lo stato del livello, mette in pausa il tempo e prepara l'interfaccia utente.
    /// </summary>
    void Start()
    {
        // Blocca il tempo all'avvio per mostrare il menu di intro
        Time.timeScale = 0f; 
        partitaInCorso = false;

        // Disabilita i controlli del giocatore per evitare movimenti nel menu
        foreach (var script in scriptsPlayer) 
        {
            if (script != null) script.enabled = false;
        }

        // Sblocca il cursore per permettere l'interazione con i menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Configura la visibilità dei pannelli
        if (introPanel != null) introPanel.SetActive(true);
        if (gameHUD != null) gameHUD.SetActive(false);
        if (pannelloGameOver != null) pannelloGameOver.SetActive(false);
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(false);

        tempoRimanente = tempoTotale;
    }

    /// <summary>
    /// Avvia la sessione di gioco attiva, abilitando i controlli e facendo partire il timer.
    /// </summary>
    public void IniziaPartita()
    {
        // Gestione UI
        if (introPanel != null) introPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(true);
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(true);

        // Abilita i controlli del giocatore
        foreach (var script in scriptsPlayer) 
        {
            if (script != null) script.enabled = true;
        }

        // Ripristina il tempo e blocca il cursore per il gameplay
        Time.timeScale = 1f;
        partitaInCorso = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Loop principale per la gestione del countdown del timer.
    /// </summary>
    void Update()
    {
        if (partitaInCorso)
        {
            tempoRimanente -= Time.deltaTime;

            // Aggiorna il testo del timer formattandolo in Minuti:Secondi
            if (testoTimer != null)
            {
                int minuti = Mathf.FloorToInt(tempoRimanente / 60);
                int secondi = Mathf.FloorToInt(tempoRimanente % 60);
                testoTimer.text = string.Format("Tempo: {0:00}:{1:00}", minuti, secondi);
            }

            // Verifica condizione di sconfitta/fine tempo
            if (tempoRimanente <= 0)
            {
                AttivaGameOver();
            }
        }
    }

    /// <summary>
    /// Gestisce la fine della partita quando il tempo scade.
    /// Calcola il punteggio finale, assegna le stelle e mostra il menu di riepilogo.
    /// </summary>
    void AttivaGameOver()
    {
        partitaInCorso = false;
        tempoRimanente = 0;
        
        // Recupera il punteggio dal Singleton ScoreManager
        float puntiFinali = 0;
        if (ScoreManager.instance != null)
        {
            puntiFinali = ScoreManager.instance.GetPunteggio();
        }

        Debug.Log($"Fine Partita. Punteggio totale: {puntiFinali}");

        // Attiva il pannello di Game Over e gestisce il focus per il controller
        if (pannelloGameOver != null) pannelloGameOver.SetActive(true);
        if (GameMenuController.instance != null) 
        {
            GameMenuController.instance.FocusGameOver();
        }

        // Calcola le stelle basate sul punteggio ottenuto
        CalcolaESalvaStelle(puntiFinali);

        // Aggiorna il testo del punteggio nella UI finale
        if (testoPuntiFinale != null)
        {
            testoPuntiFinale.text = "Punteggio finale: " + puntiFinali.ToString("F1");
        }

        // Ferma il gioco e mostra il cursore
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disabilita spawner e controlli giocatore
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(false);
        foreach (var script in scriptsPlayer) 
        {
            if (script != null) script.enabled = false;
        }
    }

    /// <summary>
    /// Calcola il numero di stelle (0-3) in base al punteggio, salva i progressi e aggiorna la UI.
    /// </summary>
    /// <param name="punteggio">Il punteggio finale ottenuto dal giocatore.</param>
    void CalcolaESalvaStelle(float punteggio)
    {
        float step = punteggioPerTreStelle / 3f;
        int stelleGuadagnate = 0;

        // Logica di assegnazione stelle
        if (punteggio >= punteggioPerTreStelle) stelleGuadagnate = 3;
        else if (punteggio >= step * 2) stelleGuadagnate = 2;
        else if (punteggio >= step) stelleGuadagnate = 1;
        else stelleGuadagnate = 0;

        // Salvataggio persistente dei dati (PlayerPrefs) se è stato battuto il record
        string chiaveSalvataggio = "Livello_" + iDLivello + "_Stelle";
        int recordPrecedente = PlayerPrefs.GetInt(chiaveSalvataggio, 0);

        if (stelleGuadagnate > recordPrecedente)
        {
            PlayerPrefs.SetInt(chiaveSalvataggio, stelleGuadagnate);
            PlayerPrefs.Save();
        }
        
        // Aggiornamento visuale delle stelle nel pannello Game Over
        if(stellaGameover1 != null) stellaGameover1.SetActive(false);
        if(stellaGameover2 != null) stellaGameover2.SetActive(false);
        if(stellaGameover3 != null) stellaGameover3.SetActive(false);

        if (stelleGuadagnate >= 1 && stellaGameover1 != null) stellaGameover1.SetActive(true);
        if (stelleGuadagnate >= 2 && stellaGameover2 != null) stellaGameover2.SetActive(true);
        if (stelleGuadagnate >= 3 && stellaGameover3 != null) stellaGameover3.SetActive(true);
    }
    
    /// <summary>
    /// Carica la scena del Menu Principale.
    /// </summary>
    public void TornaAlMenu()
    {
        Time.timeScale = 1f; // Importante: ripristina il tempo prima di cambiare scena
        SceneManager.LoadScene("MainMenu");
    }
    
    /// <summary>
    /// Ricarica la scena corrente per riprovare il livello.
    /// </summary>
    public void RicominciaLivello()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Calcola e carica automaticamente la scena del livello successivo basandosi sull'ID corrente.
    /// </summary>
    public void CaricaProssimoLivello()
    {
        Time.timeScale = 1f;

        // Costruisce il nome della scena successiva (es. "Scena 2" se siamo al livello 1)
        string nomeProssimaScena = "Scena " + (iDLivello + 1);

        Debug.Log("Tentativo di caricamento scena: " + nomeProssimaScena);

        // Verifica se la scena esiste nel Build Settings prima di caricarla
        if (Application.CanStreamedLevelBeLoaded(nomeProssimaScena))
        {
            SceneManager.LoadScene(nomeProssimaScena);
        }
        else
        {
            Debug.LogError($"ERRORE: La scena '{nomeProssimaScena}' non è stata trovata nei Build Settings.");
            // Fallback al menu principale in caso di errore
            SceneManager.LoadScene("MainMenu");
        }
    }
}