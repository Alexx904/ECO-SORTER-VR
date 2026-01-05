using UnityEngine;

/// <summary>
/// Manager audio centralizzato per l'interfaccia utente (UI).
/// Gestisce la riproduzione dei suoni di Click e Hover per tutti i pulsanti del gioco
/// tramite un pattern Singleton accessibile globalmente.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class UIAudioManager : MonoBehaviour
{
    /// <summary>
    /// Istanza statica globale. Permette di chiamare 'UIAudioManager.instance.PlayClick()' da qualsiasi script.
    /// </summary>
    public static UIAudioManager instance;

    [Header("Assets Audio")]
    [Tooltip("Il suono da riprodurre quando si preme un pulsante.")]
    public AudioClip clickSound;
    
    [Tooltip("Il suono da riprodurre quando si passa sopra o si seleziona un pulsante.")]
    public AudioClip hoverSound;

    [Header("Componenti")]
    [Tooltip("L'AudioSource che emetterà fisicamente i suoni.")]
    public AudioSource audioSource;

    /// <summary>
    /// Inizializza il Singleton.
    /// </summary>
    void Awake()
    {
        // Imposta questo script come riferimento globale
        instance = this;
    }

    /// <summary>
    /// Recupera il componente AudioSource se non è stato assegnato manualmente.
    /// </summary>
    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    // --- FUNZIONI PUBBLICHE ---

    /// <summary>
    /// Riproduce il suono del "Click" (Conferma).
    /// Da collegare agli eventi OnClick o OnSubmit dei pulsanti.
    /// </summary>
    public void PlayClick()
    {
        // PlayOneShot è ideale per la UI perché permette ai suoni di sovrapporsi senza tagliarsi a vicenda
        if (clickSound != null && audioSource != null) 
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    /// <summary>
    /// Riproduce il suono dell'"Hover" (Selezione/Passaggio mouse).
    /// Da collegare agli eventi OnPointerEnter o OnSelect dei pulsanti.
    /// </summary>
    public void PlayHover()
    {
        if (hoverSound != null && audioSource != null) 
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}