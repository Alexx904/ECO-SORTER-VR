using UnityEngine;
using UnityEngine.EventSystems; // INDISPENSABILE per il controller

public class GameMenuController : MonoBehaviour
{
    // Questo trucco (Singleton) permette agli altri script di trovare questo oggetto
    // scrivendo semplicemente "GameMenuController.instance" senza trascinare nulla.
    public static GameMenuController instance;

    [Header("Bottoni di Partenza (Trascina qui i primi bottoni)")]
    public GameObject bottoneIntro;    // Es. "Inizia Partita"
    public GameObject bottonePausa;    // Es. "Riprendi"
    public GameObject bottoneGameOver; // Es. "Riprova Livello"
    public GameObject bottoneSettings; // Es. "Indietro" o Slider Volume

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Appena parte il livello, se c'è l'Intro, selezioniamo il suo bottone
        if (bottoneIntro != null && bottoneIntro.activeInHierarchy)
        {
            Seleziona(bottoneIntro);
        }
    }

    // --- FUNZIONI CHE CHIAMERAI DAGLI ALTRI SCRIPT ---

    public void FocusPausa()
    {
        Seleziona(bottonePausa);
    }

    public void FocusGameOver()
    {
        Seleziona(bottoneGameOver);
    }

    public void FocusSettings()
    {
        Seleziona(bottoneSettings);
    }
    
    public void FocusIntro()
    {
        Seleziona(bottoneIntro);
    }

    // --- IL CUORE DEL SISTEMA ---
    private void Seleziona(GameObject bottone)
    {
        // 1. Pulisce la memoria del controller
        EventSystem.current.SetSelectedGameObject(null);

        // 2. Imposta il nuovo bottone
        if (bottone != null)
        {
            EventSystem.current.SetSelectedGameObject(bottone);
        }
    }
}