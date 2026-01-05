using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // <--- AGGIUNTO: Serve per comandare il controller

public class MainMenuController : MonoBehaviour
{
    [Header("Pannelli")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject resetPanel;
    public GameObject sandboxWIPText; 

    [Header("Bottoni di Partenza (Controller/VR)")]
    // TRASCINA QUI I BOTTONI CHE SI DEVONO ACCENDERE APPENA APRI UN MENU
    public GameObject primoBottoneMenu;      // Es. Il bottone "Seleziona Livello"
    public GameObject primoBottoneLivelli;   // Es. Il bottone "Livello 1"
    public GameObject primoBottoneOpzioni;   // Es. Il bottone "Indietro" o "Audio"
    public GameObject primoBottoneReset;     // Es. Il bottone "Indietro" del reset

    [Header("Bottoni Sandbox")]
    public Button sandboxButton;

    private void Start()
    {
        ShowMainMenu();
    }

    // --- QUESTA È LA FUNZIONE CHE FA FUNZIONARE IL CONTROLLER ---
    void SelezionaBottone(GameObject bottone)
    {
        // 1. Pulisce la memoria del controller
        EventSystem.current.SetSelectedGameObject(null);
        
        // 2. Gli dice forzatamente quale nuovo bottone guardare
        if (bottone != null)
        {
            EventSystem.current.SetSelectedGameObject(bottone);
        }
    }
    // ------------------------------------------------------------

    public void ShowMainMenu()
    {
        ResetAllPanels();
        mainMenuPanel.SetActive(true);
        SelezionaBottone(primoBottoneMenu); // <-- Seleziona il bottone del menu
    }

    public void ShowLevelSelect()
    {
        ResetAllPanels();
        levelSelectPanel.SetActive(true);
        SelezionaBottone(primoBottoneLivelli); // <-- Seleziona il Livello 1
    }

    public void ShowSettings()
    {
        ResetAllPanels();
        settingsPanel.SetActive(true);
        SelezionaBottone(primoBottoneOpzioni); // <-- Seleziona il primo delle opzioni
    }

    public void ApriPannelloReset()
    {
        // Caso speciale: Chiude Settings e apre Reset
        if(settingsPanel != null) settingsPanel.SetActive(false);
        if(resetPanel != null) resetPanel.SetActive(true);
        
        SelezionaBottone(primoBottoneReset); // <-- Seleziona "Indietro" o "Conferma"
    }

    public void AnnullaCancellazione()
    {
        if(resetPanel != null) resetPanel.SetActive(false);
        // Torna alle impostazioni
        ShowSettings(); 
    }

    public void ConfermaCancellazione()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("⚠️ TUTTI I DATI CANCELLATI!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Funzione di pulizia per spegnere tutto prima di accendere un pannello nuovo
    void ResetAllPanels()
    {
        if(mainMenuPanel) mainMenuPanel.SetActive(false);
        if(levelSelectPanel) levelSelectPanel.SetActive(false);
        if(settingsPanel) settingsPanel.SetActive(false);
        if(resetPanel) resetPanel.SetActive(false);
        if(sandboxWIPText) sandboxWIPText.SetActive(false);
    }

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