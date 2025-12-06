using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Pannelli")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject sandboxWIPText; // Opzionale: testo che appare per Sandbox

    [Header("Bottoni Sandbox")]
    public Button sandboxButton;

    private void Start()
    {
        // Assicuriamoci che solo il menu principale sia visibile all'inizio
        ShowMainMenu();
        
        // Setup Sandbox (WIP)
        // Puoi disabilitare il bottone o fargli mostrare un messaggio
        // sandboxButton.interactable = false; 
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        if(sandboxWIPText) sandboxWIPText.SetActive(false);
    }

    public void ShowLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnSandboxClicked()
    {
        // Logica per Sandbox WIP
        Debug.Log("Sandbox è Work in Progress");
        if(sandboxWIPText) sandboxWIPText.SetActive(true);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco...");
        Application.Quit();
    }
}