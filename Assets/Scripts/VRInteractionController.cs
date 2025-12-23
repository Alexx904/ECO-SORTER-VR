using UnityEngine;
using UnityEngine.InputSystem; 

public class VRInteractionController : MonoBehaviour
{
    [Header("CONFIGURAZIONE TASTI")]
    public InputActionProperty interactAction; // Per raccogliere (es. Grilletto)
    public InputActionProperty pauseAction;    // Per la pausa (es. Tasto Menu)

    [Header("CONFIGURAZIONE MANO")]
    public Transform handTransform; // La mano da cui parte il raggio
    public float pickupRange = 5f;
    public LayerMask pickupLayer;   // I layer degli oggetti raccoglibili
    public Transform holdPosition;  // Dove sta l'oggetto quando lo tieni in mano

    // Variabili interne (uguali a quelle del PC)
    private GameObject heldObject;
    private Rigidbody heldObjRb;
    
    // Riferimenti ai menu
    private PauseMenu pauseMenuLogic;
    private GameMenuController menuController;

    void Start()
    {
        // Trova i menu nella scena
        pauseMenuLogic = Object.FindFirstObjectByType<PauseMenu>();
        menuController = Object.FindFirstObjectByType<GameMenuController>();
    }

    void OnEnable()
    {
        interactAction.action.Enable();
        pauseAction.action.Enable();
    }

    void OnDisable()
    {
        interactAction.action.Disable();
        pauseAction.action.Disable();
    }

    void Update()
    {
        // Se il gioco non è in pausa, possiamo interagire
        if (Time.timeScale != 0)
        {
            HandleInteraction();
        }
        
        // La pausa funziona sempre
        HandlePause();
    }

    void HandleInteraction()
    {
        // Se premiamo il tasto Interazione
        if (interactAction.action.WasPressedThisFrame())
        {
            if (heldObject == null) TryPickupObject();
            else DropObject();
        }
    }
    
    void HandlePause()
    {
        // Se premiamo il tasto Pausa
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (pauseMenuLogic != null) pauseMenuLogic.TogglePause();
            else if (menuController != null) menuController.FocusPausa();
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        // Spara un raggio invisibile dalla mano in avanti
        if (Physics.Raycast(handTransform.position, handTransform.forward, out hit, pickupRange, pickupLayer))
        {
            // Se colpisce qualcosa che ha un Rigidbody (fisica)
            if (hit.collider.GetComponent<Rigidbody>()) 
            {
                PickupObject(hit.collider.gameObject);
            }
        }
    }

    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        heldObjRb = obj.GetComponent<Rigidbody>();
        
        // Disattiva la fisica dell'oggetto per non farlo cadere mentre lo tieni
        heldObjRb.isKinematic = true;
        
        // Attaccalo alla posizione "HoldPosition"
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.rotation = holdPosition.rotation;
        heldObject.transform.parent = holdPosition;
    }

    void DropObject()
    {
        // Stacca l'oggetto
        heldObject.transform.parent = null;
        heldObjRb.isKinematic = false; // Riattiva la fisica
        
        // Dagli una spinta in avanti
        heldObjRb.AddForce(handTransform.forward * 3f, ForceMode.Impulse);
        
        heldObject = null;
    }
}