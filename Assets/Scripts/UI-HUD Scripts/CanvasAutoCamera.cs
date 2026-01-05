using UnityEngine;

/// <summary>
/// Configura automaticamente il Canvas per agganciarsi alla telecamera attiva.
/// Utile in progetti ibridi (PC/VR) dove la camera principale può cambiare o essere diversa in base al dispositivo.
/// Assicura che l'interfaccia utente (UI) sia renderizzata correttamente nello spazio della camera.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class CanvasAutoCamera : MonoBehaviour
{
    [Header("Configurazione Distanza")]
    [Tooltip("Distanza in unità Unity tra la telecamera e il piano del Canvas. Regola questo valore per evitare che il menu entri 'negli occhi' in VR.")]
    public float distanzaDagliOcchi = 1.0f; 

    /// <summary>
    /// All'avvio, cerca la camera principale e configura il Canvas per utilizzarla.
    /// </summary>
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        // 1. Tenta di recuperare la camera taggata come "MainCamera"
        Camera cameraAttiva = Camera.main;

        // Fallback: Se Camera.main è nullo, cerca la prima camera attiva nella scena
        if (cameraAttiva == null)
        {
            cameraAttiva = Object.FindFirstObjectByType<Camera>();
        }

        // Se abbiamo trovato sia il Canvas che una Camera valida, procediamo al collegamento
        if (cameraAttiva != null && canvas != null)
        {
            // 2. Imposta la modalità di render su "Screen Space - Camera"
            // Questo è necessario affinché la UI segua la profondità e la prospettiva della camera
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            
            // 3. Assegna la camera trovata come responsabile del rendering UI
            canvas.worldCamera = cameraAttiva;

            // 4. Imposta la distanza di rendering (Plane Distance)
            canvas.planeDistance = distanzaDagliOcchi;

            Debug.Log($"[CanvasAutoCamera] Canvas '{name}' agganciato con successo alla camera: {cameraAttiva.name}");
        }
        else
        {
            Debug.LogWarning($"[CanvasAutoCamera] Impossibile configurare il Canvas '{name}': Camera non trovata.");
        }
    }
}