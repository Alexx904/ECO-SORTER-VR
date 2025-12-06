using UnityEngine;

public class ScriptBinController : MonoBehaviour
{
    [Tooltip("Scrivi qui il tag esatto che questo bidone deve accettare (es. Plastica)")]
    public string tagAccettato; 

    // Questa funzione parte in automatico quando qualcosa entra nel Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Controlliamo se l'oggetto che è entrato ha il Tag giusto
        if (other.gameObject.CompareTag(tagAccettato))
        {
            Debug.Log("✅ CORRETTO! Hai buttato " + other.gameObject.name);
            
            // AGGIUNTA: Aggiungiamo 1 punto chiamando lo ScoreManager
            ScoreManager.instance.ModificaPunteggio(1f);
            
            // Distruggiamo il rifiuto per pulire la scena
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("❌ ERRORE! Questo bidone non accetta " + other.gameObject.tag);
            
            // AGGIUNTA: Togliamo 0.5 punti
            ScoreManager.instance.ModificaPunteggio(-0.5f);
            Destroy(other.gameObject);
        }
    }
}