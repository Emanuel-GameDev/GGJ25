using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Metodo per caricare una scena specifica
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Il nome della scena non può essere vuoto!");
        }
    }

    // Metodo per ricaricare la scena attuale
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // Metodo per chiudere l'applicazione (funziona solo in build)
    public void QuitApplication()
    {
        Debug.Log("Uscita dall'applicazione...");
        Application.Quit();
    }
}
