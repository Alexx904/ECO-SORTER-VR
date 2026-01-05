using UnityEngine;
using UnityEngine.EventSystems; // Namespace necessario per intercettare gli eventi UI

/// <summary>
/// Gestisce il feedback audio (suoni di hover e click) per i bottoni dell'interfaccia utente (UI).
/// Supporta sia l'input via Mouse che via Controller/Tastiera implementando le interfacce di Unity.
/// </summary>
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerClickHandler, ISubmitHandler
{
    // --- GESTIONE INPUT MOUSE ---

    /// <summary>
    /// Evento chiamato quando il puntatore del mouse entra nell'area del bottone.
    /// </summary>
    /// <param name="eventData">Dati relativi all'evento del puntatore.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        SuonaHover();
    }

    /// <summary>
    /// Evento chiamato quando si clicca il bottone col mouse (rilascio del tasto).
    /// </summary>
    /// <param name="eventData">Dati relativi al click.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        SuonaClick();
    }

    // --- GESTIONE INPUT CONTROLLER / TASTIERA ---

    /// <summary>
    /// Evento chiamato quando il bottone viene selezionato tramite navigazione.
    /// </summary>
    /// <param name="eventData">Dati relativi all'evento di selezione.</param>
    public void OnSelect(BaseEventData eventData)
    {
        SuonaHover();
    }

    /// <summary>
    /// Evento chiamato quando si preme il tasto di conferma mentre il bottone è selezionato.
    /// </summary>
    /// <param name="eventData">Dati relativi all'evento di invio.</param>
    public void OnSubmit(BaseEventData eventData)
    {
        SuonaClick();
    }

    // --- LOGICA AUDIO ---

    /// <summary>
    /// Richiede al Singleton UIAudioManager di riprodurre il suono di scorrimento/hover.
    /// </summary>
    void SuonaHover()
    {
        // Controllo di sicurezza: verifica che il manager esista prima di chiamarlo
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayHover();
        }
    }

    /// <summary>
    /// Richiede al Singleton UIAudioManager di riprodurre il suono di conferma/click.
    /// </summary>
    void SuonaClick()
    {
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayClick();
        }
    }
}