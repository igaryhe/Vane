using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Selection");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}