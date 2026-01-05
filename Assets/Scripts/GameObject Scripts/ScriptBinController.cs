using UnityEngine;

/// <summary>
/// Controlla la logica del singolo bidone della spazzatura.
/// Verifica la correttezza del rifiuto inserito, gestisce il punteggio e il feedback audio.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ScriptBinController : MonoBehaviour
{
    [Header("Configurazione Bidone")]
    [Tooltip("Il Tag esatto che questo bidone accetta. Deve corrispondere al Tag dell'oggetto.")]
    public string tagAccettato; 

    [Header("Feedback Audio")]
    [Tooltip("Clip audio da riprodurre quando il rifiuto è corretto.")]
    public AudioClip suonoCorretto; 

    [Tooltip("Clip audio da riprodurre quando il rifiuto è sbagliato.")]
    public AudioClip suonoErrato; 

    // Riferimento al componente AudioSource locale
    private AudioSource audioSource;

    /// <summary>
    /// Inizializza i riferimenti ai componenti necessari.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Gestisce l'evento fisico di entrata nel Trigger del bidone.
    /// Confronta i tag, aggiorna il punteggio tramite ScoreManager e distrugge l'oggetto.
    /// </summary>
    /// <param name="other">Il Collider dell'oggetto che è entrato nel bidone.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Ignora oggetti che non hanno un corpo fisico (Rigidbody) o non sono interattivi
        if (other.attachedRigidbody == null) return;

        // Verifica se il Tag dell'oggetto corrisponde a quello accettato dal bidone
        if (other.gameObject.CompareTag(tagAccettato))
        {
            Debug.Log($"✅ CORRETTO! Hai buttato: {other.gameObject.name}");
            
            // Aggiunge punti (Bonus)
            if(ScoreManager.instance != null)
            {
                ScoreManager.instance.ModificaPunteggio(1f);
            }

            // Riproduce il suono di successo
            if (audioSource != null && suonoCorretto != null)
            {
                audioSource.PlayOneShot(suonoCorretto);
            }
            
            // Rimuove il rifiuto dalla scena
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log($"❌ ERRORE! Questo bidone non accetta: {other.gameObject.tag}");
            
            // Toglie punti (Malus)
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.ModificaPunteggio(-0.5f);
            }

            // Riproduce il suono di errore
            if (audioSource != null && suonoErrato != null)
            {
                audioSource.PlayOneShot(suonoErrato);
            }

            // Rimuove comunque il rifiuto errato per pulire l'area
            Destroy(other.gameObject);
        }
    }
}