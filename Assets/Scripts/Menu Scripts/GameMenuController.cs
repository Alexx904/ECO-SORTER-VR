using UnityEngine;
using UnityEngine.EventSystems; // Necessario per gestire la selezione dei bottoni

/// <summary>
/// Gestisce la navigazione dell'interfaccia utente (UI) tramite Gamepad o Tastiera.
/// Si assicura che un pulsante sia sempre selezionato (evidenziato) quando si apre un menu,
/// evitando che il navigatore si perda.
/// </summary>
[RequireComponent(typeof(UIInputHandler))]
public class GameMenuController : MonoBehaviour
{
    /// <summary>
    /// Istanza statica per l'accesso globale (Singleton).
    /// Permette di richiamare funzioni da qualsiasi altro script.
    /// </summary>
    public static GameMenuController instance;
    private UIInputHandler inputHandler;

    [Header("Bottoni di Default")]
    [Tooltip("Il bottone da evidenziare automaticamente nel menu iniziale (es. 'Gioca').")]
    public GameObject bottoneIntro;    
    
    [Tooltip("Il bottone da evidenziare nel menu di Pausa (es. 'Riprendi').")]
    public GameObject bottonePausa;    
    
    [Tooltip("Il bottone da evidenziare nella schermata di Game Over (es. 'Riprova').")]
    public GameObject bottoneGameOver; 
    
    [Tooltip("Il primo elemento selezionabile nel menu Impostazioni (es. Slider Volume o 'Indietro').")]
    public GameObject bottoneSettings; 

    /// <summary>
    /// Inizializza il Singleton all'avvio dell'oggetto.
    /// </summary>
    void Awake()
    {
        instance = this;
        inputHandler = GetComponent<UIInputHandler>();
    }

    /// <summary>
    /// Al primo frame, se il menu Intro è attivo, seleziona subito il suo bottone.
    /// </summary>
    void Start()
    {
        if (bottoneIntro != null && bottoneIntro.activeInHierarchy)
        {
            inputHandler.ImpostaSelezione(bottoneIntro );
        }
    }

    // --- METODI PUBBLICI DI NAVIGAZIONE ---

    /// <summary>
    /// Sposta la selezione del controller sul menu di Pausa.
    /// </summary>
    public void FocusPausa()
    {
        inputHandler.ImpostaSelezione(bottonePausa);
    }

    /// <summary>
    /// Sposta la selezione del controller sulla schermata di Game Over.
    /// </summary>
    public void FocusGameOver()
    {
        inputHandler.ImpostaSelezione(bottoneGameOver);
    }

    /// <summary>
    /// Sposta la selezione del controller sul menu delle Impostazioni.
    /// </summary>
    public void FocusSettings()
    {
        inputHandler.ImpostaSelezione(bottoneSettings);
    }
    
    /// <summary>
    /// Sposta la selezione del controller sul menu Introduttivo.
    /// </summary>
    public void FocusIntro()
    {
        inputHandler.ImpostaSelezione(bottoneIntro);
    }

}