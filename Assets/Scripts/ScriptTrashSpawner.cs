using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnerRifiuti : MonoBehaviour
{
    [Header("Impostazioni Spawn")]
    public Transform puntoDiSpawn; // Trascina qui l'oggetto vuoto da dove nascono i rifiuti
    public float tempoTraSpawn = 2.0f; // Velocità di spawn (secondi)

    [Header("Categorie Rifiuti")]
    public List<CategoriaRifiuto> categorie;

    private Coroutine spawnCoroutine;
    private bool isSpawning = false;

    [System.Serializable]
    public class CategoriaRifiuto
    {
        public string nome;       
        public bool attiva = true; 
        public List<GameObject> prefabs; 
    }

    void Start()
    {
        if (puntoDiSpawn == null) puntoDiSpawn = transform;
        StartSpawning();
    }

    // --- LOGICA DI SPAWN ---

    public void StartSpawning()
    {
        if (isSpawning) return;
        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
    }

    IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            GeneraRifiuto();
            yield return new WaitForSeconds(tempoTraSpawn);
        }
    }

    void GeneraRifiuto()
    {
        // 1. Filtra solo le categorie attive che hanno almeno un prefab
        List<CategoriaRifiuto> categorieAttive = new List<CategoriaRifiuto>();
        foreach (var cat in categorie)
        {
            if (cat.attiva && cat.prefabs.Count > 0)
                categorieAttive.Add(cat);
        }

        if (categorieAttive.Count == 0) return; 

        // 2. Scegli una categoria a caso
        CategoriaRifiuto categoriaScelta = categorieAttive[Random.Range(0, categorieAttive.Count)];

        // 3. Scegli un prefab a caso dentro quella categoria
        GameObject prefabScelto = categoriaScelta.prefabs[Random.Range(0, categoriaScelta.prefabs.Count)];

        // --- NUOVA MODIFICA QUI SOTTO ---
        
        // 4. Calcola una rotazione casuale sull'asse Y (da 0 a 360 gradi)
        // X e Z rimangono a 0 per non far ribaltare l'oggetto, cambia solo la direzione in cui guarda
        Quaternion rotazioneRandom = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // 5. Spawna l'oggetto applicando la rotazione
        Instantiate(prefabScelto, puntoDiSpawn.position, rotazioneRandom);
    }

    // --- FUNZIONI PER CAMBIARE LIVELLO/DIFFICOLTA' ---

    public void CambiaVelocita(float nuoviSecondi)
    {
        tempoTraSpawn = nuoviSecondi;
    }

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

    // --- AUTOMAZIONE EDITOR ---
#if UNITY_EDITOR
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
            Debug.Log($"Caricati {nuovaCat.prefabs.Count} prefabs per la categoria {nomeCartella}");
        }
    }
#endif
}