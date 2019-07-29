using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] buttons;
    private const int count = 12;
    private const int col = 6;
    public Button btn;
    public GameObject content;

    private void Start()
    {
        var progress = SaveSystem.Load();
        LevelData.progress = progress.level;
        for (var i = 0; i != count; i++)
        {
            var b = Instantiate(btn);
            b.GetComponent<Transform>().SetParent(content.transform, false);
            b.GetComponent<RectTransform>().localPosition = new Vector3(50 + 100 * (i % 6), -50 - 100 * (i / 6));
            b.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            var tmp = i;
            b.onClick.AddListener(delegate { LoadLevel(tmp + 1); });
            if (i >= LevelData.progress)
            {
                b.interactable = !b.interactable;
            }
            
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
