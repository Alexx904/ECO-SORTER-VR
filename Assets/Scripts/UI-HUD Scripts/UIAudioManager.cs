using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager instance;

    [Header("Suoni")]
    public AudioClip clickSound;
    public AudioClip hoverSound;

    [Header("Componenti")]
    public AudioSource audioSource;

    void Awake()
    {
        // Singleton semplice per essere trovato da tutti i bottoni
        instance = this;
    }

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        if (clickSound != null) audioSource.PlayOneShot(clickSound);
    }

    public void PlayHover()
    {
        // Controllo opzionale: evita di suonare l'hover se il gioco è in pausa o fermo
        if (hoverSound != null) audioSource.PlayOneShot(hoverSound);
    }
}