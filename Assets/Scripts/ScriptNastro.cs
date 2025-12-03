using UnityEngine;

public class ScriptNastro : MonoBehaviour
{
    [Header("Impostazioni")]
    public float speed = 1.0f;

    [Header("Collegamenti")]
    [Tooltip("Trascina qui l'oggetto PaletArrow (la gomma che gira)")]
    public Rigidbody nastroMobile; // Riferimento al rigidbody del figlio

    void Start()
    {
        // Controllo di sicurezza: se ti dimentichi di collegarlo, ti avvisa
        if (nastroMobile == null)
        {
            Debug.LogError("ATTENZIONE: Non hai collegato il nastroMobile nello script!");
            return;
        }

        // Configurazione automatica della fisica
        nastroMobile.isKinematic = true;
        nastroMobile.useGravity = false;
    }

    void FixedUpdate()
    {
        if (nastroMobile == null) return;

        // Muoviamo il Rigidbody collegato, non "this.transform"
        Vector3 pos = nastroMobile.position;
        // Usiamo transform.forward del PADRE (così la direzione dipende da come ruoti l'oggetto intero)
        nastroMobile.position -= transform.forward * speed * Time.fixedDeltaTime;
        nastroMobile.MovePosition(pos);
    }
}