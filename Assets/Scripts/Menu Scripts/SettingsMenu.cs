using UnityEngine;
using UnityEngine.Audio; // Necessario per il Mixer
using UnityEngine.UI;    // Necessario per gli Slider

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mainMixer; // Trascina qui il MainMixer
    public Slider musicSlider;
    public Slider sfxSlider;

    // Chiavi per salvare i dati
    private const string MUSIC_KEY = "musicVolume";
    private const string SFX_KEY = "sfxVolume";

    void Start()
    {
        // Carica i volumi salvati (o mette default a 1 se è la prima volta)
        float musicValue = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxValue = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        // Imposta la posizione visiva degli slider
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;

        // Applica subito il volume reale
        SetMusicVolume(musicValue);
        SetSFXVolume(sfxValue);
    }

    // Collega questa funzione allo slider Musica (On Value Changed)
    public void SetMusicVolume(float volume)
    {
        // Formula magica: converte 0-1 (Slider) in -80dB a 0dB (Mixer)
        // Usiamo Mathf.Log10(volume) * 20
        mainMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        
        // Salva la preferenza
        PlayerPrefs.SetFloat(MUSIC_KEY, volume);
    }

    // Collega questa funzione allo slider SFX (On Value Changed)
    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        
        PlayerPrefs.SetFloat(SFX_KEY, volume);
    }
}