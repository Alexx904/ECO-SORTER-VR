using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Pannelli")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject resetPanel; // <--- NUOVO: Trascina qui il ResetPanel
    public GameObject sandboxWIPText; 

    [Header("Bottoni Sandbox")]
    public Button sandboxButton;

    private void Start()
    {
        // Assicuriamoci che solo il menu principale sia visibile all'inizio
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        if(resetPanel != null) resetPanel.SetActive(false); // Assicurati che sia chiuso
        if(sandboxWIPText) sandboxWIPText.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(true);
        if(resetPanel != null) resetPanel.SetActive(false);
    }

    // --- NUOVE FUNZIONI PER IL RESET ---

    // 1. Funzione da collegare al bottone "Reset" (quello nelle opzioni o nel menu)
    public void ApriPannelloReset()
    {
        // Nascondi gli altri pannelli per pulizia (opzionale, dipende dal tuo design)
        settingsPanel.SetActive(false); 
        
        // Mostra il pannello di conferma
        if(resetPanel != null) resetPanel.SetActive(true);
    }

    // 2. Funzione da collegare al bottone "Cancella i Progressi" (CONFERMA)
    public void ConfermaCancellazione()
    {
        // CANCELLA TUTTO (Punteggi, Livelli sbloccati, Volume, ecc.)
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // Forza il salvataggio immediato
        
        Debug.Log("⚠️ TUTTI I DATI CANCELLATI!");

        // Ricarica la scena del Menu per aggiornare visivamente i lucchetti e le stelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

    // ------------------------------------

    public void OnSandboxClicked()
    {
        Debug.Log("Sandbox è Work in Progress");
        if(sandboxWIPText) sandboxWIPText.SetActive(true);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco...");
        Application.Quit();
    }
}