using UnityEngine;
using UnityEngine.InputSystem; 

/// <summary>
/// Controller per l'interazione in Realtà Virtuale (VR).
/// Permette al giocatore di afferrare oggetti a distanza usando un Raycast (puntatore laser) 
/// e di gestire il menu di pausa tramite i pulsanti del controller VR.
/// </summary>
public class VRInteractionController : MonoBehaviour
{
    [Header("Configurazione Input VR")]
    [Tooltip("L'azione dell'Input System per interagire/afferrare (solitamente il Grip o il Trigger).")]
    public InputActionProperty interactAction; 
    
    [Tooltip("L'azione dell'Input System per aprire il menu (solitamente il tasto Menu o Start).")]
    public InputActionProperty pauseAction;    

    [Header("Configurazione Mano e Fisica")]
    [Tooltip("Il Transform della mano o del controller da cui parte il raggio di interazione.")]
    public Transform handTransform; 
    
    [Tooltip("Distanza massima a cui è possibile afferrare un oggetto.")]
    public float pickupRange = 5f;
    
    [Tooltip("Layer degli oggetti che possono essere raccolti.")]
    public LayerMask pickupLayer;   
    
    [Tooltip("Il punto esatto (ancoraggio) dove l'oggetto si posizionerà quando tenuto in mano.")]
    public Transform holdPosition;  

    // Riferimenti interni per l'oggetto attualmente afferrato
    private GameObject heldObject;
    private Rigidbody heldObjRb;
    
    // Riferimenti ai manager di gioco
    private PauseMenu pauseMenuLogic;
    private GameMenuController menuController;

    /// <summary>
    /// Inizializza i riferimenti ai menu cercandoli nella scena.
    /// </summary>
    void Start()
    {
        // Trova le istanze dei menu
        pauseMenuLogic = Object.FindFirstObjectByType<PauseMenu>();
        menuController = Object.FindFirstObjectByType<GameMenuController>();
    }

    /// <summary>
    /// Attiva le azioni di input quando l'oggetto viene abilitato.
    /// </summary>
    void OnEnable()
    {
        interactAction.action.Enable();
        pauseAction.action.Enable();
    }

    /// <summary>
    /// Disattiva le azioni di input quando l'oggetto viene disabilitato.
    /// </summary>
    void OnDisable()
    {
        interactAction.action.Disable();
        pauseAction.action.Disable();
    }

    /// <summary>
    /// Loop principale. Gestisce l'input solo se il gioco non è in pausa.
    /// </summary>
    void Update()
    {
        // Se il tempo scorre (gioco attivo), controlliamo le interazioni fisiche
        if (Time.timeScale != 0)
        {
            HandleInteraction();
        }
        
        // Il tasto pausa deve essere sempre reattivo
        HandlePause();
    }

    /// <summary>
    /// Gestisce la logica di presa e rilascio degli oggetti.
    /// </summary>
    void HandleInteraction()
    {
        // Controlla se il tasto di interazione è stato premuto in questo frame
        if (interactAction.action.WasPressedThisFrame())
        {
            if (heldObject == null) TryPickupObject(); // Se la mano è vuota, prova a prendere
            else DropObject();                         // Se ha qualcosa, lascia andare
        }
    }
    
    /// <summary>
    /// Gestisce l'apertura e chiusura del menu di pausa.
    /// </summary>
    void HandlePause()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (pauseMenuLogic != null) pauseMenuLogic.TogglePause();
            else if (menuController != null) menuController.FocusPausa();
        }
    }

    /// <summary>
    /// Lancia un raggio (Raycast) dalla mano per trovare oggetti interagibili.
    /// </summary>
    void TryPickupObject()
    {
        RaycastHit hit;
        // Spara un raggio invisibile dalla mano in avanti (transform.forward)
        if (Physics.Raycast(handTransform.position, handTransform.forward, out hit, pickupRange, pickupLayer))
        {
            // Verifica se l'oggetto colpito ha un Rigidbody (è un oggetto fisico)
            if (hit.collider.GetComponent<Rigidbody>()) 
            {
                PickupObject(hit.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// Esegue la logica fisica per afferrare l'oggetto: lo rende cinematico e lo imparenta alla mano.
    /// </summary>
    /// <param name="obj">L'oggetto da afferrare.</param>
    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        heldObjRb = obj.GetComponent<Rigidbody>();
        
        // Disattiva la fisica (gravità/collisioni attive) per non farlo cadere o tremare
        heldObjRb.isKinematic = true;
        
        // Sposta l'oggetto nella posizione di presa e lo rende figlio dell'ancoraggio
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.rotation = holdPosition.rotation;
        heldObject.transform.parent = holdPosition;
    }

    /// <summary>
    /// Esegue la logica per rilasciare l'oggetto: riattiva la fisica e applica una spinta.
    /// </summary>
    void DropObject()
    {
        // Sgancia l'oggetto dalla gerarchia della mano
        heldObject.transform.parent = null;
        heldObjRb.isKinematic = false; // Riattiva la gravità
        
        // Applica una forza in avanti per "lanciare" l'oggetto
        heldObjRb.AddForce(handTransform.forward * 3f, ForceMode.Impulse);
        
        heldObject = null;
    }
}