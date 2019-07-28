using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Level");
    }
}