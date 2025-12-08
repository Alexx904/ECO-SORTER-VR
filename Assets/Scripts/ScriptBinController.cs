using UnityEngine;

public class ScriptBinController : MonoBehaviour
{
    [Tooltip("Scrivi qui il tag esatto che questo bidone deve accettare (es. Plastica)")]
    public string tagAccettato; 

    [Header("Audio")]
    [Tooltip("Trascina qui il file audio per quando il rifiuto è GIUSTO")]
    public AudioClip suonoCorretto; 

    [Tooltip("Trascina qui il file audio per quando il rifiuto è SBAGLIATO (Opzionale)")]
    public AudioClip suonoErrato; 

    private AudioSource audioSource;

    void Start()
    {
        // Recuperiamo il componente AudioSource che dovresti aver aggiunto al bidone
        audioSource = GetComponent<AudioSource>();
    }

    // Questa funzione parte in automatico quando qualcosa entra nel Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Controllo extra: ignoriamo oggetti che non hanno rigidbody o non sono rifiuti
        if (other.attachedRigidbody == null) return;

        // Controlliamo se l'oggetto che è entrato ha il Tag giusto
        if (other.gameObject.CompareTag(tagAccettato))
        {
            Debug.Log("✅ CORRETTO! Hai buttato " + other.gameObject.name);
            
            // AGGIUNTA: Aggiungiamo 1 punto chiamando lo ScoreManager
            if(ScoreManager.instance != null)
            {
                ScoreManager.instance.ModificaPunteggio(1f);
            }

            // --- NUOVO: RIPRODUCI AUDIO CORRETTO ---
            if (audioSource != null && suonoCorretto != null)
            {
                audioSource.PlayOneShot(suonoCorretto);
            }
            
            // Distruggiamo il rifiuto per pulire la scena
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("❌ ERRORE! Questo bidone non accetta " + other.gameObject.tag);
            
            // AGGIUNTA: Togliamo 0.5 punti
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.ModificaPunteggio(-0.5f);
            }

            // --- NUOVO: RIPRODUCI AUDIO ERRATO (Se lo hai inserito) ---
            if (audioSource != null && suonoErrato != null)
            {
                audioSource.PlayOneShot(suonoErrato);
            }

            Destroy(other.gameObject);
        }
    }
}