using UnityEngine;

/// <summary>
/// Controller per i menu in-game (Pausa, Game Over, Intro Livello).
/// Agisce da interfaccia tra la logica di gioco e il sistema di navigazione UI,
/// delegando la gestione tecnica dell'input al componente UIInputHandler.
/// </summary>
[RequireComponent(typeof(UIInputHandler))] // Assicura la presenza del gestore input
public class GameMenuController : MonoBehaviour
{
    /// <summary>
    /// Istanza Singleton per accesso globale facile (es. dal PlayerController).
    /// </summary>
    public static GameMenuController instance;

    // Riferimento al componente di gestione input
    private UIInputHandler inputHandler;

    [Header("Bottoni di Default (Target Focus)")]
    [Tooltip("Bottone iniziale per il pannello Intro (se presente).")]
    public GameObject bottoneIntro;    
    
    [Tooltip("Bottone iniziale per il menu di Pausa.")]
    public GameObject bottonePausa;    
    
    [Tooltip("Bottone iniziale per la schermata di Game Over.")]
    public GameObject bottoneGameOver; 
    
    [Tooltip("Bottone iniziale per il menu Impostazioni in-game.")]
    public GameObject bottoneSettings; 

    private void Awake()
    {
        instance = this;
        inputHandler = GetComponent<UIInputHandler>();
    }

    private void Start()
    {
        // Se il livello parte con un menu intro attivo, imposta subito il focus
        if (bottoneIntro != null && bottoneIntro.activeInHierarchy)
        {
            inputHandler.ImpostaSelezione(bottoneIntro);
        }
    }

    // --- API PUBBLICHE PER IL CONTROLLO FOCUS ---

    /// <summary>
    /// Sposta il focus sul menu di Pausa.
    /// </summary>
    public void FocusPausa()
    {
        inputHandler.ImpostaSelezione(bottonePausa);
    }

    /// <summary>
    /// Sposta il focus sulla schermata di Game Over.
    /// </summary>
    public void FocusGameOver()
    {
        inputHandler.ImpostaSelezione(bottoneGameOver);
    }

    /// <summary>
    /// Sposta il focus sul menu delle Impostazioni.
    /// </summary>
    public void FocusSettings()
    {
        inputHandler.ImpostaSelezione(bottoneSettings);
    }
    
    /// <summary>
    /// Sposta il focus sul menu Introduttivo.
    /// </summary>
    public void FocusIntro()
    {
        inputHandler.ImpostaSelezione(bottoneIntro);
    }
}