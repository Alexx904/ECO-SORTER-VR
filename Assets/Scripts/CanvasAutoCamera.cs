using UnityEngine;

public class CanvasAutoCamera : MonoBehaviour
{
    public float distanzaDagliOcchi = 1.0f; // Quanto lontano sta il menu dalla faccia

    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        // 1. Cerca la camera attiva in questo momento (che sia PC o VR)
        Camera cameraAttiva = Camera.main;

        // Se per caso Camera.main non trova nulla, cerca la prima camera disponibile
        if (cameraAttiva == null)
        {
            cameraAttiva = Object.FindFirstObjectByType<Camera>();
        }

        if (cameraAttiva != null && canvas != null)
        {
            // 2. Imposta la modalità su "Screen Space - Camera"
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            
            // 3. Assegna la camera trovata
            canvas.worldCamera = cameraAttiva;

            // 4. Imposta la distanza (così non ti entra negli occhi in VR)
            canvas.planeDistance = distanzaDagliOcchi;

            Debug.Log("Canvas agganciato alla camera: " + cameraAttiva.name);
        }
    }
}