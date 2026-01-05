using UnityEngine;
using UnityEngine.EventSystems; // Necessario per gestire la selezione dei bottoni

/// <summary>
/// Gestisce la navigazione dell'interfaccia utente (UI) tramite Gamepad o Tastiera.
/// Si assicura che un pulsante sia sempre selezionato (evidenziato) quando si apre un menu,
/// evitando che il navigatore si perda.
/// </summary>
public class GameMenuController : MonoBehaviour
{
    /// <summary>
    /// Istanza statica per l'accesso globale (Singleton).
    /// Permette di richiamare funzioni da qualsiasi altro script.
    /// </summary>
    public static GameMenuController instance;

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
    }

    /// <summary>
    /// Al primo frame, se il menu Intro è attivo, seleziona subito il suo bottone.
    /// </summary>
    void Start()
    {
        if (bottoneIntro != null && bottoneIntro.activeInHierarchy)
        {
            Seleziona(bottoneIntro);
        }
    }

    // --- METODI PUBBLICI DI NAVIGAZIONE ---

    /// <summary>
    /// Sposta la selezione del controller sul menu di Pausa.
    /// </summary>
    public void FocusPausa()
    {
        Seleziona(bottonePausa);
    }

    /// <summary>
    /// Sposta la selezione del controller sulla schermata di Game Over.
    /// </summary>
    public void FocusGameOver()
    {
        Seleziona(bottoneGameOver);
    }

    /// <summary>
    /// Sposta la selezione del controller sul menu delle Impostazioni.
    /// </summary>
    public void FocusSettings()
    {
        Seleziona(bottoneSettings);
    }
    
    /// <summary>
    /// Sposta la selezione del controller sul menu Introduttivo.
    /// </summary>
    public void FocusIntro()
    {
        Seleziona(bottoneIntro);
    }

    // --- LOGICA CORE ---

    /// <summary>
    /// Funzione interna che forza l'EventSystem di Unity a selezionare un oggetto specifico.
    /// </summary>
    /// <param name="bottone">Il GameObject UI da evidenziare.</param>
    private void Seleziona(GameObject bottone)
    {
        // 1. Deseleziona tutto per "pulire" la memoria dell'EventSystem
        EventSystem.current.SetSelectedGameObject(null);

        // 2. Imposta il nuovo bottone come oggetto attivo
        if (bottone != null)
        {
            EventSystem.current.SetSelectedGameObject(bottone);
        }
    }
}