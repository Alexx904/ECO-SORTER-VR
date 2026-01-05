using UnityEngine;
using TMPro;

/// <summary>
/// Gestisce la rotazione automatica di messaggi o consigli (Tips) nell'interfaccia utente.
/// È progettato specificamente per funzionare nei menu di Pausa, utilizzando il tempo "Unscaled" 
/// per aggiornarsi anche quando il gioco è fermo (Time.timeScale = 0).
/// </summary>
public class TipsRotator : MonoBehaviour
{
    [Header("Riferimenti UI")]
    [Tooltip("Il componente TextMeshPro dove verrà mostrato il testo del consiglio.")]
    public TextMeshProUGUI testoConsiglio;

    [Header("Configurazione Tempo")]
    [Tooltip("Ogni quanti secondi deve cambiare la frase.")]
    public float tempoPerOgniConsiglio = 4.0f; 

    [Header("Database Consigli")]
    [Tooltip("Lista delle frasi da mostrare a rotazione.")]
    [TextArea(2, 5)] // Crea un box più grande nell'Inspector per scrivere comodamente
    public string[] consigli = new string[] 
    {
        "Ricordati: Le bottiglie di plastica vanno schiacciate!",
        "La carta sporca di cibo non va nella carta, ma nell'organico.",
        "Il vetro è riciclabile all'infinito!",
        "Togli sempre il tappo dai barattoli di vetro.",
        "Gli scontrini fiscali non vanno nella carta (sono carta termica)!",
        "Usa meno plastica possibile per salvare gli oceani."
    };

    // Variabili interne per il timer
    private float timer;
    private int indiceAttuale = 0;

    /// <summary>
    /// Viene chiamato ogni volta che l'oggetto viene attivato (es. aprendo il menu di pausa).
    /// Resetta il timer e mostra subito una frase nuova per non far vedere sempre la stessa.
    /// </summary>
    void OnEnable() 
    {
        timer = 0;
        MostraConsiglioCasuale(); 
    }

    /// <summary>
    /// Loop principale. Gestisce il conteggio del tempo.
    /// </summary>
    void Update()
    {
        // IMPORTANTE: Usiamo 'unscaledDeltaTime' invece di 'deltaTime'.
        // Questo perché nel menu di pausa il 'Time.timeScale' è solitamente 0.
        // unscaledDeltaTime ignora la pausa e continua a contare i secondi reali.
        timer += Time.unscaledDeltaTime;

        if (timer >= tempoPerOgniConsiglio)
        {
            CambiaConsiglio();
            timer = 0;
        }
    }

    /// <summary>
    /// Passa al prossimo consiglio nella lista in modo sequenziale.
    /// </summary>
    void CambiaConsiglio()
    {
        indiceAttuale++;
        
        // Se siamo arrivati alla fine della lista, ricomincia da capo (Loop)
        if (indiceAttuale >= consigli.Length) 
        {
            indiceAttuale = 0;
        }
        
        if (testoConsiglio != null)
        {
            testoConsiglio.text = consigli[indiceAttuale];
        }
    }

    /// <summary>
    /// Seleziona un consiglio casuale dalla lista (usato all'apertura del menu).
    /// </summary>
    void MostraConsiglioCasuale()
    {
        if (consigli.Length > 0 && testoConsiglio != null)
        {
            indiceAttuale = Random.Range(0, consigli.Length);
            testoConsiglio.text = consigli[indiceAttuale];
        }
    }
}