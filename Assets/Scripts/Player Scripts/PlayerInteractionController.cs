using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerInteractionController : MonoBehaviour
{
    [Header("INPUT SYSTEM")]
    public InputActionProperty moveAction;     
    public InputActionProperty lookAction;     
    public InputActionProperty interactAction; 
    public InputActionProperty pauseAction;    

    [Header("Impostazioni Movimento")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Impostazioni Camera")]
    public Camera playerCamera;
    
    [Range(0.1f, 5f)] public float mouseSensitivity = 1f;   
    [Range(50f, 300f)] public float gamepadSensitivity = 150f; 

    [Header("Fluidità Mouse")]
    [Tooltip("Valore consigliato tra 0.01 (molto reattivo) e 0.1 (molto morbido). 0.03-0.05 è ideale.")]
    [Range(0.0f, 0.2f)] public float mouseSmoothing = 0.03f; 
    
    // Variabili per il calcolo dello smoothing
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseVelocity;

    float xRotation = 0f;

    [Header("Impostazioni Raccolta Oggetti")]
    public float pickupRange = 3f;
    public Transform holdPosition;
    public LayerMask pickupLayer;

    // Riferimenti
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private GameObject heldObject;
    private Rigidbody heldObjRb;
    
    private GameMenuController menuController;
    private PauseMenu pauseMenuLogic; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        menuController = Object.FindFirstObjectByType<GameMenuController>();
        pauseMenuLogic = Object.FindFirstObjectByType<PauseMenu>(); 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        interactAction.action.Enable();
        pauseAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        interactAction.action.Disable();
        pauseAction.action.Disable();
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            HandleMovement();
            HandleLook();
            HandleInteraction();
        }

        HandlePause();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 inputMove = moveAction.action.ReadValue<Vector2>();
        Vector3 move = transform.right * inputMove.x + transform.forward * inputMove.y;
        
        controller.Move(move * walkSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        Vector2 targetLook = lookAction.action.ReadValue<Vector2>();

        // 1. RILEVAMENTO CORRETTO DEL DEVICE
        // Chiediamo all'Input System quale dispositivo sta mandando l'input.
        // Questo è il modo sicuro che non rompe i micro-movimenti del mouse.
        bool isGamepad = false;
        if (lookAction.action.activeControl != null)
        {
            isGamepad = lookAction.action.activeControl.device is Gamepad;
        }

        float lookX = 0f;
        float lookY = 0f;

        if (isGamepad)
        {
            // --- LOGICA GAMEPAD ---
            // Il Gamepad ha bisogno di Time.deltaTime per essere fluido e indipendente dagli FPS.
            // Usiamo 'gamepadSensitivity' (es. 150-300).
            lookX = targetLook.x * gamepadSensitivity * Time.deltaTime;
            lookY = targetLook.y * gamepadSensitivity * Time.deltaTime;

            // Reset delle variabili di smoothing del mouse per evitare "drift" se cambi input
            currentMouseDelta = Vector2.zero;
            currentMouseVelocity = Vector2.zero;
        }
        else
        {
            // --- LOGICA MOUSE ---
            // Questa è la parte che abbiamo sistemato prima e che ora funziona bene.
            if (mouseSmoothing > 0f)
            {
                currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetLook, ref currentMouseVelocity, mouseSmoothing);
            }
            else
            {
                currentMouseDelta = targetLook;
            }

            // Nota: Qui NON usiamo Time.deltaTime perché il Delta del mouse è già "spazio percorso".
            // Moltiplichiamo per 0.1f per bilanciare i valori alti dei DPI.
            lookX = currentMouseDelta.x * mouseSensitivity * 0.1f;
            lookY = currentMouseDelta.y * mouseSensitivity * 0.1f;
        }

        // --- APPLICAZIONE ROTAZIONE ---
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookX);
    }

    void HandleInteraction()
    {
        if (interactAction.action.WasPressedThisFrame())
        {
            if (heldObject == null) TryPickupObject();
            else DropObject();
        }
    }
    
    void HandlePause()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (pauseMenuLogic != null) pauseMenuLogic.TogglePause();
            else if (menuController != null) menuController.FocusPausa();
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange, pickupLayer))
        {
            if (hit.collider.GetComponent<Rigidbody>()) PickupObject(hit.collider.gameObject);
        }
    }

    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        heldObjRb = obj.GetComponent<Rigidbody>();
        heldObjRb.isKinematic = true;
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.parent = holdPosition;
    }

    void DropObject()
    {
        heldObject.transform.parent = null;
        heldObjRb.isKinematic = false;
        heldObjRb.AddForce(playerCamera.transform.forward * 2f, ForceMode.Impulse);
        heldObject = null;
    }
}