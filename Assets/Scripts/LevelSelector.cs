using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] buttons;
    private void Start()
    {
        var progress = SaveSystem.Load();
        LevelData.progress = progress.level;
        var i = 0;
        foreach (var b in buttons)
        {
            if (i >= LevelData.progress)
            {
                b.interactable = !b.interactable;
            }
            i++;
        }
    }

    public void LoadLevel(int level)
    {
        LevelData.currentLevel = level;
        SceneManager.LoadScene("Level");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Start");
        }
    }
}
