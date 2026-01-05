using UnityEngine;
using UnityEngine.Audio; // Necessario per gestire l'AudioMixer
using UnityEngine.UI;    // Necessario per interagire con gli Slider

/// <summary>
/// Gestisce il menu delle impostazioni audio.
/// Permette al giocatore di regolare i volumi (Musica ed Effetti) tramite Slider UI,
/// convertendo i valori per l'AudioMixer e salvando le preferenze in modo persistente.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    [Header("Configurazione Audio")]
    [Tooltip("L'AudioMixer principale del progetto.")]
    public AudioMixer mainMixer;

    [Header("Riferimenti UI")]
    [Tooltip("Lo slider per il controllo del volume della Musica.")]
    public Slider musicSlider;
    [Tooltip("Lo slider per il controllo del volume degli Effetti Sonori (SFX).")]
    public Slider sfxSlider;

    // Chiavi costanti per il salvataggio dei dati su PlayerPrefs
    private const string MUSIC_KEY = "musicVolume";
    private const string SFX_KEY = "sfxVolume";

    /// <summary>
    /// Inizializza i volumi all'avvio della scena.
    /// Recupera i valori salvati (o usa il default) e aggiorna la posizione degli slider.
    /// </summary>
    void Start()
    {
        // Carica i volumi salvati (default a 1, ovvero massimo, se non esistono)
        float musicValue = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxValue = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        // Aggiorna visivamente la posizione delle maniglie degli slider
        if (musicSlider != null) musicSlider.value = musicValue;
        if (sfxSlider != null) sfxSlider.value = sfxValue;

        // Applica immediatamente il volume all'AudioMixer per evitare sbalzi all'avvio
        SetMusicVolume(musicValue);
        SetSFXVolume(sfxValue);
    }

    /// <summary>
    /// Imposta il volume del canale Musica.
    /// Questa funzione va collegata all'evento "On Value Changed" dello Slider.
    /// </summary>
    /// <param name="volume">Il valore dello slider (tra 0.0001 e 1).</param>
    public void SetMusicVolume(float volume)
    {
        // Controllo di sicurezza: Log10(0) è -infinito, quindi usiamo un valore minimo molto basso
        if (volume <= 0) volume = 0.0001f;

        // Conversione Logaritmica: Slider (0-1) -> Mixer (-80dB a 0dB)
        mainMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        
        // Salva la preferenza dell'utente
        PlayerPrefs.SetFloat(MUSIC_KEY, volume);
    }

    /// <summary>
    /// Imposta il volume del canale Effetti Sonori (SFX).
    /// Questa funzione va collegata all'evento "On Value Changed" dello Slider.
    /// </summary>
    /// <param name="volume">Il valore dello slider (tra 0.0001 e 1).</param>
    public void SetSFXVolume(float volume)
    {
        if (volume <= 0) volume = 0.0001f;

        mainMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        
        PlayerPrefs.SetFloat(SFX_KEY, volume);
    }
}