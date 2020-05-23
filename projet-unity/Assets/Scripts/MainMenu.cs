using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("level1source");
    }

    public void ListOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void QuitGame()
    {
        Debug.Log("Fermeture du jeu...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
