using UnityEngine;
using UnityEngine.InputSystem; // <--- NECESSARIO PER IL NUOVO SISTEMA

[RequireComponent(typeof(CharacterController))]
public class PlayerInteractionController : MonoBehaviour
{
    [Header("INPUT SYSTEM (Trascina qui le azioni)")]
    public InputActionProperty moveAction;     // Collega qui Player/Move
    public InputActionProperty lookAction;     // Collega qui Player/Look
    public InputActionProperty interactAction; // Collega qui Player/Interact
    public InputActionProperty pauseAction;    // Collega qui Player/Pause

    [Header("Impostazioni Movimento")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Impostazioni Camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 15f;   // Sensibilità Mouse
    public float gamepadSensitivity = 100f; // Sensibilità Gamepad (spesso serve più alta)
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
    
    // Riferimento al menu per la pausa
    private GameMenuController menuController;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        menuController = FindFirstObjectByType<GameMenuController>();

        // Blocca il cursore al centro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Abilitiamo le azioni quando lo script è attivo
    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        interactAction.action.Enable();
        pauseAction.action.Enable();
    }

    // Disabilitiamo le azioni quando lo script si spegne (es. Game Over)
    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        interactAction.action.Disable();
        pauseAction.action.Disable();
    }

    void Update()
    {
        // Se il gioco è in pausa, non muoverti
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

        // LEGGERE L'INPUT (WASD o Levetta Sinistra)
        Vector2 inputMove = moveAction.action.ReadValue<Vector2>();

        Vector3 move = transform.right * inputMove.x + transform.forward * inputMove.y;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Gravità
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        // LEGGERE L'INPUT (Mouse o Levetta Destra)
        Vector2 inputLook = lookAction.action.ReadValue<Vector2>();

        // Capire se stiamo usando un gamepad o un mouse per regolare la velocità
        bool isGamepad = inputLook.x != 0 && (Mathf.Abs(inputLook.x) < 2f); // I gamepad danno valori tra -1 e 1
        float sensitivity = isGamepad ? gamepadSensitivity : mouseSensitivity;

        float lookX = inputLook.x * sensitivity * Time.deltaTime;
        float lookY = inputLook.y * sensitivity * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookX);
    }

    void HandleInteraction()
    {
        // LEGGERE IL CLICK (Tasto E o Tasto A del Gamepad)
        if (interactAction.action.WasPressedThisFrame())
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                DropObject();
            }
        }
    }
    
    void HandlePause()
    {
        // LEGGERE LA PAUSA (Esc o Start)
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (menuController != null)
            {
                // Dice al menu controller di mettere in pausa
                menuController.FocusPausa();
            }
        }
    }

    // --- LOGICA DI RACCOLTA (Identica a prima) ---

    void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange, pickupLayer))
        {
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
        heldObjRb.isKinematic = true;
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.parent = holdPosition;
        
        // (Opzionale) Riproduci suono raccolta se vuoi
    }

    void DropObject()
    {
        heldObject.transform.parent = null;
        heldObjRb.isKinematic = false;
        heldObjRb.AddForce(playerCamera.transform.forward * 2f, ForceMode.Impulse);
        heldObject = null;
    }
}