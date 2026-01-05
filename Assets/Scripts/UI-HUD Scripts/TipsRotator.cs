using UnityEngine;
using TMPro;

public class TipsRotator : MonoBehaviour
{
    [Header("Collegamenti")]
    public TextMeshProUGUI testoConsiglio;

    [Header("Impostazioni")]
    public float tempoPerOgniConsiglio = 4.0f; // Ogni quanti secondi cambia

    [Header("Lista dei Consigli")]
    [TextArea(2, 5)] // Crea un box più grande nell'Inspector per scrivere comodo
    public string[] consigli = new string[] 
    {
        "Ricordati: Le bottiglie di plastica vanno schiacciate!",
        "La carta sporca di cibo non va nella carta, ma nell'organico.",
        "Il vetro è riciclabile all'infinito!",
        "Togli sempre il tappo dai barattoli di vetro.",
        "Le scontrini fiscali non vanno nella carta (sono carta termica)!",
        "Usa meno plastica possibile per salvare gli oceani."
    };

    private float timer;
    private int indiceAttuale = 0;

    void OnEnable() // Parte ogni volta che apri il menu di pausa
    {
        timer = 0;
        MostraConsiglioCasuale(); // Appena apri, ne mostra uno a caso
    }

    void Update()
    {
        // Usiamo unscaledDeltaTime perché il gioco è in pausa (TimeScale = 0)
        timer += Time.unscaledDeltaTime;

        if (timer >= tempoPerOgniConsiglio)
        {
            CambiaConsiglio();
            timer = 0;
        }
    }

    void CambiaConsiglio()
    {
        indiceAttuale++;
        if (indiceAttuale >= consigli.Length) indiceAttuale = 0;
        
        testoConsiglio.text = consigli[indiceAttuale];
    }

    void MostraConsiglioCasuale()
    {
        indiceAttuale = Random.Range(0, consigli.Length);
        testoConsiglio.text = consigli[indiceAttuale];
    }
}