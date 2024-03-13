using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu"  && !File.Exists("Spawan.txt"))
        {
            GameObject conButton = GameObject.Find("Continue Button (Legacy)");
            Button button = conButton.GetComponent<Button>();
            button.enabled = false;
            Color color = Color.white;
            color.g = .5f;
            color.b = .5f;
            color.r = .5f;
            conButton.GetComponent<Image>().color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        if (File.Exists("Spawn.txt"))
        {
            File.Delete("Spawn.txt");
        }
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void Continue ()
    {
        SceneManager.LoadSceneAsync("MainScence");
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
