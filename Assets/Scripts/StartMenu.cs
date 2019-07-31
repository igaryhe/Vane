using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public AudioSource bgm;
    public void OnClick()
    {
        SceneManager.LoadScene("Selection");
        bgm.volume = 0.3f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}