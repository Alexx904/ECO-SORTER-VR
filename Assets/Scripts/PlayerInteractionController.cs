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
    
    // --- Riferimenti Menu ---
    private GameMenuController menuController;
    private PauseMenu pauseMenuLogic; // NUOVO RIFERIMENTO

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Trova i riferimenti automatici
        menuController = Object.FindFirstObjectByType<GameMenuController>();
        pauseMenuLogic = Object.FindFirstObjectByType<PauseMenu>(); // LO CERCHIAMO QUI

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
        // --- MODIFICA FONDAMENTALE ---
        // Prima qui c'era: if (Time.timeScale == 0) return;
        // L'abbiamo tolto, altrimenti il tasto Start non veniva letto durante la pausa!

        // I movimenti funzionano SOLO se il tempo scorre
        if (Time.timeScale != 0)
        {
            HandleMovement();
            HandleLook();
            HandleInteraction();
        }

        // La pausa deve funzionare SEMPRE (anche a gioco fermo)
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
        bool isGamepad = inputLook.magnitude < 1.1f && inputLook.magnitude > 0f;

        float lookX = 0f;
        float lookY = 0f;

        if (isGamepad)
        {
            lookX = inputLook.x * gamepadSensitivity * Time.deltaTime;
            lookY = inputLook.y * gamepadSensitivity * Time.deltaTime;
        }
        else
        {
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
            // --- MODIFICA: Ora chiamiamo la logica vera ---
            if (pauseMenuLogic != null) 
            {
                pauseMenuLogic.TogglePause();
            }
            // Fallback: se manca la logica, proviamo almeno a evidenziare il bottone (vecchio metodo)
            else if (menuController != null) 
            {
                menuController.FocusPausa();
            }
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