using UnityEngine;

/// <summary>
/// Gestisce la "Kill Zone" (zona di eliminazione).
/// Distrugge automaticamente qualsiasi oggetto che entra in questo Trigger, 
/// </summary>
public class ScriptDeletePrefabs : MonoBehaviour
{
    /// <summary>
    /// Rileva l'ingresso di un oggetto nell'area di eliminazione.
    /// Distrugge l'oggetto a meno che non sia il Giocatore.
    /// </summary>
    /// <param name="other">Il Collider dell'oggetto entrato nella zona.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Controllo di sicurezza: Impedisce la distruzione del Player
        if (other.CompareTag("Player"))
        {
            return; 
        }

        // Distrugge l'oggetto entrato
        Destroy(other.gameObject);
        
        Debug.Log($"Oggetto rimosso dalla scena: {other.name}");
    }
}