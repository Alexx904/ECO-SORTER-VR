using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controller principale per il flusso del Menu Iniziale (Main Menu).
/// Gestisce la visibilità dei pannelli (Opzioni, Livelli, Reset) e delega la logica di input
/// al componente UIInputHandler per garantire la navigazione ibrida (Mouse/Gamepad).
/// </summary>
[RequireComponent(typeof(UIInputHandler))] // Dipendenza obbligatoria
public class MainMenuController : MonoBehaviour
{
    [Header("Riferimenti Pannelli UI")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject resetPanel;
    public GameObject sandboxWIPText; 

    [Header("Bottoni di Default (Focus Iniziale)")]
    [Tooltip("Focus iniziale per il menu principale.")]
    public GameObject primoBottoneMenu;      
    [Tooltip("Focus iniziale per la selezione livelli.")]
    public GameObject primoBottoneLivelli;   
    [Tooltip("Focus iniziale per le opzioni.")]
    public GameObject primoBottoneOpzioni;   
    [Tooltip("Focus iniziale per il pannello di conferma reset.")]
    public GameObject primoBottoneReset;     

    [Header("Riferimenti Extra")]
    public Button sandboxButton;

    // Riferimento al gestore dell'input (iniettato via GetComponent)
    private UIInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponent<UIInputHandler>();
    }

    private void Start()
    {
        ShowMainMenu();
    }

    // --- GESTIONE PANNELLI E NAVIGAZIONE ---

    /// <summary>
    /// Mostra il Menu Principale e imposta il focus sul primo bottone.
    /// </summary>
    public void ShowMainMenu()
    {
        ResetAllPanels();
        mainMenuPanel.SetActive(true);
        // Delega la selezione all'handler universale
        inputHandler.ImpostaSelezione(primoBottoneMenu); 
    }

    /// <summary>
    /// Mostra il pannello Selezione Livelli e aggiorna il focus.
    /// </summary>
    public void ShowLevelSelect()
    {
        ResetAllPanels();
        levelSelectPanel.SetActive(true);
        inputHandler.ImpostaSelezione(primoBottoneLivelli); 
    }

    /// <summary>
    /// Mostra il pannello Impostazioni e aggiorna il focus.
    /// </summary>
    public void ShowSettings()
    {
        ResetAllPanels();
        settingsPanel.SetActive(true);
        inputHandler.ImpostaSelezione(primoBottoneOpzioni); 
    }

    /// <summary>
    /// Mostra il pannello di conferma Reset Dati.
    /// </summary>
    public void ApriPannelloReset()
    {
        if(settingsPanel != null) settingsPanel.SetActive(false);
        if(resetPanel != null) resetPanel.SetActive(true);
        
        inputHandler.ImpostaSelezione(primoBottoneReset); 
    }

    /// <summary>
    /// Annulla l'operazione di reset e torna alle impostazioni.
    /// </summary>
    public void AnnullaCancellazione()
    {
        if(resetPanel != null) resetPanel.SetActive(false);
        ShowSettings(); // ShowSettings gestirà il ripristino del focus corretto
    }

    /// <summary>
    /// Esegue la cancellazione dei dati e ricarica la scena.
    /// </summary>
    public void ConfermaCancellazione()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.LogWarning("⚠️ TUTTI I DATI SONO STATI CANCELLATI!");
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Disattiva tutti i pannelli per preparare una transizione pulita.
    /// </summary>
    private void ResetAllPanels()
    {
        if(mainMenuPanel) mainMenuPanel.SetActive(false);
        if(levelSelectPanel) levelSelectPanel.SetActive(false);
        if(settingsPanel) settingsPanel.SetActive(false);
        if(resetPanel) resetPanel.SetActive(false);
        if(sandboxWIPText) sandboxWIPText.SetActive(false);
    }

    // --- UTILITIES ---

    public void OnSandboxClicked()
    {
        if(sandboxWIPText) sandboxWIPText.SetActive(true);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}