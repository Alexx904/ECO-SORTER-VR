using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerInteractionController : MonoBehaviour
{
    [Header("INPUT SYSTEM (Trascina qui le azioni)")]
    public InputActionProperty moveAction;     
    public InputActionProperty lookAction;     
    public InputActionProperty interactAction; 
    public InputActionProperty pauseAction;    

    [Header("Impostazioni Movimento")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Impostazioni Camera")]
    public Camera playerCamera;
    
    // ORA PUOI USARE VALORI NORMALI
    // Prova Mouse = 0.5 oppure 1.0
    // Prova Gamepad = 100 o 150
    [Range(0.1f, 5f)] public float mouseSensitivity = 1f;   
    [Range(50f, 300f)] public float gamepadSensitivity = 150f; 
    
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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Trova il menu anche se ci sono più oggetti, ne prende uno valido
        menuController = Object.FindFirstObjectByType<GameMenuController>();

        // Blocca il cursore
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
        if (Time.timeScale == 0) return;

        HandleMovement();
        HandleLook();
        HandleInteraction();
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
        Vector2 inputLook = lookAction.action.ReadValue<Vector2>();

        // --- FIX SENSIBILITÀ ---
        // Cerchiamo di capire se è un Gamepad o un Mouse in base all'intensità dell'input
        // Il Mouse genera valori molto alti (delta pixel), il Gamepad max 1.0
        bool isGamepad = inputLook.magnitude < 1.1f && inputLook.magnitude > 0f;

        float lookX = 0f;
        float lookY = 0f;

        if (isGamepad)
        {
            // IL GAMEPAD vuole il Time.deltaTime perché è una velocità costante
            lookX = inputLook.x * gamepadSensitivity * Time.deltaTime;
            lookY = inputLook.y * gamepadSensitivity * Time.deltaTime;
        }
        else
        {
            // IL MOUSE NON vuole il Time.deltaTime perché è uno spostamento fisico (pixel)
            // Moltiplichiamo per un fattore fisso (0.1f) per rendere i valori nell'Inspector più gestibili
            lookX = inputLook.x * mouseSensitivity * 0.1f;
            lookY = inputLook.y * mouseSensitivity * 0.1f;
        }

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
            if (menuController != null) menuController.FocusPausa();
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