using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; 

/// <summary>
/// Gestisce il feedback visivo dei pulsanti dell'interfaccia utente (UI).
/// Modifica l'aspetto grafico (colore del testo e visibilità dello sfondo) quando il pulsante viene evidenziato 
/// tramite Mouse (Hover) o tramite navigazione Controller/Tastiera (Select).
/// </summary>
public class ButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Riferimenti Componenti")]
    [Tooltip("L'immagine di sfondo del pulsante (che apparirà/scomparirà).")]
    public Image sfondo;
    
    [Tooltip("Il componente di testo del pulsante.")]
    public TextMeshProUGUI testo;

    [Header("Configurazione Colori")]
    [Tooltip("Colore del testo quando il pulsante NON è selezionato.")]
    public Color testoNormale = Color.white;
    
    [Tooltip("Colore del testo quando il pulsante È selezionato o evidenziato.")]
    public Color testoSelezionato = Color.red; 

    /// <summary>
    /// Inizializza lo stato visivo del pulsante all'avvio.
    /// </summary>
    void Start()
    {
        // Imposta lo stato iniziale (non selezionato)
        ResetVisuals();
    }

    /// <summary>
    /// Quando il bottone viene disattivato, forziamo il reset grafico. Così quando riapparirà, sarà pulito.
    /// </summary>
    void OnDisable()
    {
        ResetVisuals();
    }

    // --- GESTIONE EVENTI (INTERFACCE) ---

    /// <summary>
    /// Chiamato quando il puntatore del mouse entra nell'area del pulsante.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData) 
    { 
        AttivaGrafica(); 
    }

    /// <summary>
    /// Chiamato quando il pulsante viene selezionato tramite navigazione (es. Frecce o Gamepad).
    /// </summary>
    public void OnSelect(BaseEventData eventData)      
    { 
        AttivaGrafica(); 
    }

    /// <summary>
    /// Chiamato quando il puntatore del mouse esce dall'area del pulsante.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData) 
    { 
        ResetVisuals(); 
    }

    /// <summary>
    /// Chiamato quando il pulsante perde la selezione (es. l'utente si sposta su un altro tasto).
    /// </summary>
    public void OnDeselect(BaseEventData eventData)       
    { 
        ResetVisuals(); 
    }

    /// <summary>
    /// Appena si clicca il bottone, si resetta la grafica. Questo impedisce che il bottone rimanga "selezionato" 
    /// mentre la scena cambia o il menu si chiude.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        ResetVisuals();
    }
    
    // --- LOGICA GRAFICA ---

    /// <summary>
    /// Attiva l'aspetto "Evidenziato": mostra lo sfondo e cambia il colore del testo.
    /// </summary>
    void AttivaGrafica()
    {
        // Rende visibile lo sfondo
        if (sfondo != null) sfondo.color = Color.white; 
        
        // Cambia il colore del testo per indicare la selezione
        if (testo != null) testo.color = testoSelezionato;
    }

    /// <summary>
    /// Ripristina l'aspetto "Normale": nasconde lo sfondo e ripristina il colore originale del testo.
    /// </summary>
    void ResetVisuals()
    {
        // Rende invisibile lo sfondo impostando l'Alpha a 0 (Trasparenza totale)
        if (sfondo != null) 
        {
            Color c = Color.white;
            c.a = 0f; 
            sfondo.color = c;
        }

        // Ripristina il colore standard del testo
        if (testo != null) testo.color = testoNormale;
    }
}