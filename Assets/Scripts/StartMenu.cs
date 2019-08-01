using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private AudioSource bgm;

    private void Start()
    {
        bgm = PersistAudio.Instance.gameObject.GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        bgm.volume = 0.3f;
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