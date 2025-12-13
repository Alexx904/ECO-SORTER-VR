using UnityEngine;
using TMPro; // Per i testi
using UnityEngine.SceneManagement; // Per cambiare scena

public class LevelManager : MonoBehaviour
{
    [Header("Impostazioni Tempo")]
    public float tempoTotale = 60f; // Durata partita in secondi
    public float tempoRimanente;
    private bool partitaInCorso = true;

    [Header("Interfaccia (UI)")]
    public TextMeshProUGUI testoTimer;       // Il conto alla rovescia in alto (00:00)
    public GameObject pannelloGameOver;      // Il pannello che appare alla fine
    public TextMeshProUGUI testoPuntiFinale; // Il testo DENTRO il pannello Game Over

    [Header("Oggetti da Bloccare (Opzionale)")]
    public GameObject spawnerRifiuti;        // Per smettere di far nascere rifiuti

    void Start()
    {
        // 1. Reset iniziale fondamentale
        Time.timeScale = 1f; // Assicura che il tempo scorra
        tempoRimanente = tempoTotale;
        partitaInCorso = true;

        // 2. Nascondi il Game Over se è attivo per sbaglio
        if (pannelloGameOver != null) pannelloGameOver.SetActive(false);
    }

    void Update()
    {
        if (partitaInCorso)
        {
            // Gestione Timer
            tempoRimanente -= Time.deltaTime;

            // Aggiorna il testo a schermo (Formato Minuti:Secondi)
            if (testoTimer != null)
            {
                int minuti = Mathf.FloorToInt(tempoRimanente / 60);
                int secondi = Mathf.FloorToInt(tempoRimanente % 60);
                testoTimer.text = string.Format("Tempo rimanente: {0:00}:{1:00}", minuti, secondi);
            }

            // Se il tempo finisce...
            if (tempoRimanente <= 0)
            {
                AttivaGameOver();
            }
        }
    }

    void AttivaGameOver()
    {
        partitaInCorso = false;
        tempoRimanente = 0; // Per evitare numeri negativi
        Debug.Log("🛑 TEMPO SCADUTO!");

        // 1. Mostra il Pannello
        if (pannelloGameOver != null) pannelloGameOver.SetActive(true);

        // 2. Recupera i punti dallo ScoreManager e scrivili
        if (testoPuntiFinale != null && ScoreManager.instance != null)
        {
            float puntiFinali = ScoreManager.instance.GetPunteggio();
            testoPuntiFinale.text = "Hai totalizzato: " + puntiFinali + " punti!";
        }

        // 3. Blocca il Gioco
        Time.timeScale = 0f; // Congela fisica e movimenti
        
        // 4. Sblocca il Mouse (per cliccare i tasti)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 5. Spegni lo Spawner (così non nascono oggetti mentre sei fermo)
        if (spawnerRifiuti != null) spawnerRifiuti.SetActive(false);
    }

    // Funzione per il bottone "Torna al Menu"
    public void TornaAlMenu()
    {
        Time.timeScale = 1f; // Riattiva il tempo prima di uscire
        SceneManager.LoadScene("MainMenu"); // Assicurati che il nome sia giusto!
    }
    
    // Funzione per il bottone "Ricomincia" (se vuoi metterlo)
    public void RicominciaLivello()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}