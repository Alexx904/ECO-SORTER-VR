using UnityEngine;

/// <summary>
/// Gestisce la visualizzazione delle stelle ottenute (1-3) sui pulsanti di selezione livello nel Menu Principale.
/// Legge i salvataggi da PlayerPrefs e aggiorna automaticamente la grafica quando il menu viene aperto.
/// </summary>
public class LevelStarDisplay : MonoBehaviour
{
    [Header("Configurazione")]
    [Tooltip("L'ID del livello associato a questo bottone.")]
    public int livelloID = 1; 

    // Riferimenti interni agli oggetti stella
    private GameObject stella1;
    private GameObject stella2;
    private GameObject stella3;

    /// <summary>
    /// Metodo chiamato alla creazione dell'oggetto. Inizializza i riferimenti cercando i figli.
    /// </summary>
    private void Awake() 
    {
        TrovaLeStelle();
    }

    /// <summary>
    /// Metodo chiamato ogni volta che l'oggetto o il menu vengono attivati.
    /// Garantisce che le stelle siano aggiornate anche se torni al menu dopo aver giocato.
    /// </summary>
    private void OnEnable() 
    {
        // Controllo di sicurezza: se i riferimenti sono nulli, li cerca di nuovo
        if (stella1 == null) TrovaLeStelle();
        
        AggiornaStelleVisibili();
    }

    /// <summary>
    /// Cerca automaticamente gli oggetti grafici delle stelle all'interno della gerarchia del bottone.
    /// Richiede un oggetto figlio chiamato "StarsContainer" che contiene "Star1", "Star2", "Star3".
    /// </summary>
    private void TrovaLeStelle()
    {
        Transform container = transform.Find("StarsContainer");

        if (container != null)
        {
            // Trova i figli specifici per nome
            Transform t1 = container.Find("Star1");
            Transform t2 = container.Find("Star2");
            Transform t3 = container.Find("Star3");

            if (t1 != null) stella1 = t1.gameObject;
            if (t2 != null) stella2 = t2.gameObject;
            if (t3 != null) stella3 = t3.gameObject;
        }
        else
        {
            Debug.LogError($"[LevelStarDisplay] ERRORE: Non trovo l'oggetto 'StarsContainer' dentro {gameObject.name}. Controlla la gerarchia!");
        }
    }

    /// <summary>
    /// Legge il punteggio salvato nelle PlayerPrefs e accende/spegne le icone delle stelle.
    /// </summary>
    public void AggiornaStelleVisibili()
    {
        string chiave = "Livello_" + livelloID + "_Stelle";
        int stelleSalvate = PlayerPrefs.GetInt(chiave, 0);

        // 1. Reset iniziale: Spegne tutte le stelle per sicurezza
        if(stella1 != null) stella1.SetActive(false);
        if(stella2 != null) stella2.SetActive(false);
        if(stella3 != null) stella3.SetActive(false);

        // 2. Attivazione progressiva in base al record salvato
        if (stelleSalvate >= 1 && stella1 != null) stella1.SetActive(true);
        if (stelleSalvate >= 2 && stella2 != null) stella2.SetActive(true);
        if (stelleSalvate >= 3 && stella3 != null) stella3.SetActive(true);
    }
    
    /// <summary>
    /// Funzione di debug accessibile dall'Editor (tasto destro sullo script -> Resetta Dati Livello).
    /// Cancella i progressi salvati per questo specifico livello.
    /// </summary>
    [ContextMenu("Resetta Dati Livello")]
    public void ResettaDati()
    {
        string chiave = "Livello_" + livelloID + "_Stelle";
        PlayerPrefs.DeleteKey(chiave);
        
        AggiornaStelleVisibili();
        Debug.Log($"Dati resettati correttamente per: {chiave}");
    }
}