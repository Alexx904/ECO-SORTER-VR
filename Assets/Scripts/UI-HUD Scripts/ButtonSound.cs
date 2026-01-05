using UnityEngine;
using UnityEngine.EventSystems; // Fondamentale per il controller

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerClickHandler, ISubmitHandler
{
    // --- MOUSE (Passaggio) ---
    public void OnPointerEnter(PointerEventData eventData)
    {
        SuonaHover();
    }

    // --- CONTROLLER / TASTIERA (Spostamento levetta) ---
    public void OnSelect(BaseEventData eventData)
    {
        SuonaHover();
    }

    // --- MOUSE (Click) ---
    public void OnPointerClick(PointerEventData eventData)
    {
        SuonaClick();
    }

    // --- CONTROLLER (Tasto A / Invio) ---
    public void OnSubmit(BaseEventData eventData)
    {
        SuonaClick();
    }

    // --- FUNZIONI DI COLLEGAMENTO AL MANAGER ---
    void SuonaHover()
    {
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayHover();
        }
    }

    void SuonaClick()
    {
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayClick();
        }
    }
}