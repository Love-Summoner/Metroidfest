using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool newgame = false;
    [SerializeField] private DataPersistenceManager dataPersistenceManager;
    // Start is called before the first frame update

    public void NewGame()
    {
        if (File.Exists("Spawn.txt"))
        {
            File.Delete("Spawn.txt");
        }
        //DataPersistenceManager.instance.NewGame();
        newgame = true;
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void Continue ()
    {
        newgame = false;
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void GoToCredits ()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
