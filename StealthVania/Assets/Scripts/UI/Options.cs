using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private GameObject options;

    // Update is called once per frame
    private bool is_up = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) )
        {
            options.SetActive(!is_up);
            is_up = !is_up;
        }

    }
    public void resume()
    {
        options.SetActive(false);
    }

    public void main_menu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void exit()
    {
        Application.Quit();
    }
}
