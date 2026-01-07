using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; 

/// <summary>
/// COMPONENTE UNIVERSALE PER L'INPUT UI.
/// Si occupa esclusivamente di gestire il conflitto tra Mouse e Gamepad e di mantenere la "memoria" del cursore.
/// Va affiancato ai controller dei menu (es. GameMenuController, MainMenuController).
/// </summary>
public class UIInputHandler : MonoBehaviour
{
    // Il bottone "sicuro" da selezionare se la memoria è vuota (cambia a seconda del menu aperto)
    private GameObject currentDefaultButton;
    
    // L'ultimo bottone toccato (la memoria)
    private GameObject lastSelectedObject;

    private void Update()
    {
        // 1. TRACKING: Aggiorna la memoria se c'è qualcosa di selezionato
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        // 2. MOUSE: Se si muove, cancella la selezione
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.magnitude > 2.0f)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        // 3. GAMEPAD: Se si muove e non c'è selezione, ripristina la memoria o il default
        if (Gamepad.current != null)
        {
            bool inputNavigazione = Gamepad.current.leftStick.ReadValue().magnitude > 0.1f || 
                                    Gamepad.current.dpad.ReadValue().magnitude > 0.1f;
            
            if (inputNavigazione && EventSystem.current.currentSelectedGameObject == null)
            {
                if (lastSelectedObject != null && lastSelectedObject.activeInHierarchy)
                {
                    Seleziona(lastSelectedObject);
                }
                else
                {
                    Seleziona(currentDefaultButton);
                }
            }
        }
    }

    /// <summary>
    /// Metodo pubblico per forzare la selezione di un bottone e aggiornare il Default corrente.
    /// Da chiamare quando cambi pagina o apri un menu.
    /// </summary>
    public void ImpostaSelezione(GameObject bottone)
    {
        currentDefaultButton = bottone; // Aggiorna il "Paracadute"
        Seleziona(bottone);             // Seleziona subito
    }

    // Funzione interna helper
    private void Seleziona(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (obj != null && obj.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(obj);
            lastSelectedObject = obj; // Aggiorna subito la memoria
        }
    }
}