using UnityEngine;

public class PCFootsteps : MonoBehaviour
{
    [Header("Collegamenti")]
    public AudioSource audioSource;
    public CharacterController characterController;

    [Header("Audio")]
    public AudioClip suonoPasso;

    [Header("Ritmo")]
    public float intervalloCamminata = 0.5f;
    public float intervalloCorsa = 0.3f;
    [Range(0.8f, 1.2f)]
    public float variazioneTono = 0.1f;

    private float prossimoPasso = 0;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController == null || audioSource == null || suonoPasso == null) return;

        // 1. Controlliamo se stiamo premendo i tasti di movimento (WASD)
        bool stoPremendoTasti = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
                                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // 2. Controlliamo se tocchiamo terra (isGrounded)
        // Nota: Se stoPremendoTasti è vero, suoniamo anche se la velocity fisica è 0
        if (characterController.isGrounded && stoPremendoTasti)
        {
            // Calcola velocità in base a se premiamo Shift (Corsa) o no
            // Se il tuo gioco usa Shift per correre, questo cambia il ritmo
            float intervalloAttuale = Input.GetKey(KeyCode.LeftShift) ? intervalloCorsa : intervalloCamminata;

            if (Time.time >= prossimoPasso)
            {
                Suona();
                prossimoPasso = Time.time + intervalloAttuale;
            }
        }
    }

    void Suona()
    {
        // Variazione tono per realismo
        audioSource.pitch = 1f + Random.Range(-variazioneTono, variazioneTono);
        audioSource.PlayOneShot(suonoPasso);
    }
}