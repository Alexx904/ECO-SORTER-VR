using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Gestisce la generazione automatica (spawning) dei rifiuti nella scena.
/// Supporta la suddivisione in categorie, la frequenza di spawn variabile e strumenti per l'automazione in Editor.
/// </summary>
public class SpawnerRifiuti : MonoBehaviour
{
    [Header("Configurazione Spawn")]
    [Tooltip("Il punto (Transform) dove verranno creati i rifiuti. Se vuoto, userà la posizione di questo oggetto.")]
    public Transform puntoDiSpawn; 
    
    [Tooltip("Intervallo di tempo in secondi tra la generazione di un rifiuto e l'altro.")]
    public float tempoTraSpawn = 2.0f; 

    [Header("Gestione Categorie")]
    [Tooltip("Lista delle categorie di rifiuti configurate (es. Carta, Plastica, Vetro).")]
    public List<CategoriaRifiuto> categorie;

    // Variabili interne per la gestione della coroutine
    private Coroutine spawnCoroutine;
    private bool isSpawning = false;

    /// <summary>
    /// Classe nidificata per organizzare i gruppi di prefabs.
    /// </summary>
    [System.Serializable]
    public class CategoriaRifiuto
    {
        [Tooltip("Nome identificativo della categoria (es. 'Plastica').")]
        public string nome;       
        [Tooltip("Se disattivata, questa categoria non verrà spawnata.")]
        public bool attiva = true; 
        [Tooltip("Elenco dei Prefab appartenenti a questa categoria.")]
        public List<GameObject> prefabs; 
    }

    /// <summary>
    /// Inizializza lo spawner all'avvio del gioco.
    /// </summary>
    void Start()
    {
        // Fallback: se non hai assegnato un punto di spawn, usa la posizione dello script stesso
        if (puntoDiSpawn == null) puntoDiSpawn = transform;
        
        StartSpawning();
    }

    /// <summary>
    /// Avvia la routine di generazione dei rifiuti.
    /// </summary>
    public void StartSpawning()
    {
        if (isSpawning) return;
        
        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    /// <summary>
    /// Interrompe la generazione dei rifiuti.
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
    }

    /// <summary>
    /// Coroutine che gestisce il loop temporale dello spawn.
    /// </summary>
    IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            GeneraRifiuto();
            yield return new WaitForSeconds(tempoTraSpawn);
        }
    }

    /// <summary>
    /// Logica principale di creazione.
    /// Seleziona una categoria attiva a caso, poi un prefab a caso e lo istanzia con rotazione casuale.
    /// </summary>
    void GeneraRifiuto()
    {
        // 1. Filtra solo le categorie attive che contengono almeno un prefab
        List<CategoriaRifiuto> categorieAttive = new List<CategoriaRifiuto>();
        foreach (var cat in categorie)
        {
            if (cat.attiva && cat.prefabs.Count > 0)
                categorieAttive.Add(cat);
        }

        // Se non ci sono categorie valide, esce per evitare errori
        if (categorieAttive.Count == 0) return; 

        // 2. Sceglie casualmente una categoria dalla lista filtrata
        CategoriaRifiuto categoriaScelta = categorieAttive[Random.Range(0, categorieAttive.Count)];

        // 3. Sceglie casualmente un prefab specifico all'interno della categoria
        GameObject prefabScelto = categoriaScelta.prefabs[Random.Range(0, categoriaScelta.prefabs.Count)];

        // 4. Calcola una rotazione casuale sull'asse Y (0-360 gradi) per variare l'estetica
        Quaternion rotazioneRandom = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // 5. Crea l'oggetto nella scena
        Instantiate(prefabScelto, puntoDiSpawn.position, rotazioneRandom);
    }

    /// <summary>
    /// Modifica la velocità di spawn in tempo reale (utile per aumentare la difficoltà).
    /// </summary>
    /// <param name="nuoviSecondi">Il nuovo tempo di attesa tra un rifiuto e l'altro.</param>
    public void CambiaVelocita(float nuoviSecondi)
    {
        tempoTraSpawn = nuoviSecondi;
    }

    /// <summary>
    /// Abilita o disabilita una specifica categoria di rifiuti.
    /// </summary>
    /// <param name="nomeCategoria">Il nome della categoria da modificare.</param>
    /// <param name="stato">True per attivare, False per disattivare.</param>
    public void AttivaCategoria(string nomeCategoria, bool stato)
    {
        foreach(var cat in categorie)
        {
            if(cat.nome == nomeCategoria)
            {
                cat.attiva = stato;
                return;
            }
        }
    }

    // --- STRUMENTI EDITOR (Non inclusi nella Build finale del gioco) ---
#if UNITY_EDITOR
    /// <summary>
    /// Strumento accessibile dal menu contestuale dell'Inspector (tasto destro sullo script).
    /// Carica automaticamente i prefab dalle cartelle del progetto nelle liste categorie.
    /// </summary>
    [ContextMenu("Carica Prefab dalle Cartelle")]
    void CaricaPrefabAutomaticamente()
    {
        string pathBase = "Assets/Prefabs/Rifiuti"; 
        string[] nomiCartelle = { "Carta", "Plastica", "Speciale", "Umido", "Vetro" };

        categorie = new List<CategoriaRifiuto>();

        foreach (string nomeCartella in nomiCartelle)
        {
            CategoriaRifiuto nuovaCat = new CategoriaRifiuto();
            nuovaCat.nome = nomeCartella;
            nuovaCat.prefabs = new List<GameObject>();

            // Cerca i file prefab nella cartella specifica
            string fullPath = pathBase + "/" + nomeCartella;
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { fullPath });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null)
                {
                    nuovaCat.prefabs.Add(prefab);
                }
            }
            
            categorie.Add(nuovaCat);
            Debug.Log($"[Automazione] Caricati {nuovaCat.prefabs.Count} prefabs per la categoria {nomeCartella}");
        }
    }
#endif
}