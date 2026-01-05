using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; 

public class ButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Componenti")]
    public Image sfondo;
    public TextMeshProUGUI testo;

    [Header("Colori")]
    public Color testoNormale = Color.white;
    public Color testoSelezionato = Color.red; // O Nero, come preferisci

    void Start()
    {
        // All'inizio nascondiamo lo sfondo (trasparente) e mettiamo il testo bianco
        ResetVisuals();
    }

    // -- QUANDO ENTRI COL MOUSE O SELEZIONI --
    public void OnPointerEnter(PointerEventData eventData) { AttivaGrafica(); }
    public void OnSelect(BaseEventData eventData)      { AttivaGrafica(); }

    // -- QUANDO ESCI COL MOUSE O DESELEZIONI --
    public void OnPointerExit(PointerEventData eventData) { ResetVisuals(); }
    public void OnDeselect(BaseEventData eventData)       { ResetVisuals(); }

    void AttivaGrafica()
    {
        // Sfondo diventa visibile (Bianco opaco)
        if (sfondo != null) sfondo.color = Color.white; 
        
        // Testo cambia colore
        if (testo != null) testo.color = testoSelezionato;
    }

    void ResetVisuals()
    {
        // Sfondo diventa invisibile (Alpha 0)
        if (sfondo != null) 
        {
            Color c = Color.white;
            c.a = 0f; // Trasparenza totale
            sfondo.color = c;
        }

        // Testo torna normale
        if (testo != null) testo.color = testoNormale;
    }
}