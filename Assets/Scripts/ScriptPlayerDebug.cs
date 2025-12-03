using UnityEngine;
[RequireComponent(typeof(CharacterController))]

public class ScriptPlayerDebug : MonoBehaviour
{
    [Header("Impostazioni Movimento")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Impostazioni Camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 2.0f;
    public float lookXLimit = 90.0f;

    // Variabili private
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Blocca e nasconde il cursore
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Gestione Rotazione Camera (Mouse)
        // La rotazione orizzontale ruota il CORPO del player
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseX, 0);

        // La rotazione verticale ruota solo la CAMERA
        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);


        // 2. Calcolo Movimento (WASD)
        // Determina se stiamo correndo
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        // Mantieni la direzione verticale (Y) per la gravità, ricalcola X e Z
        float movementDirectionY = moveDirection.y;
        
        // Trasforma la direzione locale in globale
        moveDirection = (transform.forward * curSpeedX) + (transform.right * curSpeedY);

        // 3. Gestione Salto e Gravità
        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump") && canMove)
            {
                // Formula fisica per il salto: v = sqrt(h * -2 * g)
                moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                // Piccola forza verso il basso per tenerlo incollato al pavimento quando cammina
                moveDirection.y = -2f; 
            }
        }
        else
        {
            // Applica la gravità se siamo in aria (mantenendo l'inerzia del salto precedente)
            moveDirection.y = movementDirectionY; 
            moveDirection.y += gravity * Time.deltaTime;
        }

        // 4. Muovi il controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
