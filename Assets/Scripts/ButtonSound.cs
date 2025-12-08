using UnityEngine;
using UnityEngine.EventSystems; // FONDAMENTALE per rilevare il mouse/laser

// Questo script promette di gestire l'entrata del puntatore e il click
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    // Parte quando il mouse o il laser VR entra nel bottone
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayHover();
        }
    }

    // Parte quando clicchi (o premi il grilletto VR)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayClick();
        }
    }
}