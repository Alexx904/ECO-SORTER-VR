using UnityEngine;

public class ScriptDeletePrefabs : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. Controllo di sicurezza: NON distruggere il Player se ci cammina sopra!
        if (other.CompareTag("Player"))
        {
            return; // Esce dalla funzione senza fare nulla
        }

        // 2. (Opzionale) Distruggi solo oggetti con Rigidbody o oggetti raccoglibili
        // if (other.GetComponent<Rigidbody>() == null) return;

        // 3. Distrugge l'oggetto che è entrato nel collider
        Destroy(other.gameObject);
        
        Debug.Log("Ho eliminato: " + other.name);
    }
}