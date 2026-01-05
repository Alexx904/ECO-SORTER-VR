using UnityEngine;
using UnityEngine.InputSystem; 

/// <summary>
/// Controller principale del giocatore in prima persona (FPS).
/// Gestisce il movimento, la rotazione della telecamera (Mouse/Gamepad), 
/// l'interazione con gli oggetti (Pickup) e l'input di pausa.
/// Richiede il New Input System di Unity.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerInteractionController : MonoBehaviour
{
    [Header("Configurazione Input System")]
    [Tooltip("Azione per il movimento (WASD o Levetta Sinistra).")]
    public InputActionProperty moveAction;     
    
    [Tooltip("Azione per guardarsi intorno (Mouse Delta o Levetta Destra).")]
    public InputActionProperty lookAction;     
    
    [Tooltip("Azione per interagire/raccogliere oggetti (Tasto E o Bottone Sud).")]
    public InputActionProperty interactAction; 
    
    [Tooltip("Azione per mettere in pausa (ESC o Start).")]
    public InputActionProperty pauseAction;    

    [Header("Parametri Movimento")]
    [Tooltip("Velocità di camminata.")]
    public float walkSpeed = 5f;
    [Tooltip("Forza di gravità applicata al giocatore.")]
    public float gravity = -9.81f;

    [Header("Parametri Camera")]
    [Tooltip("La telecamera figlia del giocatore.")]
    public Camera playerCamera;
    
    [Range(0.1f, 5f)] 
    [Tooltip("Moltiplicatore sensibilità per il Mouse.")]
    public float mouseSensitivity = 1f;   
    
    [Range(50f, 300f)] 
    [Tooltip("Sensibilità specifica per il Gamepad (richiede valori più alti).")]
    public float gamepadSensitivity = 150f; 

    [Header("Fluidità Visuale")]
    [Tooltip("Ammorbidisce i movimenti del mouse. 0 = scattante (Raw), 0.1 = molto morbido.")]
    [Range(0.0f, 0.2f)] public float mouseSmoothing = 0.03f; 
    
    // Variabili interne per l'algoritmo di smoothing (SmoothDamp)
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseVelocity;

    // Rotazione verticale accumulata (per bloccare il movimento della visuale in verticale)
    float xRotation = 0f;

    [Header("Sistema di Raccolta (Pickup)")]
    [Tooltip("Distanza massima per raccogliere un oggetto.")]
    public float pickupRange = 3f;
    [Tooltip("Il punto (Transform) dove l'oggetto verrà tenuto in mano.")]
    public Transform holdPosition;
    [Tooltip("Layer degli oggetti che possono essere raccolti.")]
    public LayerMask pickupLayer;

    // Riferimenti ai componenti interni ed esterni
    private CharacterController controller;
    private Vector3 velocity; // Velocità attuale (usata per la gravità)
    private bool isGrounded;
    private GameObject heldObject; // L'oggetto attualmente in mano
    private Rigidbody heldObjRb;   // Il Rigidbody dell'oggetto in mano
    
    private GameMenuController menuController;
    private PauseMenu pauseMenuLogic; 

    /// <summary>
    /// Inizializzazione dei riferimenti e setup del cursore.
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Trova automaticamente i manager nella scena
        menuController = Object.FindFirstObjectByType<GameMenuController>();
        pauseMenuLogic = Object.FindFirstObjectByType<PauseMenu>(); 

        // Nasconde e blocca il cursore al centro schermo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Abilita le azioni dell'Input System quando lo script è attivo.
    /// </summary>
    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        interactAction.action.Enable();
        pauseAction.action.Enable();
    }

    /// <summary>
    /// Disabilita le azioni dell'Input System quando lo script viene spento.
    /// </summary>
    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        interactAction.action.Disable();
        pauseAction.action.Disable();
    }

    /// <summary>
    /// Loop principale di gioco.
    /// </summary>
    void Update()
    {
        // Esegue i controlli solo se il gioco non è in pausa
        if (Time.timeScale != 0)
        {
            HandleMovement();
            HandleLook();
            HandleInteraction();
        }

        // Il controllo della pausa deve funzionare sempre
        HandlePause();
    }

    /// <summary>
    /// Gestisce il movimento fisico del personaggio e la gravità.
    /// </summary>
    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        
        // Reset della velocità verticale se siamo a terra (mantiene il player incollato al suolo)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Legge l'input di movimento (Vector2)
        Vector2 inputMove = moveAction.action.ReadValue<Vector2>();
        
        // Trasforma l'input in direzione relativa al giocatore (destra/avanti)
        Vector3 move = transform.right * inputMove.x + transform.forward * inputMove.y;
        
        // Applica il movimento
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Applica la gravità
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// Gestisce la rotazione della visuale.
    /// Include una logica ibrida per distinguere tra Mouse (Raw input) e Gamepad (Time based).
    /// </summary>
    void HandleLook()
    {
        Vector2 targetLook = lookAction.action.ReadValue<Vector2>();

        // Verifica se l'input proviene da un Gamepad
        bool isGamepad = false;
        if (lookAction.action.activeControl != null)
        {
            isGamepad = lookAction.action.activeControl.device is Gamepad;
        }

        float lookX = 0f;
        float lookY = 0f;

        if (isGamepad)
        {
            // LOGICA GAMEPAD: Moltiplica per Time.deltaTime per fluidità costante
            lookX = targetLook.x * gamepadSensitivity * Time.deltaTime;
            lookY = targetLook.y * gamepadSensitivity * Time.deltaTime;

            // Resetta lo smoothing del mouse per evitare conflitti
            currentMouseDelta = Vector2.zero;
            currentMouseVelocity = Vector2.zero;
        }
        else
        {
            // LOGICA MOUSE: Usa valori Raw o SmoothDamp per precisione
            if (mouseSmoothing > 0f)
            {
                currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetLook, ref currentMouseVelocity, mouseSmoothing);
            }
            else
            {
                currentMouseDelta = targetLook;
            }

            // Nota: NON usa Time.deltaTime qui perché il delta del mouse è già spaziale
            lookX = currentMouseDelta.x * mouseSensitivity * 0.1f;
            lookY = currentMouseDelta.y * mouseSensitivity * 0.1f;
        }

        // Gestione rotazione verticale (Camera) con Clamp per non ribaltarsi
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Gestione rotazione orizzontale (Corpo del Player)
        transform.Rotate(Vector3.up * lookX);
    }

    /// <summary>
    /// Gestisce l'input di interazione per raccogliere o lasciare oggetti.
    /// </summary>
    void HandleInteraction()
    {
        if (interactAction.action.WasPressedThisFrame())
        {
            if (heldObject == null) TryPickupObject();
            else DropObject();
        }
    }
    
    /// <summary>
    /// Gestisce l'input per aprire il menu di pausa.
    /// </summary>
    void HandlePause()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            // Tenta di usare il manager della pausa, altrimenti il controller menu generico
            if (pauseMenuLogic != null) pauseMenuLogic.TogglePause();
            else if (menuController != null) menuController.FocusPausa();
        }
    }

    /// <summary>
    /// Lancia un raggio (Raycast) per cercare oggetti interagibili.
    /// </summary>
    void TryPickupObject()
    {
        RaycastHit hit;
        // Lancia un raggio dal centro della telecamera in avanti
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange, pickupLayer))
        {
            // Se colpisce qualcosa con un Rigidbody, lo raccoglie
            if (hit.collider.GetComponent<Rigidbody>()) PickupObject(hit.collider.gameObject);
        }
    }

    /// <summary>
    /// Logica fisica per raccogliere l'oggetto (lo rende cinematico e lo imparenta al player).
    /// </summary>
    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        heldObjRb = obj.GetComponent<Rigidbody>();
        
        // Disabilita la fisica per non farlo cadere mentre lo teniamo
        heldObjRb.isKinematic = true;
        
        // Posiziona l'oggetto nella "mano" e lo imparenta
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.parent = holdPosition;
    }

    /// <summary>
    /// Logica fisica per lasciare l'oggetto (riattiva la fisica e applica una spinta).
    /// </summary>
    void DropObject()
    {
        heldObject.transform.parent = null;
        heldObjRb.isKinematic = false;
        
        // Lancia leggermente l'oggetto in avanti
        heldObjRb.AddForce(playerCamera.transform.forward * 2f, ForceMode.Impulse);
        
        heldObject = null;
    }
}