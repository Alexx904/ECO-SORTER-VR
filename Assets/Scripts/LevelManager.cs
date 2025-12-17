using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Impostazioni Livello")]
    public int iDLivello = 1; // IMPORTANTE: Cambia questo numero per ogni scena (1, 2, 3...)
    public float punteggioPerTreStelle = 15f; // Il punteggio massimo previsto (es. 15)

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
    public TextMeshProUGUI testoStelleOttenute; // (Opzionale) Se vuoi scrivere "Hai preso 2 stelle!"

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
        
        // Calcoliamo i punti finali
        float puntiFinali = 0;
        if (ScoreManager.instance != null)
        {
            puntiFinali = ScoreManager.instance.GetPunteggio();
        }

        Debug.Log("🛑 TEMPO SCADUTO! Punti: " + puntiFinali);

        // --- CALCOLO E SALVATAGGIO STELLE ---
        CalcolaESalvaStelle(puntiFinali);
        // ------------------------------------

        if (pannelloGameOver != null) pannelloGameOver.SetActive(true);

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

    // NUOVA FUNZIONE PER GESTIRE LE STELLE
    void CalcolaESalvaStelle(float punteggio)
    {
        // Dividiamo il target per 3. Es: 15 / 3 = 5.
        // 1 stella = 5 punti, 2 stelle = 10 punti, 3 stelle = 15 punti.
        float step = punteggioPerTreStelle / 3f;

        int stelleGuadagnate = 0;

        if (punteggio >= punteggioPerTreStelle) 
            stelleGuadagnate = 3;
        else if (punteggio >= step * 2) 
            stelleGuadagnate = 2;
        else if (punteggio >= step) 
            stelleGuadagnate = 1;
        else 
            stelleGuadagnate = 0;

        // Creiamo una chiave unica per il salvataggio, es: "Livello_1_Stelle"
        string chiaveSalvataggio = "Livello_" + iDLivello + "_Stelle";

        // Recuperiamo il vecchio record (se non c'è, è 0)
        int recordPrecedente = PlayerPrefs.GetInt(chiaveSalvataggio, 0);

        // Se abbiamo fatto meglio di prima, salviamo il nuovo risultato
        if (stelleGuadagnate > recordPrecedente)
        {
            PlayerPrefs.SetInt(chiaveSalvataggio, stelleGuadagnate);
            PlayerPrefs.Save(); // Forza il salvataggio su disco
            Debug.Log($"Nuovo Record! Salvate {stelleGuadagnate} stelle per il Livello {iDLivello}");
        }
        
        // (Opzionale) Aggiorna un testo nel Game Over
        if(testoStelleOttenute != null)
        {
            testoStelleOttenute.text = "Stelle ottenute: " + stelleGuadagnate + "/3";
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