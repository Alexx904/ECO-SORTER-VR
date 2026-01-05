using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

public class SliderHighlighter : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Target")]
    public TextMeshProUGUI testoDaIlluminare; 

    [Header("Impostazioni")]
    public Color coloreNormale = Color.white;
    public Color coloreSelezionato = Color.yellow;
    public Vector3 scalaIngrandita = new Vector3(1.1f, 1.1f, 1f); // 10% più grande

    // Memorizziamo la scala originale per poter tornare indietro
    private Vector3 scalaOriginale;

    void Awake()
    {
        if (testoDaIlluminare != null)
            scalaOriginale = testoDaIlluminare.transform.localScale;
    }

    void OnEnable()
    {
        DisattivaEffetto(); // Reset quando apri il menu
    }

    // --- QUANDO IL CONTROLLER O IL MOUSE ENTRANO ---
    
    // 1. Controller (Levetta)
    public void OnSelect(BaseEventData eventData)
    {
        AttivaEffetto();
    }

    // 2. Mouse (Cursore)
    public void OnPointerEnter(PointerEventData eventData)
    {
        AttivaEffetto();
    }

    // --- QUANDO IL CONTROLLER O IL MOUSE ESCONO ---

    // 1. Controller (Spostamento via)
    public void OnDeselect(BaseEventData eventData)
    {
        DisattivaEffetto();
    }

    // 2. Mouse (Uscita cursore)
    public void OnPointerExit(PointerEventData eventData)
    {
        DisattivaEffetto();
    }

    // --- LOGICA CENTRALE ---

    void AttivaEffetto()
    {
        if (testoDaIlluminare != null)
        {
            // CAMBIA COLORE
            testoDaIlluminare.color = coloreSelezionato;
            
            // INGRANDISCE (Pop!)
            testoDaIlluminare.transform.localScale = scalaIngrandita;
        }

        // SUONA L'AUDIO (Chiama il tuo manager)
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayHover();
        }
    }

    void DisattivaEffetto()
    {
        if (testoDaIlluminare != null)
        {
            // TORNA BIANCO
            testoDaIlluminare.color = coloreNormale;

            // TORNA PICCOLO
            testoDaIlluminare.transform.localScale = scalaOriginale; // O Vector3.one
        }
    }
}