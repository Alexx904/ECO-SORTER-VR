using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; 

/// <summary>
/// Componente per la gestione avanzata dell'input nell'interfaccia utente (UI).
/// Risolve i conflitti di focus tra Mouse e Gamepad e mantiene una "memoria storica" della navigazione
/// per ripristinare la selezione corretta quando si passa da un dispositivo all'altro.
/// </summary>
public class UIInputHandler : MonoBehaviour
{
    // Riferimento al bottone di default per la vista corrente 
    private GameObject currentDefaultButton;
    
    // Riferimento all'ultimo oggetto validamente selezionato 
    private GameObject lastSelectedObject;

    /// <summary>
    /// Loop di gestione input frame-by-frame.
    /// Monitora l'EventSystem e gli input periferici per arbitrare la priorità del focus.
    /// </summary>
    private void Update()
    {
        // Aggiornamento continuo della memoria
        // Se un oggetto è selezionato (dal sistema o dall'utente), lo registriamo.
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        // Gestione priorità cursore Mouse
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            
            // Soglia (Deadzone) di 2.0f per filtrare micro-movimenti o drift del sensore
            if (mouseDelta.magnitude > 2.0f)
            {
                // Se il mouse si muove intenzionalmente, rilasciamo il focus logico
                // per evitare l'artefatto visivo della "doppia selezione".
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        // Ripristino intelligente del focus per Gamepad
        if (Gamepad.current != null)
        {
            // Rileviamo input di navigazione (Stick o D-Pad)
            bool inputNavigazione = Gamepad.current.leftStick.ReadValue().magnitude > 0.1f || 
                                    Gamepad.current.dpad.ReadValue().magnitude > 0.1f;
            
            // Se l'utente tenta di navigare ma il focus è nullo (causa uso precedente del mouse)...
            if (inputNavigazione && EventSystem.current.currentSelectedGameObject == null)
            {
                // ...Tentiamo di ripristinare l'ultimo oggetto noto.
                if (lastSelectedObject != null && lastSelectedObject.activeInHierarchy)
                {
                    Seleziona(lastSelectedObject);
                }
                else
                {
                    // Fallback: Se la memoria è invalida, usiamo il default della pagina corrente.
                    Seleziona(currentDefaultButton);
                }
            }
        }
    }

    /// <summary>
    /// Imposta forzatamente la selezione su un bottone specifico e lo registra come nuovo Default.
    /// Da utilizzare durante le transizioni tra pannelli o menu.
    /// </summary>
    /// <param name="bottone">Il GameObject UI da selezionare.</param>
    public void ImpostaSelezione(GameObject bottone)
    {
        currentDefaultButton = bottone; // Aggiorna il punto di ripristino sicuro
        Seleziona(bottone);             // Applica la selezione immediata
    }

    /// <summary>
    /// Helper interno per applicare la selezione tramite EventSystem in modo sicuro.
    /// </summary>
    private void Seleziona(GameObject obj)
    {
        // Pulisce la selezione precedente per forzare l'evento OnSelect
        EventSystem.current.SetSelectedGameObject(null);
        
        if (obj != null && obj.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(obj);
            lastSelectedObject = obj; // Sincronizza immediatamente la memoria
        }
    }
}