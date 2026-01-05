using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

/// <summary>
/// Gestisce la logica del Menu Principale, la navigazione tra i pannelli (Impostazioni, Selezione Livelli)
/// e il supporto per la navigazione tramite Controller/Tastiera.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Riferimenti Pannelli UI")]
    [Tooltip("Il pannello principale con i bottoni 'Gioca', 'Opzioni', 'Esci'.")]
    public GameObject mainMenuPanel;
    [Tooltip("Il pannello per la selezione dei livelli.")]
    public GameObject levelSelectPanel;
    [Tooltip("Il pannello delle impostazioni.")]
    public GameObject settingsPanel;
    [Tooltip("Il pannello di conferma reset dati.")]
    public GameObject resetPanel;
    [Tooltip("Testo o pannello 'Work in Progress' per la modalità Sandbox.")]
    public GameObject sandboxWIPText; 

    [Header("Navigazione Controller (Auto-Focus)")]
    [Tooltip("Il bottone da evidenziare automaticamente quando si apre il Menu Principale.")]
    public GameObject primoBottoneMenu;      
    [Tooltip("Il bottone da evidenziare nel menu Selezione Livelli.")]
    public GameObject primoBottoneLivelli;   
    [Tooltip("Il bottone da evidenziare nel menu Opzioni.")]
    public GameObject primoBottoneOpzioni;   
    [Tooltip("Il bottone da evidenziare nel menu Reset.")]
    public GameObject primoBottoneReset;     

    [Header("Riferimenti Extra")]
    public Button sandboxButton;

    /// <summary>
    /// All'avvio della scena, mostra il menu principale.
    /// </summary>
    private void Start()
    {
        ShowMainMenu();
    }

    // --- LOGICA DI NAVIGAZIONE ---

    /// <summary>
    /// Forza l'EventSystem a selezionare un pulsante specifico.
    /// Indispensabile per permettere la navigazione con Gamepad o Tastiera appena si apre un menu.
    /// </summary>
    /// <param name="bottone">Il GameObject del bottone da evidenziare.</param>
    void SelezionaBottone(GameObject bottone)
    {
        // 1. Deseleziona l'oggetto corrente per resettare lo stato
        EventSystem.current.SetSelectedGameObject(null);
        
        // 2. Imposta il nuovo bottone come oggetto attivo
        if (bottone != null)
        {
            EventSystem.current.SetSelectedGameObject(bottone);
        }
    }

    // --- GESTIONE PANNELLI ---

    /// <summary>
    /// Attiva il Menu Principale e imposta il focus sul primo bottone.
    /// </summary>
    public void ShowMainMenu()
    {
        ResetAllPanels();
        mainMenuPanel.SetActive(true);
        SelezionaBottone(primoBottoneMenu); 
    }

    /// <summary>
    /// Attiva il menu di Selezione Livelli e imposta il focus sul Livello 1.
    /// </summary>
    public void ShowLevelSelect()
    {
        ResetAllPanels();
        levelSelectPanel.SetActive(true);
        SelezionaBottone(primoBottoneLivelli); 
    }

    /// <summary>
    /// Attiva il menu Impostazioni e imposta il focus sul primo controllo.
    /// </summary>
    public void ShowSettings()
    {
        ResetAllPanels();
        settingsPanel.SetActive(true);
        SelezionaBottone(primoBottoneOpzioni); 
    }

    /// <summary>
    /// Chiude le impostazioni e apre il pannello di conferma Reset.
    /// </summary>
    public void ApriPannelloReset()
    {
        if(settingsPanel != null) settingsPanel.SetActive(false);
        if(resetPanel != null) resetPanel.SetActive(true);
        
        SelezionaBottone(primoBottoneReset); 
    }

    /// <summary>
    /// Chiude il pannello di reset e torna alle impostazioni senza cancellare nulla.
    /// </summary>
    public void AnnullaCancellazione()
    {
        if(resetPanel != null) resetPanel.SetActive(false);
        ShowSettings(); 
    }

    /// <summary>
    /// Cancella definitivamente tutti i salvataggi (PlayerPrefs) e ricarica la scena.
    /// </summary>
    public void ConfermaCancellazione()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.LogWarning("⚠️ TUTTI I DATI SONO STATI CANCELLATI!");
        
        // Ricarica la scena corrente per aggiornare visivamente le stelle (che ora saranno 0)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Disattiva tutti i pannelli UI per garantire una transizione pulita.
    /// </summary>
    void ResetAllPanels()
    {
        if(mainMenuPanel) mainMenuPanel.SetActive(false);
        if(levelSelectPanel) levelSelectPanel.SetActive(false);
        if(settingsPanel) settingsPanel.SetActive(false);
        if(resetPanel) resetPanel.SetActive(false);
        if(sandboxWIPText) sandboxWIPText.SetActive(false);
    }

    // --- ALTRE FUNZIONI ---

    /// <summary>
    /// Gestisce il click sul pulsante Sandbox.
    /// </summary>
    public void OnSandboxClicked()
    {
        Debug.Log("Sandbox è Work in Progress");
        if(sandboxWIPText) sandboxWIPText.SetActive(true);
    }

    /// <summary>
    /// Carica una scena specifica tramite nome.
    /// </summary>
    /// <param name="sceneName">Il nome esatto della scena da caricare.</param>
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Chiude l'applicazione. Funziona solo nella Build finale (non nell'Editor).
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco...");
        Application.Quit();
    }
}