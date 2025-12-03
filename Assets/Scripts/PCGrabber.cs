using UnityEngine;

public class PCGrabber : MonoBehaviour
{
    [Header("Impostazioni")]
    public Transform holdPoint;
    public float grabRange = 3f;
    
    [Header("Filtri")]
    public LayerMask layerRifiuti;

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        // Debug visivo del raggio
        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.red);

        // Tasto E o Click Mouse
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (heldObject == null) TryGrab();
            else DropObject(); // Ho cambiato nome da Throw a Drop
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, grabRange, layerRifiuti))
        {
            Rigidbody targetRb = hit.collider.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                Grab(hit.collider.gameObject, targetRb);
            }
        }
    }

    void Grab(GameObject obj, Rigidbody rb)
    {
        heldObject = obj;
        heldRb = rb;
        
        // Disattiva fisica
        heldRb.isKinematic = true; 
        
        // Attacca all'HoldPoint
        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero; 
        heldObject.transform.localRotation = Quaternion.identity;
    }

    // --- QUESTA È LA PARTE MODIFICATA ---
    void DropObject()
    {
        // Stacca l'oggetto dalla "mano"
        heldObject.transform.SetParent(null);
        
        // Riattiva la gravità
        heldRb.isKinematic = false;

        // --- ABBIAMO TOLTO IL LANCIO (AddForce) ---
        // Ora cade semplicemente giù per la gravità, come se aprissi la mano.

        // Pulizia variabili
        heldObject = null;
        heldRb = null;
    }
}