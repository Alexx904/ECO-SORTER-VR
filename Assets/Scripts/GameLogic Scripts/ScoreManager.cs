using UnityEngine;
using TMPro; // Necessario per usare TextMeshPro

public class ScoreManager : MonoBehaviour
{
    // Creiamo un'istanza statica per poter chiamare questo script da ovunque
    public static ScoreManager instance;

    [Header("Impostazioni UI")]
    [Tooltip("Trascina qui l'oggetto Text della tua WhiteBoard")]
    public TextMeshProUGUI whiteBoardText; 

    private float punteggioAttuale = 0;

    private void Awake()
    {
        // Impostiamo questo script come Singleton
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // All'inizio del gioco aggiorniamo la scritta a 0
        AggiornaGrafica();
    }

    // Questa funzione verrà chiamata dai bidoni
    public void ModificaPunteggio(float valore)
    {
        punteggioAttuale += valore;
        AggiornaGrafica();
    }

    // Questa funzione scrive il testo sulla WhiteBoard
    private void AggiornaGrafica()
    {
        if (whiteBoardText != null)
        {
            // "F1" formatta il numero con 1 decimale (es. 10.5)
            whiteBoardText.text = "Punteggio: " + punteggioAttuale.ToString("F1");
        }
    }
    // AGGIUNGI QUESTA FUNZIONE IN FONDO
    // Serve agli altri script per sapere quanti punti hai fatto
    public float GetPunteggio()
    {
        return punteggioAttuale;
    }
}