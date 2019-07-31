using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private int count;
    private const int col = 6;
    public Button btn;
    public GameObject content;

    private void Start()
    {
        var levels = Resources.LoadAll("Levels");
        count = levels.Length - 1;
        var progress = SaveSystem.Load();
        LevelData.progress = progress.level;
        for (var i = 0; i != count; i++)
        {
            var b = Instantiate(btn);
            b.GetComponent<Transform>().SetParent(content.transform, false);
            b.GetComponent<RectTransform>().localPosition = new Vector3(50 + 100 * (i % col), -50 - 100 * (i / col));
            b.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            var tmp = i;
            b.onClick.AddListener(delegate { LoadLevel(tmp + 1); });
            if (i >= LevelData.progress)
            {
                // b.interactable = !b.interactable;
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
