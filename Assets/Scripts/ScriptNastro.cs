using UnityEngine;

public class ScriptNastro : MonoBehaviour

{
    public float speed = 1.0f; // Velocità modificabile da Inspector
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Blocchiamo il nastro perché non deve cadere o ruotare, deve solo spingere
        rb.isKinematic = true; 
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Sposta qualsiasi Rigidbody che sta toccando questo oggetto
        // "move back" sposta la posizione indietro rispetto alla fisica corrente
        Vector3 pos = rb.position;
        rb.position -= transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}