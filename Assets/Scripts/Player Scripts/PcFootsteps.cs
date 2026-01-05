using UnityEngine;

/// <summary>
/// Gestisce la riproduzione degli effetti sonori dei passi (Footsteps) basandosi sulla velocità fisica del giocatore.
/// Funziona sia con Tastiera che con Gamepad poiché analizza il movimento effettivo del CharacterController.
/// </summary>
public class PCFootsteps : MonoBehaviour
{
    [Header("Riferimenti Componenti")]
    [Tooltip("L'AudioSource che emetterà il suono.")]
    public AudioSource audioSource;
    [Tooltip("Il CharacterController da cui leggere la velocità di movimento.")]
    public CharacterController characterController;

    [Header("Assets Audio")]
    [Tooltip("La clip audio del passo")]
    public AudioClip suonoPasso;

    [Header("Configurazione Ritmo")]
    [Tooltip("Intervallo in secondi tra un passo e l'altro durante la camminata normale.")]
    public float intervalloCamminata = 0.5f;
    
    [Tooltip("Intervallo in secondi tra un passo e l'altro durante la corsa.")]
    public float intervalloCorsa = 0.3f;
    
    [Tooltip("Soglia di velocità oltre la quale il gioco considera che stai 'correndo'.")]
    public float sogliaCorsa = 6.0f; 

    [Tooltip("Variazione casuale del tono (Pitch) per rendere i passi meno ripetitivi.")]
    [Range(0.8f, 1.2f)]
    public float variazioneTono = 0.1f;

    // Variabile interna per gestire il timing
    private float prossimoPasso = 0;

    /// <summary>
    /// Inizializza i riferimenti se non sono stati assegnati manualmente nell'Inspector.
    /// </summary>
    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Controlla ad ogni frame se il giocatore si sta muovendo e se è il momento di riprodurre il suono.
    /// </summary>
    void Update()
    {
        // Controllo di sicurezza: se mancano i componenti, non fa nulla per evitare errori
        if (characterController == null || audioSource == null || suonoPasso == null) return;

        // 1. Controllo se il giocatore tocca terra (isGrounded)
        // Se siamo in aria (salto o caduta), non dobbiamo riprodurre passi.
        if (characterController.isGrounded)
        {
            // 2. Calcolo della velocità orizzontale
            // Ignoriamo la velocità verticale (Y) per evitare che cadere conti come "camminare".
            Vector3 velocitaOrizzontale = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
            float speed = velocitaOrizzontale.magnitude;

            // 3. Verifica se ci stiamo muovendo
            // Usiamo una soglia minima (0.1f) per evitare suoni se il player scivola impercettibilmente.
            if (speed > 0.1f)
            {
                // Determina se stiamo correndo o camminando basandosi sulla velocità attuale
                // (Questo sostituisce il controllo del tasto Shift, rendendolo compatibile con il Gamepad)
                float intervalloAttuale = (speed > sogliaCorsa) ? intervalloCorsa : intervalloCamminata;

                // Gestione del Timer
                if (Time.time >= prossimoPasso)
                {
                    Suona();
                    prossimoPasso = Time.time + intervalloAttuale;
                }
            }
        }
    }

    /// <summary>
    /// Riproduce il suono del passo applicando una leggera variazione di tono (Pitch) per realismo.
    /// </summary>
    void Suona()
    {
        // Resetta il pitch a 1 (normale) e aggiunge una variazione casuale
        audioSource.pitch = 1f + Random.Range(-variazioneTono, variazioneTono);
        
        // PlayOneShot permette di sovrapporre i suoni se si corre molto veloce
        audioSource.PlayOneShot(suonoPasso);
    }
}