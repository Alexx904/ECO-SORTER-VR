using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gestisce la logica fisica del nastro trasportatore.
/// Muove i segmenti del nastro (Rigidbody) per trasportare gli oggetti appoggiati sopra.
/// </summary>
public class ScriptNastro : MonoBehaviour
{
    [Header("Impostazioni Globali")]
    [Tooltip("La velocità di scorrimento del nastro.")]
    public float speed = 0.2f;

    [Header("Collegamenti")]
    [Tooltip("Lista dei Rigidbody che compongono le parti mobili del nastro.")]
    public List<Rigidbody> nastriMobili; 

    /// <summary>
    /// Esegue il calcolo fisico del movimento a intervalli fissi.
    /// </summary>
    void FixedUpdate()
    {
        foreach (Rigidbody rb in nastriMobili)
        {
            if (rb != null)
            {
                // Memorizziamo la posizione attuale
                Vector3 pos = rb.position;
                
                // Modifichiamo direttamente la posizione del Rigidbody all'indietro
                // (Questa logica crea l'effetto di scorrimento superficiale)
                rb.position -= rb.transform.forward * speed * Time.fixedDeltaTime;
                
                // Forziamo il movimento fisico alla posizione originale per gestire le collisioni
                rb.MovePosition(pos);
            }
        }
    }
}