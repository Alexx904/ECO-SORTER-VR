using UnityEngine;
using TMPro;

/// <summary>
/// Gestisce il sistema di punteggio del gioco.
/// Si occupa di memorizzare i punti, aggiornare l'interfaccia utente (UI) e fornire l'accesso globale tramite Singleton.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Istanza statica per accedere allo ScoreManager da qualsiasi altro script (Pattern Singleton).
    /// </summary>
    public static ScoreManager instance;

    [Header("Riferimenti UI")]
    [Tooltip("Il componente TextMeshProUGUI dove verrà mostrato il punteggio.")]
    public TextMeshProUGUI whiteBoardText; 

    // Variabile interna per tenere traccia del punteggio corrente
    private float punteggioAttuale = 0;

    /// <summary>
    /// Metodo chiamato al caricamento dell'istanza dello script.
    /// Inizializza il Singleton per garantire un unico punto di accesso.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Metodo chiamato al primo frame.
    /// Imposta la grafica iniziale del punteggio a zero.
    /// </summary>
    private void Start()
    {
        AggiornaGrafica();
    }

    /// <summary>
    /// Modifica il punteggio attuale aggiungendo (o sottraendo) il valore specificato.
    /// </summary>
    /// <param name="valore">La quantità di punti da aggiungere. Usa numeri negativi per penalità.</param>
    public void ModificaPunteggio(float valore)
    {
        punteggioAttuale += valore;
        AggiornaGrafica();
    }

    /// <summary>
    /// Funzione interna per aggiornare il testo nell'interfaccia utente.
    /// </summary>
    private void AggiornaGrafica()
    {
        if (whiteBoardText != null)
        {
            // "F1" formatta il numero con una sola cifra decimale (es. 10.5)
            whiteBoardText.text = "Punteggio: " + punteggioAttuale.ToString("F1");
        }
    }

    /// <summary>
    /// Restituisce il valore numerico del punteggio attuale.
    /// Utile per altri manager per calcolare le stelle a fine partita.
    /// </summary>
    /// <returns>Il punteggio corrente (float).</returns>
    public float GetPunteggio()
    {
        return punteggioAttuale;
    }
}