using UnityEngine;

public class LevelStarDisplay : MonoBehaviour
{
    [Header("Configurazione")]
    public int livelloID = 1; // Cambia questo numero nell'inspector per ogni bottone (1, 2, 3...)

    // Non serve più che siano public per trascinarli, li troviamo da soli!
    private GameObject stella1;
    private GameObject stella2;
    private GameObject stella3;

    private void Awake() // Awake parte appena il gioco si avvia o l'oggetto viene creato
    {
        TrovaLeStelle();
    }

    private void OnEnable() // OnEnable parte ogni volta che il menu viene mostrato
    {
        // Se per caso Awake non ha fatto in tempo o le stelle sono state perse
        if (stella1 == null) TrovaLeStelle();
        
        AggiornaStelleVisibili();
    }

    // Funzione che cerca automaticamente gli oggetti figli
    private void TrovaLeStelle()
    {
        // Cerchiamo l'oggetto "StarsContainer" dentro questo bottone
        Transform container = transform.Find("StarsContainer");

        if (container != null)
        {
            // Troviamo le stelle dentro il container per nome
            // Assicurati che nella gerarchia si chiamino ESATTAMENTE "Star1", "Star2", "Star3"
            Transform t1 = container.Find("Star1");
            Transform t2 = container.Find("Star2");
            Transform t3 = container.Find("Star3");

            if (t1 != null) stella1 = t1.gameObject;
            if (t2 != null) stella2 = t2.gameObject;
            if (t3 != null) stella3 = t3.gameObject;
        }
        else
        {
            Debug.LogError("ATTENZIONE: Non trovo l'oggetto 'StarsContainer' dentro " + gameObject.name);
        }
    }

    public void AggiornaStelleVisibili()
    {
        string chiave = "Livello_" + livelloID + "_Stelle";
        int stelleSalvate = PlayerPrefs.GetInt(chiave, 0);

        // Spegni tutto (controllo null per sicurezza)
        if(stella1 != null) stella1.SetActive(false);
        if(stella2 != null) stella2.SetActive(false);
        if(stella3 != null) stella3.SetActive(false);

        // Accendi in base al punteggio
        if (stelleSalvate >= 1 && stella1 != null) stella1.SetActive(true);
        if (stelleSalvate >= 2 && stella2 != null) stella2.SetActive(true);
        if (stelleSalvate >= 3 && stella3 != null) stella3.SetActive(true);
    }
    
    [ContextMenu("Resetta Dati Livello")]
    public void ResettaDati()
    {
        string chiave = "Livello_" + livelloID + "_Stelle";
        PlayerPrefs.DeleteKey(chiave);
        AggiornaStelleVisibili();
        Debug.Log("Reset eseguito per " + chiave);
    }
}