using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

/// <summary>
/// Aggiunge effetti visivi (Colore e Ingrandimento) e sonori alle etichette degli Slider.
/// Funziona sia al passaggio del Mouse (Hover) sia alla selezione tramite Controller (Select).
/// </summary>
public class SliderHighlighter : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Riferimenti UI")]
    [Tooltip("Il testo (TextMeshPro) che deve illuminarsi quando questo slider viene selezionato.")]
    public TextMeshProUGUI testoDaIlluminare; 

    [Header("Configurazione Effetti")]
    [Tooltip("Il colore del testo quando lo slider NON è attivo.")]
    public Color coloreNormale = Color.white;
    
    [Tooltip("Il colore del testo quando lo slider È attivo/selezionato.")]
    public Color coloreSelezionato = Color.yellow;
    
    [Tooltip("La scala da applicare per l'effetto 'Pop' (es. 1.1 = 10% più grande).")]
    public Vector3 scalaIngrandita = new Vector3(1.1f, 1.1f, 1f); 

    // Variabile interna per memorizzare la grandezza originale e poterla ripristinare
    private Vector3 scalaOriginale;

    /// <summary>
    /// Salva la scala iniziale dell'oggetto appena il gioco parte.
    /// </summary>
    void Awake()
    {
        if (testoDaIlluminare != null)
            scalaOriginale = testoDaIlluminare.transform.localScale;
    }

    /// <summary>
    /// Resetta la grafica ogni volta che l'oggetto viene riattivato (es. riaprendo il menu).
    /// </summary>
    void OnEnable()
    {
        DisattivaEffetto(); 
    }

    // --- GESTIONE INPUT (INTERFACCE) ---
    
    /// <summary>
    /// Chiamato quando il Controller/Tastiera seleziona questo slider.
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        AttivaEffetto();
    }

    /// <summary>
    /// Chiamato quando il cursore del Mouse entra nell'area dello slider.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        AttivaEffetto();
    }

    /// <summary>
    /// Chiamato quando il Controller/Tastiera si sposta su un altro elemento.
    /// </summary>
    public void OnDeselect(BaseEventData eventData)
    {
        DisattivaEffetto();
    }

    /// <summary>
    /// Chiamato quando il cursore del Mouse esce dall'area dello slider.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        DisattivaEffetto();
    }

    // --- LOGICA EFFETTI ---

    /// <summary>
    /// Applica le modifiche visive (Colore + Scala) e riproduce il suono di feedback.
    /// </summary>
    void AttivaEffetto()
    {
        if (testoDaIlluminare != null)
        {
            // Cambia il colore del testo
            testoDaIlluminare.color = coloreSelezionato;
            
            // Ingrandisce il testo per dare enfasi
            testoDaIlluminare.transform.localScale = scalaIngrandita;
        }

        // Riproduce l'effetto sonoro tramite il Manager Audio UI
        if (UIAudioManager.instance != null)
        {
            UIAudioManager.instance.PlayHover();
        }
    }

    /// <summary>
    /// Ripristina l'aspetto originale dell'elemento.
    /// </summary>
    void DisattivaEffetto()
    {
        if (testoDaIlluminare != null)
        {
            // Torna al colore base
            testoDaIlluminare.color = coloreNormale;

            // Torna alla dimensione originale salvata in Awake
            testoDaIlluminare.transform.localScale = scalaOriginale; 
        }
    }
}